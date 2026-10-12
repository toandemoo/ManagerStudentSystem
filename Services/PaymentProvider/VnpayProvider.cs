using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ManagerStudentSystem.Dtos;
using ManagerStudentSystem.Services.Interfaces;
using Microsoft.Extensions.Options;
using ManagerStudentSystem.Services.PaymentProvider;
using ManagerStudentSystem.Enums;
using ManagerStudentSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using ManagerStudentSystem.Dtos.PaymentProvider;
using ManagerStudentSystem.Dtos.Refund;
using System.Text.Json;
using System.Net.Http;
using System.Net.Http.Headers;


namespace ManagerStudentSystem.Services
{
    public class VnPayProvider : IPaymentProvider
    {
        private readonly TaekwondoDbContext _context;
        private readonly VnPayOptions _options;
        private readonly HttpClient HttpClient;
        private readonly ILogger<VnPayProvider> _logger;

        public string Code => "VNPAY";

        public VnPayProvider(
            IOptions<VnPayOptions> options,
            TaekwondoDbContext context,
            HttpClient httpClient,
            ILogger<VnPayProvider> logger)
        {
            _options = options.Value;
            _context = context;
            HttpClient = httpClient;
            _logger = logger;

        }

        public async Task<PaymentProviderResultDTO> CreatePaymentAsync(PaymentProviderRequestDTO request)
        {
            var parameters = new SortedDictionary<string, string>
            {
                ["vnp_Version"] = "2.1.0",
                ["vnp_Command"] = "pay",
                ["vnp_TmnCode"] = _options.TmnCode,
                ["vnp_Amount"] = ((long)(request.Amount * 100)).ToString(),
                ["vnp_CurrCode"] = request.Currency,
                ["vnp_TxnRef"] = request.PaymentCode,
                ["vnp_OrderInfo"] = $"Thanh toan {request.PaymentCode}",
                ["vnp_OrderType"] = "other",
                ["vnp_Locale"] = "vn",
                ["vnp_ReturnUrl"] = request.ReturnUrl,
                ["vnp_IpAddr"] = "127.0.0.1",
                ["vnp_CreateDate"] = DateTime.UtcNow.ToString("yyyyMMddHHmmss"),
                // ["vnp_ExpireDate"] = expireDate
            };

            var queryString = BuildQueryString(parameters);
            var secureHash = HmacSHA512(_options.HashSecret, queryString);

            var paymentUrl =
                $"{_options.PaymentUrl}" +
                $"?{queryString}" +
                $"&vnp_SecureHash={secureHash}";

            return new PaymentProviderResultDTO
            {
                Success = true,
                PaymentUrl = paymentUrl,
                ProviderTransactionId = request.PaymentCode
            };
        }

        private static string BuildQueryString(SortedDictionary<string, string> parameters)
        {
            var data = new StringBuilder();

            foreach (var parameter in parameters)
            {
                if (string.IsNullOrEmpty(parameter.Value))
                    continue;

                if (data.Length > 0)
                    data.Append('&');

                data.Append(Utils.UrlEncode(parameter.Key));
                data.Append('=');
                data.Append(Utils.UrlEncode(parameter.Value));
            }

            return data.ToString();
        }

        private static string HmacSHA512(string key, string inputData)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] inputBytes = Encoding.UTF8.GetBytes(inputData);

            using var hmac = new HMACSHA512(keyBytes);

            byte[] hashValue = hmac.ComputeHash(inputBytes);

            var hash = new StringBuilder(hashValue.Length * 2);

            foreach (byte b in hashValue)
            {
                hash.Append(b.ToString("x2"));
            }

            return hash.ToString();
        }

        private bool VerifySignature(SortedDictionary<string, string> data, string secureHash)
        {
            var hashData = string.Join(
                "&",
                data.Select(x => $"{x.Key}={Utils.UrlEncode(x.Value)}"));

            using var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(secureHash));

            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(hashData));

            var calculatedHash = Convert.ToHexString(hash);

            return string.Equals(
                calculatedHash,
                secureHash,
                StringComparison.OrdinalIgnoreCase);
        }

        public async Task<PaymentProviderResultDTO> ProcessReturnAsync(IQueryCollection query)
        {
            // 1. Lấy chữ ký VNPAY gửi về
            var secureHash = query["vnp_SecureHash"].FirstOrDefault();
            if (string.IsNullOrEmpty(secureHash))
            {
                return new PaymentProviderResultDTO
                {
                    Success = false,
                    ErrorMessage = "Không tìm thấy chữ ký VNPAY."
                };
            }

            // 2. Lấy các tham số VNPAY trả về
            var parameters = new SortedDictionary<string, string>(StringComparer.Ordinal);
            foreach (var item in query)
            {
                if (!item.Key.StartsWith("vnp_"))
                    continue;

                if (item.Key == "vnp_SecureHash" ||
                    item.Key == "vnp_SecureHashType")
                    continue;

                var value = item.Value.FirstOrDefault();

                if (!string.IsNullOrEmpty(value))
                {
                    parameters[item.Key] = value;
                }
            }

            // 3. Tạo hashData giống lúc tạo payment
            var hashData = BuildQueryString(parameters);

            // 4. Tự tính chữ ký
            var calculatedHash = HmacSHA512(_options.HashSecret, hashData);

            // 5. Kiểm tra chữ ký
            if (!string.Equals(
                    calculatedHash,
                    secureHash,
                    StringComparison.OrdinalIgnoreCase))
            {
                return new PaymentProviderResultDTO
                {
                    Success = false,
                    ErrorMessage = "Chữ ký VNPAY không hợp lệ."
                };
            }

            // 6. Lấy thông tin giao dịch
            var paymentCode = parameters.GetValueOrDefault("vnp_TxnRef");

            var responseCode = parameters.GetValueOrDefault("vnp_ResponseCode");

            var transactionCode = parameters.GetValueOrDefault("vnp_TransactionNo");

            if (string.IsNullOrEmpty(paymentCode))
            {
                return new PaymentProviderResultDTO
                {
                    Success = true,
                    ErrorMessage = "Không tìm thấy mã thanh toán."
                };
            }

            // 7. Tìm Payment
            var payment = await _context.Payments
                .FirstOrDefaultAsync(x =>
                    x.PaymentCode == paymentCode);

            if (payment == null)
            {
                return new PaymentProviderResultDTO
                {
                    Success = true,
                    // PaymentCode = paymentCode,
                    ErrorMessage = "Không tìm thấy Payment."
                };
            }

            // 8. Xử lý kết quả thanh toán
            if (responseCode == "00")
            {
                payment.Status = PaymentStatus.Succeeded.ToString();
                // payment.ProviderTransactionId = transactionCode;
                payment.PaidAt = DateTime.UtcNow;
            }
            else
            {
                payment.Status = PaymentStatus.Failed.ToString();
                // payment.ProviderTransactionId = transactionCode;
            }

            // 9. Lưu DB
            await _context.SaveChangesAsync();

            return new PaymentProviderResultDTO
            {
                Success = responseCode == "00",
                ProviderTransactionId = transactionCode,
                ErrorCode = responseCode,
                ErrorMessage = responseCode == "00"
                    ? "Thanh toán thành công."
                    : "Thanh toán thất bại.",
                PaymentDate = parameters.GetValueOrDefault("vnp_PayDate"),
                orderInfo = parameters.GetValueOrDefault("vnp_OrderInfo"),
                txnRef = parameters.GetValueOrDefault("vnp_TxnRef"),
                transactionNo = parameters.GetValueOrDefault("vnp_TransactionNo")
            };
        }

        public async Task<PaymentProviderResultDTO> ProcessIPNAsync(IQueryCollection query)
        {
            if (query == null || query.Count == 0)
            {
                _logger.LogWarning("IPN Payment: Input data is required.");
                return new PaymentProviderResultDTO
                {
                    Success = false,
                    ErrorCode = "99",
                    ErrorMessage = "Input data required"
                };
            }

            var hashSecret = _options.HashSecret;

            var vnpay = new Dictionary<string, string>();

            foreach (var item in query)
            {
                var key = item.Key;

                if (!string.IsNullOrEmpty(key) &&
                    key.StartsWith("vnp_", StringComparison.Ordinal))
                {
                    vnpay[key] = item.Value.ToString();
                }
            }

            var txnRef = vnpay.GetValueOrDefault("vnp_TxnRef");
            var amountString = vnpay.GetValueOrDefault("vnp_Amount");
            var transactionNoString = vnpay.GetValueOrDefault("vnp_TransactionNo");
            var responseCode = vnpay.GetValueOrDefault("vnp_ResponseCode");
            var transactionStatus = vnpay.GetValueOrDefault("vnp_TransactionStatus");
            var secureHash = vnpay.GetValueOrDefault("vnp_SecureHash");

            // if (!long.TryParse(txnRef, out var orderId))
            // {
            //     return new PaymentProviderResultDTO
            //     {
            //         Success = false,
            //         ErrorCode = "01",
            //         ErrorMessage = "Order not found"
            //     };
            // }

            if (!long.TryParse(amountString, out var vnpAmount))
            {
                _logger.LogWarning(
                    "IPN Payment {PaymentCode}: Invalid amount: {Amount}",
                    txnRef,
                    amountString);
                return new PaymentProviderResultDTO
                {
                    Success = false,
                    ErrorCode = "04",
                    ErrorMessage = "Invalid amount"
                };
            }

            var amount = vnpAmount / 100;

            long.TryParse(transactionNoString, out var vnpayTransactionId);

            var parameters = new SortedDictionary<string, string>(StringComparer.Ordinal);
            foreach (var item in query)
            {
                if (!item.Key.StartsWith("vnp_"))
                    continue;

                if (item.Key == "vnp_SecureHash" ||
                    item.Key == "vnp_SecureHashType")
                    continue;

                var value = item.Value.FirstOrDefault();

                if (!string.IsNullOrEmpty(value))
                {
                    parameters[item.Key] = value;
                }
            }

            // 3. Tạo hashData giống lúc tạo payment
            var hashData = BuildQueryString(parameters);

            // 4. Tự tính chữ ký
            var calculatedHash = HmacSHA512(_options.HashSecret, hashData);

            // 5. Kiểm tra chữ ký
            if (!string.Equals(
                    calculatedHash,
                    secureHash,
                    StringComparison.OrdinalIgnoreCase))
            {
                _logger.LogWarning(
                    "IPN Payment {PaymentCode}: Invalid signature. Calculated: {CalculatedHash}, Received: {ReceivedHash}",
                    txnRef,
                    calculatedHash,
                    secureHash);
                return new PaymentProviderResultDTO
                {
                    Success = false,
                    ErrorCode = "97",
                    ErrorMessage = "Invalid signature"
                };
            }

            var attempt = await _context.PaymentAttempts
                .Include(x => x.PaymentCodeNavigation)
                .ThenInclude(x => x.Invoice)
                .FirstOrDefaultAsync(x =>
                    x.PaymentCode == txnRef);

            if (attempt == null)
            {
                _logger.LogWarning(
                    "IPN Payment {PaymentCode} attempt not found.",
                    txnRef);
                return new PaymentProviderResultDTO
                {
                    Success = false,
                    ErrorCode = "01",
                    ErrorMessage = "Order not found"
                };
            }

            // ============================
            // Kiểm tra amount
            // ============================

            if (attempt.Amount != amount)
            {
                _logger.LogWarning(
                    "IPN Payment {PaymentCode}: Amount mismatch. Expected: {ExpectedAmount}, Received: {ReceivedAmount}",
                    txnRef,
                    attempt.Amount,
                    amount);
                return new PaymentProviderResultDTO
                {
                    Success = false,
                    ErrorCode = "04",
                    ErrorMessage = "Invalid amount"
                };
            }

            // ============================
            // Idempotency
            // ============================

            if (attempt.Status == PaymentAttemptStatus.Success.ToString())
            {
                _logger.LogWarning(
                    "IPN Payment {PaymentCode}: Transaction already succeeded.",
                    txnRef);
                return new PaymentProviderResultDTO
                {
                    Success = true,
                    ErrorCode = "00",
                    ErrorMessage = "Confirm Success"
                };
            }

            // ============================
            // Thanh toán thành công
            // ============================

            if (responseCode == "00" && transactionStatus == "00")
            {
                attempt.Status = PaymentTransactionStatus.Succeeded.ToString();

                attempt.ProviderTransactionId = vnpayTransactionId.ToString();

                attempt.PaymentCodeNavigation.Status = PaymentStatus.Succeeded.ToString();

                attempt.PaymentCodeNavigation.Invoice.Status = InvoiceStatus.Paid.ToString();

                var transaction = new PaymentTransaction
                {
                    PaymentAttemptId = attempt.PaymentAttemptId,
                    TransactionType = (byte)PaymentTransactionType.Capture,
                    Amount = attempt.Amount,
                    TransactionAt = DateTime.UtcNow,
                    ProviderTransactionId = vnpayTransactionId.ToString(),
                    Status = PaymentTransactionStatus.Succeeded.ToString()
                };

                _context.PaymentTransactions.Add(transaction);

                await _context.SaveChangesAsync();

                _logger.LogWarning(
                    "IPN Payment {PaymentCode} succeeded. Transaction ID: {TransactionId}",
                    attempt.PaymentCode,
                    attempt.ProviderTransactionId);

                return new PaymentProviderResultDTO
                {
                    Success = true,
                    ErrorCode = "00",
                    ErrorMessage = "Confirm Success",
                    PaymentDate = vnpay.GetValueOrDefault("vnp_PayDate"),
                    orderInfo = vnpay.GetValueOrDefault("vnp_OrderInfo"),
                    txnRef = vnpay.GetValueOrDefault("vnp_TxnRef"),
                    transactionNo = vnpay.GetValueOrDefault("vnp_TransactionNo")

                };
            }

            // ============================
            // Thanh toán thất bại
            // ============================

            attempt.Status = PaymentAttemptStatus.Failed.ToString();

            attempt.ProviderTransactionId = vnpayTransactionId.ToString();

            // attempt.FailureCode = responseCode;

            await _context.SaveChangesAsync();

            return new PaymentProviderResultDTO
            {
                Success = true,
                ErrorCode = "00",
                ErrorMessage = "Confirm Success"
            };
        }

        public bool VerifyWebhook(string payload, string signature)
        {
            // Verify signature theo format webhook
            // thực tế của provider.
            return true;
        }

        public async Task<RefundResultDTO> RefundAsync(RefundRequestDTO request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (string.IsNullOrWhiteSpace(request.TxnRef))
                throw new ArgumentException("TxnRef is required.", nameof(request));

            if (request.Amount <= 0)
                throw new ArgumentException("Refund amount must be greater than 0.", nameof(request));

            if (request.TransactionType != "02" &&
                request.TransactionType != "03")
            {
                throw new ArgumentException(
                    "TransactionType must be 02 or 03.",
                    nameof(request.TransactionType));
            }

            var vnpApi = _options.RefundUrl;
            var hashSecret = _options.HashSecret;
            var tmnCode = _options.TmnCode;

            // --------------------------------------------------
            // 1. Generate request information
            // --------------------------------------------------

            var requestId = Guid.NewGuid().ToString();
            var version = "2.1.0";
            var command = "refund";
            var amount = checked(request.Amount * 100);
            var txnRef = request.TxnRef;
            var transactionType = "02";//request.TransactionType;
            var transactionNo = request.TransactionNo ?? string.Empty;
            var transactionDate = request.TransactionDate;
            var createBy = request.CreateBy;
            var createDate = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
            var ipAddr = "127.0.0.1";
            var orderInfo = request.OrderInfo ?? $"Hoan tien giao dich:{txnRef}";

            var signData = string.Join("|",
                requestId,
                version,
                command,
                tmnCode,
                transactionType,
                txnRef,
                amount,
                transactionNo,
                transactionDate,
                createBy,
                createDate,
                ipAddr,
                orderInfo
            );

            var secureHash = HmacSHA512(
                hashSecret,
                signData
            );

            var requestBody = new
            {
                vnp_RequestId = requestId,
                vnp_Version = version,
                vnp_Command = command,
                vnp_TmnCode = tmnCode,
                vnp_TransactionType = transactionType,
                vnp_TxnRef = txnRef,
                vnp_Amount = amount,
                vnp_OrderInfo = orderInfo,
                vnp_TransactionNo = transactionNo,
                vnp_TransactionDate = transactionDate,
                vnp_CreateBy = createBy,
                vnp_CreateDate = createDate,
                vnp_IpAddr = ipAddr,
                vnp_SecureHash = secureHash
            };

            var json = JsonSerializer.Serialize(requestBody);

            using var requestg = new HttpRequestMessage(
                HttpMethod.Post,
                "https://sandbox.vnpayment.vn/merchant_webapi/api/transaction"
            );

            requestg.Content = new StringContent(
                json,
                Encoding.UTF8
            );

            requestg.Headers.UserAgent.ParseAdd(
                "ManagerStudentSystem/1.0"
            );

            requestg.Headers.Accept.ParseAdd("*/*");

            requestg.Content.Headers.ContentType =
                new MediaTypeHeaderValue("application/json");

            using var response = await HttpClient.SendAsync(requestg);

            var responseBody =
                await response.Content.ReadAsStringAsync();

            // --------------------------------------------------
            // 7. HTTP error
            // --------------------------------------------------

            if (!response.IsSuccessStatusCode)
            {
                return new RefundResultDTO
                {
                    Success = false,
                    IsValidSignature = false,
                    Message = $"VNPAY HTTP error: {(int)response.StatusCode}",
                    RawResponse = responseBody
                };
            }


            // --------------------------------------------------
            // 8. Deserialize VNPAY response
            // --------------------------------------------------

            var result =
                JsonSerializer.Deserialize<VnpayRefundResponseDTO>(
                    responseBody,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });


            if (result == null)
            {
                return new RefundResultDTO
                {
                    Success = false,
                    IsValidSignature = false,
                    Message = "Invalid response from VNPAY.",
                    RawResponse = responseBody
                };
            }


            // --------------------------------------------------
            // 9. Verify VNPAY response signature
            // --------------------------------------------------

            var responseSignData = string.Join("|",
                result.vnp_ResponseId,
                result.vnp_Command,
                result.vnp_ResponseCode,
                result.vnp_Message,
                result.vnp_TmnCode,
                result.vnp_TxnRef,
                result.vnp_Amount,
                result.vnp_BankCode,
                result.vnp_PayDate,
                result.vnp_TransactionNo,
                result.vnp_TransactionType,
                result.vnp_TransactionStatus,
                result.vnp_OrderInfo
            );

            var expectedHash = HmacSHA512(hashSecret, responseSignData);

            var isValidSignature =
                string.Equals(
                    expectedHash,
                    result.vnp_SecureHash,
                    StringComparison.OrdinalIgnoreCase
                );


            // --------------------------------------------------
            // 10. Return result
            // --------------------------------------------------

            return new RefundResultDTO
            {
                Success =
                    isValidSignature &&
                    result.vnp_ResponseCode == "00",

                IsValidSignature = isValidSignature,

                ResponseCode =
                    result.vnp_ResponseCode,

                Message =
                    result.vnp_Message,

                ResponseId =
                    result.vnp_ResponseId,

                TransactionNo =
                    result.vnp_TransactionNo,

                TransactionStatus =
                    result.vnp_TransactionStatus,

                RawResponse =
                    responseBody
            };
        }

        public async Task<WebhookResultDTO> ParseWebhookAsync(IQueryCollection payload)
        {
            // 1. Lấy các parameter vnp_*
            var data = new SortedDictionary<string, string>(StringComparer.Ordinal);

            foreach (var item in payload)
            {
                if (!item.Key.StartsWith("vnp_", StringComparison.Ordinal))
                    continue;

                if (item.Key.Equals(
                    "vnp_SecureHash",
                    StringComparison.OrdinalIgnoreCase))
                    continue;

                data[item.Key] = item.Value.ToString();
            }

            // 2. Lấy secure hash
            var secureHash = payload["vnp_SecureHash"].ToString();

            if (string.IsNullOrWhiteSpace(secureHash))
                throw new InvalidOperationException("Missing VNPAY secure hash.");

            // 3. Verify chữ ký
            var isValid = VerifySignature(data, secureHash);

            if (!isValid)
                throw new InvalidOperationException("Invalid VNPAY signature.");

            // 4. Lấy dữ liệu nghiệp vụ
            var txnRef = payload["vnp_TxnRef"].ToString();
            var amountRaw = payload["vnp_Amount"].ToString();
            var transactionNo = payload["vnp_TransactionNo"].ToString();
            var responseCode = payload["vnp_ResponseCode"].ToString();
            var transactionStatus = payload["vnp_TransactionStatus"].ToString();

            // 5. Validate field
            if (string.IsNullOrWhiteSpace(txnRef))
                throw new InvalidOperationException("Missing vnp_TxnRef.");

            if (!long.TryParse(amountRaw, out var amount))
            {
                throw new InvalidOperationException("Invalid vnp_Amount.");
            }

            // VNPAY amount = actual amount * 100
            var actualAmount = amount / 100m;

            // 6. Trả về DTO chuẩn hóa
            return new WebhookResultDTO
            {
                PaymentCode = txnRef,
                Amount = actualAmount,
                ProviderTransactionId = transactionNo,
                ResponseCode = responseCode,
                TransactionStatus = transactionStatus,
                Success = responseCode == "00" && transactionStatus == "00"
            };
        }

        public async Task<PaymentProviderResultDTO> QueryPaymentAsync(PaymentQueryRequestDTO request)
        {
            var vnp_Api = _options.RefundUrl; // API URL
            var vnp_HashSecret = _options.HashSecret; //Secret KEy
            var vnp_TmnCode = _options.TmnCode; // Terminal Id

            var vnp_RequestId = DateTime.Now.Ticks.ToString(); //Mã hệ thống merchant tự sinh ứng với mỗi yêu cầu truy vấn giao dịch. Mã này là duy nhất dùng để phân biệt các yêu cầu truy vấn giao dịch. Không được trùng lặp trong ngày.
            var vnp_Version = "2.1.0";
            var vnp_Command = "querydr";
            var vnp_TxnRef = request.ProviderTransactionId; // Mã giao dịch thanh toán tham chiếu
            var vnp_OrderInfo = "Truy van giao dich:" + request.ProviderTransactionId;
            var vnp_TransactionDate = request.TransactionDate;
            var vnp_CreateDate = DateTime.Now.ToString("yyyyMMddHHmmss");
            var vnp_IpAddr = "127.0.0.1";

            var signData = vnp_RequestId + "|" + vnp_Version + "|" + vnp_Command + "|" + vnp_TmnCode + "|" + vnp_TxnRef + "|" + vnp_TransactionDate + "|" + vnp_CreateDate + "|" + vnp_IpAddr + "|" + vnp_OrderInfo;
            var vnp_SecureHash = HmacSHA512(vnp_HashSecret, signData);

            var qdrData = new
            {
                vnp_RequestId = vnp_RequestId,
                vnp_Version = vnp_Version,
                vnp_Command = vnp_Command,
                vnp_TmnCode = vnp_TmnCode,
                vnp_TxnRef = vnp_TxnRef,
                vnp_OrderInfo = vnp_OrderInfo,
                vnp_TransactionDate = vnp_TransactionDate,
                vnp_CreateDate = vnp_CreateDate,
                vnp_IpAddr = vnp_IpAddr,
                vnp_SecureHash = vnp_SecureHash

            };

            var json = JsonSerializer.Serialize(qdrData);

            using var requestg = new HttpRequestMessage(
                HttpMethod.Post,
                "https://sandbox.vnpayment.vn/merchant_webapi/api/transaction"
            );

            requestg.Content = new StringContent(
                json,
                Encoding.UTF8
            );

            requestg.Headers.UserAgent.ParseAdd(
                "ManagerStudentSystem/1.0"
            );

            requestg.Headers.Accept.ParseAdd("*/*");

            requestg.Content.Headers.ContentType =
                new MediaTypeHeaderValue("application/json");

            using var response = await HttpClient.SendAsync(requestg);

            var responseBody =
                await response.Content.ReadAsStringAsync();

            var res = JsonSerializer.Deserialize<PaymentProviderResultDTO>(
                    responseBody,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

            return res;


        }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ManagerStudentSystem.Dtos;
using ManagerStudentSystem.Dtos.Refund;
using ManagerStudentSystem.Enums;
using ManagerStudentSystem.Models;
using ManagerStudentSystem.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ManagerStudentSystem.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly TaekwondoDbContext _context;
        private readonly IPaymentAttemptService _attemptService;
        private readonly IEnumerable<IPaymentProvider> _providers;

        public PaymentService(
            TaekwondoDbContext context,
            IPaymentAttemptService attemptService,
            IEnumerable<IPaymentProvider> providers)
        {
            _context = context;
            _attemptService = attemptService;
            _providers = providers;
        }

        private static string GeneratePaymentCode()
        {
            return $"PAY-{DateTime.UtcNow:yyyyMMddHHmmssfff}";
        }

        public async Task<PaymentResponseDTO> CreatePaymentAsync(PaymentRequestDTO request, string idempotencyKey)
        {
            var invoice = await _context.Invoices.FirstOrDefaultAsync(x => x.InvoiceCode == request.InvoiceCode);

            if (invoice == null)
                throw new KeyNotFoundException("Invoice not found.");

            if (invoice.Status == InvoiceStatus.Cancelled.ToString())
                throw new InvalidOperationException("Invoice has been cancelled.");

            if (invoice.Status == InvoiceStatus.Paid.ToString())
                throw new InvalidOperationException("Invoice has already been paid.");

            // 3. Calculate remaining amount
            var remainingAmount = invoice.TotalAmount - invoice.PaidAmount;

            if (remainingAmount <= 0)
                throw new InvalidOperationException("Invoice has no remaining amount.");

            if (request.Amount <= 0)
                throw new InvalidOperationException("Payment amount must be greater than zero.");

            if (request.Amount > remainingAmount)
                throw new InvalidOperationException("Payment amount exceeds invoice remaining amount.");

            var provider = await _context.PaymentProviders
                .FirstOrDefaultAsync(x =>
                    x.PaymentProviderId == request.ProviderId && x.IsActive);

            if (provider == null)
                throw new KeyNotFoundException("Payment provider not found.");

            var payment = new Payment
            {
                InvoiceId = invoice.InvoiceId,
                ProviderId = provider.PaymentProviderId,
                PaymentMethodId = request.PaymentMethodId,
                PaymentCode = GeneratePaymentCode(),
                Amount = request.Amount,
                Currency = request.Currency,
                Status = PaymentStatus.Pending.ToString(),
            };

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            var attempt = await _attemptService.CreateAsync(payment, 1);

            var paymentProvider = _providers
                .FirstOrDefault(x =>
                    string.Equals(
                        x.Code.ToString(),
                        provider.PaymentProviderCode.ToString(),
                        StringComparison.OrdinalIgnoreCase));

            if (paymentProvider == null)
                throw new InvalidOperationException(
                    $"Provider '{provider.PaymentProviderCode}' is not implemented.");

            payment.Status = PaymentStatus.Processing.ToString();
            await _context.SaveChangesAsync();

            await _attemptService.MarkProcessingAsync(attempt.PaymentAttemptId);

            var paymentRequest = new PaymentProviderRequestDTO
            {
                PaymentCode = payment.PaymentCode,
                Amount = payment.Amount,
                Currency = payment.Currency,
                ReturnUrl = "http://localhost:5260/api/payment/vnpay-return",
                CallbackUrl = "https://your-api.com/api/webhooks/payment"
            };

            var result = await paymentProvider.CreatePaymentAsync(paymentRequest);

            if (!result.Success)
            {
                await _attemptService.MarkFailedAsync(
                    attempt.PaymentAttemptId,
                    result.ErrorCode,
                    result.ErrorMessage);

                payment.Status = PaymentStatus.Failed.ToString();
                await _context.SaveChangesAsync();
                throw new InvalidOperationException(result.ErrorMessage ?? "Payment provider failed.");
            }

            attempt.ProviderTransactionId = result.ProviderTransactionId;
            attempt.PaymentUrl = result.PaymentUrl;
            await _context.SaveChangesAsync();

            return new PaymentResponseDTO
            {
                Id = payment.PaymentId,
                PaymentCode = payment.PaymentCode,
                InvoiceId = payment.InvoiceId,
                Amount = payment.Amount,
                Currency = payment.Currency,
                Status = payment.Status,
                AttemptId = attempt.PaymentAttemptId,
                PaymentUrl = attempt.PaymentUrl
            };
        }

        public async Task<PaymentProviderResultDTO> ReturnResult(IQueryCollection query)
        {
            // Get provider implementation
            var paymentProvider = _providers
                .FirstOrDefault(x =>
                    string.Equals(
                        x.Code.ToString(),
                        "VNPAY",
                        StringComparison.OrdinalIgnoreCase));

            if (paymentProvider == null)
                throw new InvalidOperationException(
                    $"Provider VNPAY is not implemented.");

            var res = await paymentProvider.ProcessReturnAsync(query);

            return res;
        }

        public async Task<PaymentProviderResultDTO> IpnResult(IQueryCollection query)
        {
            // Get provider implementation
            var paymentProvider = _providers
                .FirstOrDefault(x =>
                    string.Equals(
                        x.Code.ToString(),
                        "VNPAY",
                        StringComparison.OrdinalIgnoreCase));

            if (paymentProvider == null)
                throw new InvalidOperationException(
                    $"Provider VNPAY is not implemented.");

            var res = await paymentProvider.ProcessIPNAsync(query);

            return res;
        }

        public async Task<RefundResultDTO> RefundAsync(RefundRequestDTO request)
        {
            // Get provider implementation
            var paymentProvider = _providers
                .FirstOrDefault(x =>
                    string.Equals(
                        x.Code.ToString(),
                        "VNPAY",
                        StringComparison.OrdinalIgnoreCase));

            if (paymentProvider == null)
                throw new InvalidOperationException(
                    $"Provider VNPAY is not implemented.");

            var res = await paymentProvider.RefundAsync(request);

            return res;
        }

        public async Task<PaymentResponseDTO> GetByIdAsync(long paymentId)
        {
            var payment = await _context.Payments
                .Include(x => x.Invoice)
                .Include(x => x.Provider)
                .Include(x => x.PaymentAttempts)
                .FirstOrDefaultAsync(x => x.PaymentId == paymentId);

            if (payment == null)
                return null;

            var latestAttempt = payment.PaymentAttempts
                .OrderByDescending(x => x.AttemptNumber)
                .FirstOrDefault();

            return new PaymentResponseDTO
            {
                Id = payment.PaymentId,
                PaymentCode = payment.PaymentCode,
                InvoiceId = payment.InvoiceId,
                Amount = payment.Amount,
                Currency = payment.Currency,
                Status = payment.Status,
                AttemptId = latestAttempt?.PaymentAttemptId ?? 0,
                PaymentUrl = latestAttempt?.PaymentUrl
            };
        }

        public Task<PaymentResponseDTO> GetByCodeAsync(string paymentCode)
        {
            throw new NotImplementedException();
        }

        public async Task<PaymentResponseDTO> RetryAsync(string paymentCode)
        {
            var payment = await _context.Payments
                .Include(x => x.Invoice)
                .Include(x => x.Provider)
                .FirstOrDefaultAsync(x => x.PaymentCode == paymentCode);

            if (payment == null)
                throw new KeyNotFoundException("Payment not found.");

            if (payment.Status == PaymentStatus.Failed.ToString() && payment.Status != PaymentStatus.Expired.ToString())
            {
                throw new InvalidOperationException("Payment cannot be retried.");
            }

            var attemptNumber = await _attemptService.GetNextAttemptNumberAsync(payment.PaymentCode);

            var attempt = await _attemptService.CreateAsync(payment, attemptNumber);

            var provider = _providers
                .FirstOrDefault(x =>
                    string.Equals(
                        x.Code,
                        payment.Provider.PaymentProviderCode.ToString(),
                        StringComparison.OrdinalIgnoreCase));

            if (provider == null)
                throw new InvalidOperationException("Payment provider not implemented.");

            payment.Status = PaymentStatus.Processing.ToString();

            await _context.SaveChangesAsync();

            await _attemptService.MarkProcessingAsync(attempt.PaymentAttemptId);

            var result =
                await provider.CreatePaymentAsync(
                    new PaymentProviderRequestDTO
                    {
                        PaymentCode = payment.PaymentCode,
                        Amount = payment.Amount,
                        Currency = payment.Currency,
                        ReturnUrl = "https://your-app.com/payment/return",
                        CallbackUrl = "https://your-api.com/api/webhooks/payment"
                    });

            if (!result.Success)
            {
                await _attemptService.MarkFailedAsync(
                    attempt.PaymentAttemptId,
                    result.ErrorCode,
                    result.ErrorMessage);

                payment.Status = PaymentStatus.Failed.ToString();

                await _context.SaveChangesAsync();

                throw new InvalidOperationException(result.ErrorMessage ?? "Retry payment failed.");
            }

            attempt.ProviderTransactionId = result.ProviderTransactionId;

            attempt.PaymentUrl = result.PaymentUrl;

            await _context.SaveChangesAsync();

            return new PaymentResponseDTO
            {
                Id = payment.PaymentId,
                PaymentCode = payment.PaymentCode,
                InvoiceId = payment.InvoiceId,
                Amount = payment.Amount,
                Currency = payment.Currency,
                Status = payment.Status,
                AttemptId = attempt.PaymentAttemptId,
                PaymentUrl = attempt.PaymentUrl
            };
        }

        public async Task CancelAsync(string paymentCode)
        {
            var payment = await _context.Payments
                .FirstOrDefaultAsync(x => x.PaymentCode == paymentCode);

            if (payment == null)
                throw new KeyNotFoundException(
                    "Payment not found.");

            var paymentStatus = payment.Status;

            if (paymentStatus == PaymentStatus.Succeeded.ToString())
                throw new InvalidOperationException("Succeeded payment cannot be cancelled.");

            if (paymentStatus == PaymentStatus.Cancelled.ToString())
                return;

            payment.Status = PaymentStatus.Cancelled.ToString();
            payment.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }
    }
}
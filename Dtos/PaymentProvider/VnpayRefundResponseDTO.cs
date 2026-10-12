using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManagerStudentSystem.Dtos.Refund
{
    public class VnpayRefundResponseDTO
    {
        public string vnp_ResponseId { get; set; } = default!;
        public string vnp_Command { get; set; } = default!;
        public string vnp_ResponseCode { get; set; } = default!;
        public string vnp_Message { get; set; } = default!;
        public string vnp_TmnCode { get; set; } = default!;
        public string vnp_TxnRef { get; set; } = default!;
        public string vnp_Amount { get; set; } = default!;
        public string vnp_BankCode { get; set; } = default!;
        public string vnp_PayDate { get; set; } = default!;
        public string vnp_TransactionNo { get; set; } = default!;
        public string vnp_TransactionType { get; set; } = default!;
        public string vnp_TransactionStatus { get; set; } = default!;
        public string vnp_OrderInfo { get; set; } = default!;
        public string vnp_SecureHash { get; set; } = default!;
    }
}
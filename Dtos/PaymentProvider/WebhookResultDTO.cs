using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManagerStudentSystem.Dtos
{
    public class WebhookResultDTO
    {
        public bool IsValid { get; set; }
        public string EventId { get; set; } = null!;
        public string? EventType { get; set; }
        public string ProviderTransactionId { get; set; } = null!;
        public string PaymentCode { get; set; } = null!;
        public decimal Amount { get; set; }
        public bool Success { get; set; }
        public string? ErrorCode { get; set; }
        public string? ErrorMessage { get; set; }
        public string ResponseCode { get; set; }
        public string TransactionStatus { get; set; }
    }
}
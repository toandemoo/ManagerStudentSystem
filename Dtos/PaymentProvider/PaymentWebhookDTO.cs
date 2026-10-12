using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManagerStudentSystem.Dtos.PaymentProvider
{
    public class PaymentWebhookDTO
    {
        public string PaymentCode { get; set; }
        public decimal Amount { get; set; }
        public string ProviderTransactionId { get; set; }
        public string ResponseCode { get; set; }
        public string TransactionStatus { get; set; }
        public bool IsSuccess { get; set; }
    }
}
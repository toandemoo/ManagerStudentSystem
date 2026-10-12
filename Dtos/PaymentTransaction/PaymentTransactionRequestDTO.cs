using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ManagerStudentSystem.Enums;

namespace ManagerStudentSystem.Dtos
{
    public class PaymentTransactionRequestDTO
    {
        public int PaymentId { get; set; }
        public int PaymentAttemptId { get; set; }
        public int ProviderId { get; set; }
        public string ProviderTransactionId { get; set; } = null!;
        public PaymentTransactionType Type { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; } = null!;
    }
}
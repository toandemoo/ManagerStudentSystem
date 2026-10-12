using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ManagerStudentSystem.Enums;

namespace ManagerStudentSystem.Dtos
{
    public class PaymentTransactionDTO
    {
        public long Id { get; set; }

        public long PaymentId { get; set; }

        public long? PaymentAttemptId { get; set; }

        public int ProviderId { get; set; }

        public string ProviderTransactionId { get; set; } = null!;

        public PaymentTransactionType TransactionType { get; set; }

        public decimal Amount { get; set; }

        public PaymentTransactionStatus Status { get; set; }

        public DateTime? TransactionAt { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
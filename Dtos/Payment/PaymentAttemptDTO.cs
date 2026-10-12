using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ManagerStudentSystem.Enums;

namespace ManagerStudentSystem.Dtos
{
    public class PaymentAttemptDTO
    {
        public long Id { get; set; }

        public long PaymentId { get; set; }

        public int AttemptNumber { get; set; }

        public string? ProviderTransactionId { get; set; }

        public decimal Amount { get; set; }

        public PaymentAttemptStatus Status { get; set; }

        public string? PaymentUrl { get; set; }

        public string? ErrorCode { get; set; }

        public string? ErrorMessage { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? StartedAt { get; set; }

        public DateTime? CompletedAt { get; set; }
    }
}
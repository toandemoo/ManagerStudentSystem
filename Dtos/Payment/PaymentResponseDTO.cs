using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ManagerStudentSystem.Enums;

namespace ManagerStudentSystem.Dtos
{
    public class PaymentResponseDTO
    {
        public long Id { get; set; }
        public long InvoiceId { get; set; }
        public long AttemptId { get; set; }
        public string PaymentCode { get; set; } = null!;
        public decimal Amount { get; set; }
        public string Currency { get; set; } = null!;
        public string Status { get; set; } = null!;
        public string? PaymentUrl { get; set; }
    }
}
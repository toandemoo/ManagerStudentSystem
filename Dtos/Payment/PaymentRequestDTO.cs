using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManagerStudentSystem.Dtos
{
    public class PaymentRequestDTO
    {
        public string? InvoiceCode { get; set; }
        public int ProviderId { get; set; }
        public int PaymentMethodId { get; set; }
        public string Currency { get; set; } = "VND";
        public decimal Amount { get; set; }
    }
}
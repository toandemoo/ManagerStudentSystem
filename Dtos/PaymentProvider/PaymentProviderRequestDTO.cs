using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManagerStudentSystem.Dtos
{
    public class PaymentProviderRequestDTO
    {
        public string PaymentCode { get; set; } = null!;
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "VND";
        public string ReturnUrl { get; set; } = null!;
        public string CallbackUrl { get; set; } = null!;
    }
}
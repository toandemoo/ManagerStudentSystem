using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManagerStudentSystem.Dtos
{
    public class VnPayOptions
    {
        public string TmnCode { get; set; } = null!;
        public string HashSecret { get; set; } = null!;
        public string BaseUrl { get; set; } = null!;
        public string PaymentUrl { get; set; } = null!;
        public string ReturnUrl { get; set; } = null!;
        public string CallbackUrl { get; set; } = null!;
        public string RefundUrl { get; set; } = null!;
    }
}
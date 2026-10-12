using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManagerStudentSystem.Dtos
{
    public class PaymentProviderResultDTO
    {
        public bool Success { get; set; }
        public string? ProviderTransactionId { get; set; }
        public string? PaymentUrl { get; set; }
        public string? ErrorCode { get; set; }
        public string? ErrorMessage { get; set; }
        public string? PaymentDate { get; set; }
        public string? orderInfo { get; set; }
        public string? txnRef { get; set; }
        public string? transactionNo { get; set; }
    }
}
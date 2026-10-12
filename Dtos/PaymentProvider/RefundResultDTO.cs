using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManagerStudentSystem.Dtos.Refund
{
    public class RefundResultDTO
    {
        public bool Success { get; set; }
        public bool IsValidSignature { get; set; }
        public string? ResponseCode { get; set; }
        public string? Message { get; set; }
        public string? ResponseId { get; set; }
        public string? TransactionNo { get; set; }
        public string? TransactionStatus { get; set; }
        public string? RawResponse { get; set; }
    }
}
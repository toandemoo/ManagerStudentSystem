using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManagerStudentSystem.Dtos.Refund
{
    public class RefundRequestDTO
    {
        public string TxnRef { get; set; } = default!;
        public long Amount { get; set; }
        public string TransactionType { get; set; } = "02";
        public string CreateBy { get; set; } = default!;
        public string TransactionDate { get; set; } = default!;
        public string? TransactionNo { get; set; }
        public string? OrderInfo { get; set; }
    }
}
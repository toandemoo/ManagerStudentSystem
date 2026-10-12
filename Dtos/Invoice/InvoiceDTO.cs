using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManagerStudentSystem.Dtos
{
    public class InvoiceDTO
    {
        public int InvoiceId { get; set; }
        public int StudentId { get; set; }
        public string InvoiceCode { get; set; } = null!;
        public decimal TotalAmount { get; set; }
        public decimal? PaidAmount { get; set; }
        public decimal? RemainingAmount { get; set; }
        public string Status { get; set; } = null!;
    }
}
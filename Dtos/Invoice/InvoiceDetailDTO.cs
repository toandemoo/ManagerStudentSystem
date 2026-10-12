using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManagerStudentSystem.Dtos
{
    public class InvoiceDetailDTO
    {
        public int InvoiceDetailId { get; set; }

        public int InvoiceId { get; set; }

        public byte ItemTypeId { get; set; }

        public decimal UnitPrice { get; set; }

        public int Quantity { get; set; }

        public decimal TotalPrice { get; set; }

        public string Description { get; set; } = null!;
    }
}
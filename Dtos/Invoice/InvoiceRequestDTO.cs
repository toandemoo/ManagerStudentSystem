using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManagerStudentSystem.Dtos.Invoice
{
    public class InvoiceRequestDTO
    {
        public int StudentId { get; set; }
        public List<ItemDTO> Items { get; set; } = new List<ItemDTO>();
    }
}
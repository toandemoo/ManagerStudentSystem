using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ManagerStudentSystem.Dtos;
using ManagerStudentSystem.Dtos.Invoice;

namespace ManagerStudentSystem.Services.Interfaces
{
    public interface IInvoiceService
    {
        Task<List<InvoiceDTO>> GetAllInvoicesAsync();
        Task<InvoiceDTO> GetInvoiceByIdAsync(int invoiceId);
        Task<InvoiceDTO> CreateInvoiceAsync(InvoiceRequestDTO invoiceDto);
        Task<InvoiceDTO> UpdateInvoiceAsync(int invoiceId, InvoiceDTO invoiceDto);
        Task<bool> DeleteInvoiceAsync(int invoiceId);
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ManagerStudentSystem.Dtos;

namespace ManagerStudentSystem.Services.Interfaces
{
    public interface IInvoiceDetailService
    {
        Task<List<InvoiceDetailDTO>> GetInvoiceDetailsByInvoiceId(int invoiceId);
        Task<InvoiceDetailDTO> GetInvoiceDetailById(int invoiceDetailId);
        Task<bool> CreateInvoiceDetail(InvoiceDetailDTO invoiceDetailDto);
        Task<bool> UpdateInvoiceDetail(int invoiceDetailId, InvoiceDetailDTO invoiceDetailDto);
        Task<bool> DeleteInvoiceDetail(int invoiceDetailId);
    }
}
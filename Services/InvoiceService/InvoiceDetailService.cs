using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ManagerStudentSystem.Dtos;
using ManagerStudentSystem.Models;
using ManagerStudentSystem.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ManagerStudentSystem.Services
{
    public class InvoiceDetailService : IInvoiceDetailService
    {
        private readonly ILogger<InvoiceDetailService> _logger;
        private readonly TaekwondoDbContext _context;

        public InvoiceDetailService(ILogger<InvoiceDetailService> logger, TaekwondoDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<bool> CreateInvoiceDetail(InvoiceDetailDTO invoiceDetailDto)
        {
            try
            {
                var invoiceDetail = new InvoiceDetail
                {
                    InvoiceId = invoiceDetailDto.InvoiceId,
                    ItemTypeId = invoiceDetailDto.ItemTypeId,
                    Quantity = invoiceDetailDto.Quantity,
                    UnitPrice = invoiceDetailDto.UnitPrice,
                    TotalPrice = invoiceDetailDto.TotalPrice,
                };

                _context.InvoiceDetails.Add(invoiceDetail);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating invoice detail");
                return false;
            }
        }

        public async Task<bool> DeleteInvoiceDetail(int invoiceDetailId)
        {
            try
            {
                var invoiceDetail = await _context.InvoiceDetails.FindAsync(invoiceDetailId);
                if (invoiceDetail == null)
                {
                    return false;
                }

                _context.InvoiceDetails.Remove(invoiceDetail);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting invoice detail");
                return false;
            }
        }

        public async Task<InvoiceDetailDTO> GetInvoiceDetailById(int invoiceDetailId)
        {
            try
            {

                var invoiceDetail = await _context.InvoiceDetails.FindAsync(invoiceDetailId);
                if (invoiceDetail == null)
                {
                    return null;
                }

                return new InvoiceDetailDTO
                {
                    InvoiceDetailId = invoiceDetail.InvoiceDetailId,
                    InvoiceId = invoiceDetail.InvoiceId,
                    ItemTypeId = invoiceDetail.ItemTypeId,
                    Quantity = invoiceDetail.Quantity,
                    UnitPrice = invoiceDetail.UnitPrice,
                    TotalPrice = invoiceDetail.TotalPrice,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving invoice detail by ID");
                return null;
            }
        }

        public async Task<List<InvoiceDetailDTO>> GetInvoiceDetailsByInvoiceId(int invoiceId)
        {
            try
            {

                var invoiceDetails = await _context.InvoiceDetails
                    .Where(d => d.InvoiceId == invoiceId)
                    .ToListAsync();

                return invoiceDetails.Select(d => new InvoiceDetailDTO
                {
                    InvoiceDetailId = d.InvoiceDetailId,
                    InvoiceId = d.InvoiceId,
                    ItemTypeId = d.ItemTypeId,
                    Quantity = d.Quantity,
                    UnitPrice = d.UnitPrice,
                    TotalPrice = d.TotalPrice,
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving invoice details by invoice ID");
                return null;
            }
        }

        public async Task<bool> UpdateInvoiceDetail(int invoiceDetailId, InvoiceDetailDTO invoiceDetailDto)
        {
            try
            {
                var invoiceDetail = await _context.InvoiceDetails.FindAsync(invoiceDetailId);
                if (invoiceDetail == null)
                {
                    return false;
                }

                invoiceDetail.ItemTypeId = invoiceDetailDto.ItemTypeId;
                invoiceDetail.Quantity = invoiceDetailDto.Quantity;
                invoiceDetail.UnitPrice = invoiceDetailDto.UnitPrice;
                invoiceDetail.TotalPrice = invoiceDetailDto.TotalPrice;

                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating invoice detail");
                return false;
            }
        }

    }

}
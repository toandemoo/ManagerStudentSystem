using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ManagerStudentSystem.Dtos;
using ManagerStudentSystem.Dtos.Invoice;
using ManagerStudentSystem.Enums;
using ManagerStudentSystem.Models;
using ManagerStudentSystem.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ManagerStudentSystem.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly ILogger<InvoiceService> _logger;
        private readonly TaekwondoDbContext _context;
        public InvoiceService(ILogger<InvoiceService> logger, TaekwondoDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        private static string GenerateInvoiceCode()
        {
            return $"INVOICE-{DateTime.UtcNow:yyyyMMddHHmmssfff}";
        }

        public async Task<InvoiceDTO> CreateInvoiceAsync(InvoiceRequestDTO invoiceDto)
        {
            try
            {
                if (invoiceDto.Items == null || invoiceDto.Items.Count == 0)
                    throw new Exception("Invoice must contain at least one item.");

                var itemIds = invoiceDto.Items
                    .Select(x => x.ItemId)
                    .Distinct()
                    .ToList();

                var items = await _context.Items
                    .Where(x => itemIds.Contains(x.ItemId))
                    .ToListAsync();

                if (items.Count != itemIds.Count)
                    throw new Exception("One or more items do not exist.");

                var invoice = new Invoice
                {
                    StudentId = invoiceDto.StudentId,
                    InvoiceCode = GenerateInvoiceCode(),
                    Status = InvoiceStatus.Pending.ToString(),
                };

                foreach (var item in invoiceDto.Items)
                {
                    var invoiceItem = items
                        .First(x => x.ItemId == item.ItemId);

                    if (item.Quantity <= 0)
                        throw new Exception("Quantity must be greater than zero.");

                    var detail = new InvoiceDetail
                    {
                        ItemTypeId = invoiceItem.ItemTypeId,
                        Quantity = item.Quantity,
                        UnitPrice = invoiceItem.Price,
                        TotalPrice = invoiceItem.Price * item.Quantity
                    };

                    invoice.InvoiceDetails.Add(detail);
                }

                invoice.TotalAmount = invoice.InvoiceDetails
                    .Sum(x => x.TotalPrice);

                _context.Invoices.Add(invoice);

                await _context.SaveChangesAsync();

                return new InvoiceDTO
                {
                    InvoiceId = invoice.InvoiceId,
                    StudentId = invoice.StudentId,
                    InvoiceCode = invoice.InvoiceCode,
                    TotalAmount = invoice.TotalAmount,
                    PaidAmount = invoice.PaidAmount,
                    RemainingAmount = invoice.TotalAmount - (invoice.PaidAmount ?? 0),
                    Status = invoice.Status
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating invoice");
                return null;
            }
        }

        public async Task<bool> DeleteInvoiceAsync(int invoiceId)
        {
            try
            {
                var invoice = await _context.Invoices.FindAsync(invoiceId);
                if (invoice == null)
                    return false;

                _context.Invoices.Remove(invoice);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting invoice");
                return false;
            }
        }

        public async Task<List<InvoiceDTO>> GetAllInvoicesAsync()
        {
            var invoices = await _context.Invoices.ToListAsync();
            return invoices.Select(s => new InvoiceDTO
            {
                InvoiceId = s.InvoiceId,
                InvoiceCode = s.InvoiceCode,
                StudentId = s.StudentId,
                TotalAmount = s.TotalAmount,
                PaidAmount = s.PaidAmount,
                RemainingAmount = s.TotalAmount - (s.PaidAmount ?? 0),
                Status = s.Status
            }).ToList();
        }

        public async Task<InvoiceDTO> GetInvoiceByIdAsync(int invoiceId)
        {
            var invoice = await _context.Invoices.FindAsync(invoiceId);
            if (invoice == null)
                return null;

            return new InvoiceDTO
            {
                StudentId = invoice.StudentId,
                TotalAmount = invoice.TotalAmount,
                PaidAmount = invoice.PaidAmount,
                RemainingAmount = invoice.RemainingAmount,
                Status = invoice.Status
            };
        }

        public async Task<InvoiceDTO> UpdateInvoiceAsync(int invoiceId, InvoiceDTO invoiceDto)
        {
            var existingInvoice = await _context.Invoices.FindAsync(invoiceId);
            if (existingInvoice == null)
                return null;

            existingInvoice.StudentId = invoiceDto.StudentId;
            existingInvoice.TotalAmount = invoiceDto.TotalAmount;
            existingInvoice.PaidAmount = invoiceDto.PaidAmount;
            existingInvoice.RemainingAmount = invoiceDto.RemainingAmount;
            existingInvoice.Status = invoiceDto.Status;

            await _context.SaveChangesAsync();
            return invoiceDto;
        }
    }
}
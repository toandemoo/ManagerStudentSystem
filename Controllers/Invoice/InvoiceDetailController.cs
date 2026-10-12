using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ManagerStudentSystem.Dtos;
using ManagerStudentSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ManagerStudentSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InvoiceDetailController : ControllerBase
    {
        private readonly IInvoiceDetailService _invoiceDetailService;
        private readonly ILogger<InvoiceDetailController> _logger;

        public InvoiceDetailController(IInvoiceDetailService invoicedetailService, ILogger<InvoiceDetailController> logger)
        {
            _invoiceDetailService = invoicedetailService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetInvoicesDetailByInvoiceId(int invoiceId)
        {
            var invoiceDetails = await _invoiceDetailService.GetInvoiceDetailsByInvoiceId(invoiceId);
            return Ok(invoiceDetails);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetInvoiceById(int id)
        {
            var invoiceDetail = await _invoiceDetailService.GetInvoiceDetailById(id);
            if (invoiceDetail == null)
            {
                return NotFound();
            }
            return Ok(invoiceDetail);
        }

        [HttpPost]
        public async Task<IActionResult> CreateInoviceDetail([FromBody] InvoiceDetailDTO invoiceDetailDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdInvoiceDetail = await _invoiceDetailService.CreateInvoiceDetail(invoiceDetailDTO);
            if (createdInvoiceDetail == null)
            {
                return StatusCode(500, "An error occurred while creating the invoice.");
            }

            return Ok(createdInvoiceDetail);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateInvoiceDetail([FromRoute] int id, [FromBody] InvoiceDetailDTO invoiceDetailDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedInvoiceDetail = await _invoiceDetailService.UpdateInvoiceDetail(id, invoiceDetailDTO);
            if (updatedInvoiceDetail == null)
            {
                return NotFound();
            }

            return Ok(updatedInvoiceDetail);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInvoiceDetail([FromRoute] int id)
        {
            var result = await _invoiceDetailService.DeleteInvoiceDetail(id);
            if (!result)
            {
                return NotFound();
            }

            return Ok();
        }
    }
}
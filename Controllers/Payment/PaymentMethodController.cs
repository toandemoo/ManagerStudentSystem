using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ManagerStudentSystem.Dtos;
using ManagerStudentSystem.Services;
using ManagerStudentSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ManagerStudentSystem.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PaymentMethodController : ControllerBase
    {
        private readonly ILogger<PaymentMethodController> _logger;
        private readonly PaymentMethodService _paymentMethodService;

        public PaymentMethodController(ILogger<PaymentMethodController> logger, PaymentMethodService paymentMethodService)
        {
            _logger = logger;
            _paymentMethodService = paymentMethodService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPaymentMethods()
        {
            try
            {
                var paymentMethods = await _paymentMethodService.GetAllPaymentMethodsAsync();
                return Ok(paymentMethods);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving all payment methods.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPaymentMethodById(int id)
        {
            try
            {
                var paymentMethod = await _paymentMethodService.GetPaymentMethodByIdAsync(id);
                if (paymentMethod == null)
                {
                    return NotFound();
                }
                return Ok(paymentMethod);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while retrieving the payment method with ID {id}.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreatePaymentMethod([FromBody] PaymentMethodDTO paymentMethod)
        {
            try
            {
                var createdPaymentMethod = await _paymentMethodService.CreatePaymentMethodAsync(paymentMethod);
                if (createdPaymentMethod == null)
                {
                    return BadRequest("Failed to create payment method.");
                }
                return Ok(createdPaymentMethod);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a new payment method.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePaymentMethod(int id, [FromBody] PaymentMethodDTO paymentMethod)
        {
            try
            {
                var updatedPaymentMethod = await _paymentMethodService.UpdatePaymentMethodAsync(id, paymentMethod);
                if (updatedPaymentMethod == null)
                {
                    return NotFound();
                }
                return Ok(updatedPaymentMethod);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating the payment method with ID {id}.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePaymentMethod(int id)
        {
            try
            {
                var result = await _paymentMethodService.DeletePaymentMethodAsync(id);
                if (!result)
                {
                    return NotFound();
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting the payment method with ID {id}.");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ManagerStudentSystem.Dtos;
using ManagerStudentSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ManagerStudentSystem.Controllers
{
    [Route("[controller]")]
    public class IdempotencyKey : Controller
    {
        private readonly ILogger<IdempotencyKey> _logger;
        private readonly IPaymentService _paymentService;

        public IdempotencyKey(ILogger<IdempotencyKey> logger, IPaymentService paymentService)
        {
            _logger = logger;
            _paymentService = paymentService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(PaymentRequestDTO request)
        {
            var idempotencyKey = Request.Headers["Idempotency-Key"].FirstOrDefault();

            if (string.IsNullOrWhiteSpace(idempotencyKey))
            {
                return BadRequest(
                    "Idempotency-Key is required.");
            }

            var result = await _paymentService.CreatePaymentAsync(request, idempotencyKey);

            return Ok(result);
        }
    }
}
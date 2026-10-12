using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ManagerStudentSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ManagerStudentSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentWebhookController : ControllerBase
    {
        private readonly IWebhookService _webhookService;
        private readonly ILogger<PaymentWebhookController> _logger;

        public PaymentWebhookController(
            IWebhookService webhookService,
            ILogger<PaymentWebhookController> logger)
        {
            _webhookService = webhookService;
            _logger = logger;
        }

        [HttpPost("{providerCode}")]
        public async Task<IActionResult> Handle(string providerCode)
        {
            using var reader = new StreamReader(Request.Body);

            var payload = await reader.ReadToEndAsync();

            var signature = Request.Headers["X-Signature"].FirstOrDefault();

            // await _webhookService.HandleAsync(
            //     providerCode,
            //     payload,
            //     signature);

            return Ok(payload);
        }
    }
}
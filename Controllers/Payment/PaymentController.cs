using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ManagerStudentSystem.Dtos;
using ManagerStudentSystem.Dtos.Refund;
using ManagerStudentSystem.Services;
using ManagerStudentSystem.Services.Interfaces;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace ManagerStudentSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {

        private readonly IPaymentService _paymentService;
        private readonly IIdempotencyService _idempotencyService;
        private readonly ILogger<PaymentController> _logger;

        public PaymentController(
            IPaymentService paymentService,
            ILogger<PaymentController> logger)
        {
            _paymentService = paymentService;
            _logger = logger;
        }

        [HttpGet("vnpay-return")]
        public async Task<IActionResult> VNPayReturn()
        {
            // xử lý kết quả từ VNPAY
            var result = await _paymentService.ReturnResult(Request.Query);
            return Ok(result);
        }

        [HttpGet("vnpay-ipn")]
        public async Task<IActionResult> VNPayIpn()
        {
            var result = await _paymentService.IpnResult(Request.Query);
            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpPost("refund")]
        public async Task<IActionResult> Refund([FromBody] RefundRequestDTO request)
        {
            var result = await _paymentService.RefundAsync(request);

            if (!result.IsValidSignature)
            {
                return BadRequest(result);
            }

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }


        [HttpPost("vnpay")]
        public async Task<IActionResult> Create(PaymentRequestDTO request)
        {
            // var key = Request.Headers["Idempotency-Key"].FirstOrDefault();
            // if (string.IsNullOrWhiteSpace(key))
            //     return BadRequest(
            //         "Idempotency-Key is required.");

            var key = Request.Headers["Idempotency-Key"].FirstOrDefault() ?? "";

            var result = await _paymentService.CreatePaymentAsync(request, key);
            return Ok(result);
        }

        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await _paymentService.GetByIdAsync(id);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpPost("retry")]
        public async Task<IActionResult> Retry([FromBody] string paymentCode)
        {
            var result = await _paymentService.RetryAsync(paymentCode);

            return Ok(result);
        }

        [HttpPost("cancel")]
        public async Task<IActionResult> Cancel([FromBody] string paymentCode)
        {
            await _paymentService.CancelAsync(paymentCode);
            return NoContent();
        }
    }
}
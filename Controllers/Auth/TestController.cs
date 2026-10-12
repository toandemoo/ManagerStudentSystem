using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ManagerStudentSystem.Controllers.Auth
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        [HttpGet("zalo/callback")]
        public async Task<IActionResult> ZaloCallback([FromQuery] string? code, [FromQuery] string? state)
        {
            // 1. Kiểm tra code
            if (string.IsNullOrWhiteSpace(code))
            {
                return BadRequest(new
                {
                    message = "Authorization code is missing."
                });
            }

            // 2. Kiểm tra state
            if (string.IsNullOrWhiteSpace(state))
            {
                return BadRequest(new
                {
                    message = "State is missing."
                });
            }

            // xử lý OAuth...

            var response = new
            {
                message = "Zalo OAuth callback received.",
                code = code,
                state = state
            };

            return Ok(response);
        }
    }
}
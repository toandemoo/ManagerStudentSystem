using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ManagerStudentSystem.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SessionController : ControllerBase
    {
        private readonly ILogger<SessionController> _logger;

        public SessionController(ILogger<SessionController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var sessionValue = HttpContext.Session.GetString("MySessionKey");
            return Ok(sessionValue);
        }

        [HttpPost]
        public IActionResult Set([FromBody] string value)
        {
            HttpContext.Session.SetString("MySessionKey", value);
            return Ok();
        }
    }
}
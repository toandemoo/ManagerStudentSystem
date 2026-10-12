using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ManagerStudentSystem.Dtos.Fee;
using ManagerStudentSystem.Services.Interfaces.Fee;
using Microsoft.AspNetCore.Mvc;

namespace ManagerStudentSystem.Controllers.Fee
{
    [ApiController]
    [Route("api/[controller]")]
    public class FeePeriodController : ControllerBase
    {
        private readonly IFeePeriodService _feePeriodService;
        private readonly ILogger<FeePeriodController> _logger;
        public FeePeriodController(
            IFeePeriodService feePeriodService,
            ILogger<FeePeriodController> logger)
        {
            _feePeriodService = feePeriodService;
            _logger = logger;
        }
        [HttpPost]
        public async Task<IActionResult> CreateFeePeriod([FromBody] FeePeriodRequestDTO feePeriodDTO)
        {
            try
            {
                var feePeriod = await _feePeriodService.CreateFeePeriodAsync(feePeriodDTO);
                return Ok(feePeriod);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a new Fee Period.");
                return StatusCode(500, "An internal error occurred.");
            }
        }
    }
}
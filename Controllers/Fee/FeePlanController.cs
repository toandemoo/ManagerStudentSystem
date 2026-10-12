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
    [ApiController]
    [Route("[controller]")]
    public class FeePlanController : ControllerBase
    {
        private readonly ILogger<FeePlanController> _logger;
        private readonly IFeePlanService _feePlanService;

        public FeePlanController(ILogger<FeePlanController> logger, IFeePlanService feePlanService)
        {
            _logger = logger;
            _feePlanService = feePlanService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateFeePlan([FromBody] FeePlanDTO feePlanDto)
        {
            if (feePlanDto == null)
            {
                return BadRequest("Fee plan data is required.");
            }

            var createdFeePlan = await _feePlanService.CreateFeePlanAsync(feePlanDto);
            if (createdFeePlan == null)
            {
                return StatusCode(500, "An error occurred while creating the fee plan.");
            }

            return Ok(createdFeePlan);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllFeePlans()
        {
            var feePlans = await _feePlanService.GetAllFeePlansAsync();
            return Ok(feePlans);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFeePlanById(int id)
        {
            var feePlan = await _feePlanService.GetFeePlanByIdAsync(id);
            if (feePlan == null)
            {
                return NotFound();
            }
            return Ok(feePlan);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFeePlan(int id, [FromBody] FeePlanDTO feePlanDto)
        {
            if (feePlanDto == null)
            {
                return BadRequest("Fee plan data is required.");
            }

            var updatedFeePlan = await _feePlanService.UpdateFeePlanAsync(id, feePlanDto);
            if (updatedFeePlan == null)
            {
                return NotFound();
            }

            return Ok(updatedFeePlan);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFeePlan(int id)
        {
            var deleted = await _feePlanService.DeleteFeePlanAsync(id);
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
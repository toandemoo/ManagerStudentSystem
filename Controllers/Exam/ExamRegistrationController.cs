using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ManagerStudentSystem.Dtos;
using ManagerStudentSystem.Dtos.Exam;
using ManagerStudentSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ManagerStudentSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExamRegistrationController : ControllerBase
    {
        private readonly ILogger<ExamRegistrationController> _logger;
        private readonly IExamRegistrationService _examRegistrationService;

        public ExamRegistrationController(ILogger<ExamRegistrationController> logger, IExamRegistrationService examService)
        {
            _logger = logger;
            _examRegistrationService = examService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllExamRegistrations()
        {
            var examRegistrationss = await _examRegistrationService.GetAllExamRegistrationAsync();
            return Ok(examRegistrationss);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetExamRegistrationsById(int id)
        {
            var examRegistrations = await _examRegistrationService.GetExamRegistrationByIdAsync(id);
            if (examRegistrations == null)
            {
                return NotFound();
            }
            return Ok(examRegistrations);
        }

        [HttpPost]
        public async Task<IActionResult> CreateExamRegistration([FromBody] ExamRegistrationRequestDTO exam)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdExamRegistration = await _examRegistrationService.CreateExamRegistrationAsync(exam);
            if (createdExamRegistration == null)
            {
                return StatusCode(500, "An error occurred while creating the exam Registration.");
            }

            return Ok(createdExamRegistration);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateExamRegistration([FromRoute] int id, [FromBody] ExamRegistrationDTO exam)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedExamRegistration = await _examRegistrationService.UpdateExamRegistrationAsync(id, exam);
            if (updatedExamRegistration == null)
            {
                return NotFound();
            }

            return Ok(updatedExamRegistration);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExamRegistration([FromRoute] int id)
        {
            var result = await _examRegistrationService.DeleteExamRegistrationAsync(id);
            if (!result)
            {
                return NotFound();
            }

            return Ok();
        }
    }
}
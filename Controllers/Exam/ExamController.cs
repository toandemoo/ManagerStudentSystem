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
    public class ExamController : ControllerBase
    {
        private readonly ILogger<ExamController> _logger;
        private readonly IExamService _examService;

        public ExamController(ILogger<ExamController> logger, IExamService examService)
        {
            _logger = logger;
            _examService = examService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllExams()
        {
            var exams = await _examService.GetAllExamAsync();
            return Ok(exams);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetExamById(int id)
        {
            var exam = await _examService.GetExamByIdAsync(id);
            if (exam == null)
            {
                return NotFound();
            }
            return Ok(exam);
        }

        [HttpPost]
        public async Task<IActionResult> CreateExam([FromBody] ExamRequestDTO exam)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdExam = await _examService.CreateExamAsync(exam);

            if (createdExam == null)
            {
                return StatusCode(500, "An error occurred while creating the exam.");
            }

            return Ok(createdExam);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateExam([FromRoute] int id, [FromBody] ExamDTO exam)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedExam = await _examService.UpdateExamAsync(id, exam);
            if (updatedExam == null)
            {
                return NotFound();
            }

            return Ok(updatedExam);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExam([FromRoute] int id)
        {
            var result = await _examService.DeleteExamAsync(id);
            if (!result)
            {
                return NotFound();
            }

            return Ok();
        }
    }
}
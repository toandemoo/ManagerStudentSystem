using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ManagerStudentSystem.Dtos;
using ManagerStudentSystem.Dtos.Exam;
using ManagerStudentSystem.Models;
using ManagerStudentSystem.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ManagerStudentSystem.Services
{
    public class ExamService : IExamService
    {
        private readonly TaekwondoDbContext _context;
        private readonly ILogger<ExamService> _logger;

        public ExamService(TaekwondoDbContext context, ILogger<ExamService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<ExamDTO> CreateExamAsync(ExamRequestDTO examDTO)
        {
            try
            {
                var newExam = new Exam
                {
                    ExamName = examDTO.ExamName,
                    ExamDate = examDTO.ExamDate,
                    Location = examDTO.Location,
                    RegistrationDeadline = examDTO.RegistrationDeadline,
                    Description = examDTO.Description
                };

                _context.Exams.Add(newExam);
                await _context.SaveChangesAsync();

                return new ExamDTO
                {
                    ExamId = newExam.ExamId,
                    ExamName = newExam.ExamName,
                    ExamDate = newExam.ExamDate,
                    Location = newExam.Location,
                    RegistrationDeadline = newExam.RegistrationDeadline,
                    Status = newExam.Status,
                    Description = newExam.Description
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating exam");
                return null;
            }
        }

        public async Task<bool> DeleteExamAsync(int examId)
        {
            try
            {
                var exam = await _context.Exams.FindAsync(examId);
                if (exam == null)
                    return false;

                _context.Exams.Remove(exam);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting exam");
                return false;
            }
        }

        public async Task<List<ExamDTO>> GetAllExamAsync()
        {
            var exams = await _context.Exams.ToListAsync();
            return exams.Select(s => new ExamDTO
            {
                ExamName = s.ExamName,
                ExamDate = s.ExamDate,
                Location = s.Location,
                RegistrationDeadline = s.RegistrationDeadline,
                Status = s.Status,
                Description = s.Description
            }).ToList();
        }

        public async Task<ExamDTO> GetExamByIdAsync(int examId)
        {
            var exam = await _context.Exams.FindAsync(examId);
            if (exam == null)
                return null;

            return new ExamDTO
            {
                ExamName = exam.ExamName,
                ExamDate = exam.ExamDate,
                Location = exam.Location,
                RegistrationDeadline = exam.RegistrationDeadline,
                Status = exam.Status,
                Description = exam.Description
            };
        }

        public async Task<ExamDTO> UpdateExamAsync(int examId, ExamDTO examDTO)
        {
            var existingExam = await _context.Exams.FindAsync(examId);
            if (existingExam == null)
                return null;

            existingExam.ExamName = examDTO.ExamName;
            existingExam.ExamDate = examDTO.ExamDate;
            existingExam.Location = examDTO.Location;
            existingExam.RegistrationDeadline = examDTO.RegistrationDeadline;
            existingExam.Description = examDTO.Description;

            await _context.SaveChangesAsync();
            return new ExamDTO
            {
                ExamName = existingExam.ExamName,
                ExamDate = existingExam.ExamDate,
                Location = existingExam.Location,
                RegistrationDeadline = existingExam.RegistrationDeadline,
                Description = existingExam.Description
            };
        }
    }
}
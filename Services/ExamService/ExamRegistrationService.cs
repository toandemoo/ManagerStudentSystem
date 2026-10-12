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
    public class ExamRegistrationService : IExamRegistrationService
    {
        private readonly TaekwondoDbContext _context;
        private readonly ILogger<ExamRegistrationService> _logger;

        public ExamRegistrationService(TaekwondoDbContext context, ILogger<ExamRegistrationService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<ExamRegistrationDTO> CreateExamRegistrationAsync(ExamRegistrationRequestDTO examRegistrationRequestDTO)
        {
            try
            {
                var newExamRegistration = new ExamRegistration
                {
                    ExamId = examRegistrationRequestDTO.ExamId,
                    StudentId = examRegistrationRequestDTO.StudentId,
                    RegistrationDate = examRegistrationRequestDTO.RegistrationDate
                };

                _context.ExamRegistrations.Add(newExamRegistration);
                await _context.SaveChangesAsync();

                return new ExamRegistrationDTO
                {
                    ExamRegistrationId = newExamRegistration.ExamRegistrationId,
                    ExamId = newExamRegistration.ExamId,
                    StudentId = newExamRegistration.StudentId,
                    RegistrationDate = newExamRegistration.RegistrationDate,
                    Status = newExamRegistration.Status
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating examRegistration");
                return null;
            }
        }

        public async Task<bool> DeleteExamRegistrationAsync(int examRegistrationId)
        {
            try
            {
                var examRegistration = await _context.ExamRegistrations.FindAsync(examRegistrationId);
                if (examRegistration == null)
                    return false;

                _context.ExamRegistrations.Remove(examRegistration);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting examRegistration");
                return false;
            }
        }

        public async Task<List<ExamRegistrationDTO>> GetAllExamRegistrationAsync()
        {
            var examRegistrations = await _context.ExamRegistrations.ToListAsync();
            return examRegistrations.Select(s => new ExamRegistrationDTO
            {
                ExamRegistrationId = s.ExamRegistrationId,
                ExamId = s.ExamId,
                StudentId = s.StudentId,
                RegistrationDate = s.RegistrationDate,
                Status = s.Status
            }).ToList();
        }

        public async Task<ExamRegistrationDTO> GetExamRegistrationByIdAsync(int examRegistrationId)
        {
            var examRegistration = await _context.ExamRegistrations.FindAsync(examRegistrationId);
            if (examRegistration == null)
                return null;

            return new ExamRegistrationDTO
            {
                ExamId = examRegistration.ExamId,
                ExamRegistrationId = examRegistration.ExamRegistrationId,
                StudentId = examRegistration.StudentId,
                RegistrationDate = examRegistration.RegistrationDate,
                Status = examRegistration.Status
            };
        }

        public async Task<ExamRegistrationDTO> UpdateExamRegistrationAsync(int examRegistrationId, ExamRegistrationDTO examRegistrationDTO)
        {
            var existingExamRegistration = await _context.ExamRegistrations.FindAsync(examRegistrationId);
            if (existingExamRegistration == null)
                return null;

            existingExamRegistration.ExamId = examRegistrationDTO.ExamId;
            existingExamRegistration.ExamRegistrationId = examRegistrationDTO.ExamRegistrationId;
            existingExamRegistration.StudentId = examRegistrationDTO.StudentId;
            existingExamRegistration.RegistrationDate = examRegistrationDTO.RegistrationDate;
            existingExamRegistration.Status = examRegistrationDTO.Status;


            await _context.SaveChangesAsync();
            return examRegistrationDTO;
        }
    }
}
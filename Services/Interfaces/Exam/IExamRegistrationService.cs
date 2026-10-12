using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ManagerStudentSystem.Dtos;
using ManagerStudentSystem.Dtos.Exam;

namespace ManagerStudentSystem.Services.Interfaces
{
    public interface IExamRegistrationService
    {
        Task<List<ExamRegistrationDTO>> GetAllExamRegistrationAsync();
        Task<ExamRegistrationDTO> GetExamRegistrationByIdAsync(int examId);
        Task<ExamRegistrationDTO> CreateExamRegistrationAsync(ExamRegistrationRequestDTO examDTO);
        Task<ExamRegistrationDTO> UpdateExamRegistrationAsync(int examId, ExamRegistrationDTO examDTO);
        Task<bool> DeleteExamRegistrationAsync(int examId);
    }
}
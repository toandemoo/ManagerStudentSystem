using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ManagerStudentSystem.Dtos;
using ManagerStudentSystem.Dtos.Exam;
using ManagerStudentSystem.Models;

namespace ManagerStudentSystem.Services.Interfaces
{
    public interface IExamService
    {
        Task<List<ExamDTO>> GetAllExamAsync();
        Task<ExamDTO> GetExamByIdAsync(int examId);
        Task<ExamDTO> CreateExamAsync(ExamRequestDTO examDTO);
        Task<ExamDTO> UpdateExamAsync(int examId, ExamDTO examDTO);
        Task<bool> DeleteExamAsync(int examId);
    }
}
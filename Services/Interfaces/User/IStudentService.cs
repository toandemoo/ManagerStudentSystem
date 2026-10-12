using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ManagerStudentSystem.Dtos;
using ManagerStudentSystem.Models;

namespace ManagerStudentSystem.InterfaceServices
{
    public interface IStudentService
    {
        Task<List<StudentDTO>> GetAllStudentsAsync();
        Task<StudentDTO> GetStudentByIdAsync(int studentId);
        Task<StudentDTO> CreateStudentAsync(int userID);
        Task<StudentDTO> UpdateStudentAsync(int studentId, StudentDTO student);
        Task<bool> DeleteStudentAsync(int studentId);
    }
}
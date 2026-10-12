using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ManagerStudentSystem.Models;
using ManagerStudentSystem.InterfaceServices;
using ManagerStudentSystem.Dtos;
using Microsoft.EntityFrameworkCore;

namespace ManagerStudentSystem.Services
{
    public class StudentService : IStudentService
    {
        private readonly TaekwondoDbContext _context;
        private readonly ILogger<StudentService> _logger;

        public StudentService(TaekwondoDbContext context, ILogger<StudentService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<StudentDTO> CreateStudentAsync(int userID)
        {
            try
            {
                var newStudent = new Student
                {
                    UserId = userID,
                    LevelId = 1
                };

                _context.Students.Add(newStudent);
                await _context.SaveChangesAsync();

                return new StudentDTO
                {
                    UserId = newStudent.UserId,
                    StudentId = newStudent.StudentId,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating student");
                return null;
            }
        }

        public async Task<bool> DeleteStudentAsync(int studentId)
        {
            try
            {
                var student = await _context.Students.FindAsync(studentId);
                if (student == null)
                    return false;

                _context.Students.Remove(student);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting student");
                return false;
            }
        }

        public async Task<List<StudentDTO>> GetAllStudentsAsync()
        {
            var students = await _context.Students.ToListAsync();
            return students.Select(s => new StudentDTO
            {
                StudentId = s.StudentId,
                StudentCode = s.StudentCode,
                UserId = s.UserId,
                LevelId = s.LevelId,
                Status = s.Status
            }).ToList();
        }

        public async Task<StudentDTO> GetStudentByIdAsync(int studentId)
        {
            var student = await _context.Students.FindAsync(studentId);
            if (student == null)
                return null;

            return new StudentDTO
            {
                StudentId = student.StudentId,
                StudentCode = student.StudentCode,
                UserId = student.UserId,
                LevelId = student.LevelId,
                Status = student.Status
            };
        }

        public async Task<StudentDTO> UpdateStudentAsync(int studentId, StudentDTO student)
        {
            var existingStudent = await _context.Students.FindAsync(studentId);
            if (existingStudent == null)
                return null;

            existingStudent.StudentCode = student.StudentCode;
            existingStudent.LevelId = (int)student.LevelId;

            await _context.SaveChangesAsync();
            return student;
        }
    }
}
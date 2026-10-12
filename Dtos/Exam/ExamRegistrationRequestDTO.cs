using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManagerStudentSystem.Dtos.Exam
{
    public class ExamRegistrationRequestDTO
    {
        public int ExamId { get; set; }
        public int StudentId { get; set; }
        public DateOnly RegistrationDate { get; set; }
    }
}
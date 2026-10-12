using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManagerStudentSystem.Dtos.Exam
{
    public class ExamRequestDTO
    {
        public string ExamName { get; set; } = null!;
        public DateOnly ExamDate { get; set; }
        public string Location { get; set; } = null!;
        public DateOnly RegistrationDeadline { get; set; }
        public string Description { get; set; } = null!;
    }
}
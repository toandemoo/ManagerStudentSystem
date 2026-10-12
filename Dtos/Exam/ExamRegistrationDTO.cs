using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManagerStudentSystem.Dtos
{
    public class ExamRegistrationDTO
    {
        public int ExamRegistrationId { get; set; }
        public int ExamId { get; set; }
        public int StudentId { get; set; }
        public DateOnly RegistrationDate { get; set; }
        public string Status { get; set; } = null!;
    }
}
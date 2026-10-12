using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManagerStudentSystem.Dtos
{
    public class StudentDTO
    {
        public int StudentId { get; set; }
        public string StudentCode { get; set; } = null!;
        public int? LevelId { get; set; }
        public int UserId { get; set; }
        public string Status { get; set; } = null!;
    }
}
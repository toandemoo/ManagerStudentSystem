using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManagerStudentSystem.Dtos
{
    public class LoginRequestDTO
    {
        public string UserName { get; set; } = null!;
        public string PassWord { get; set; } = null!;
    }
}
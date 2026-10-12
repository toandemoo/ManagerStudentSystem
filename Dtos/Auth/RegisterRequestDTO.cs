using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManagerStudentSystem.Dtos.Auth
{
    public class RegisterRequestDTO
    {
        public string UserName { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public DateOnly DateOfBirth { get; set; }
        public bool Gender { get; set; }
        public string Email { get; set; } = null!;
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManagerStudentSystem.Dtos
{
    public class LoginResponseDTO
    {
        public string UserName { get; set; }
        public string AccessToken { get; set; }
        public int ExpireIn { get; set; }
        public string? RefreshToken { get; set; }
    }
}
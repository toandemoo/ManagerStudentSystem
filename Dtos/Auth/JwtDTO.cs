using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManagerStudentSystem.Models
{
    public class JwtDTO
    {
        public string AccessToken { get; set; }
        public int ExpireIn { get; set; }
        public string RefreshToken { get; set; }

    }
}
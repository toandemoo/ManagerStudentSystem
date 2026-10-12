using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManagerStudentSystem.Dtos
{
    public class RefreshTokenDTO
    {
        public int UserId { get; set; }
        public string TokenHash { get; set; } = null!;
        public DateTime? CreateAt { get; set; }
        public DateTime? ExpireIn { get; set; }
        public DateTime? RevoteAt { get; set; }
        public int RefreshTokenId { get; set; }
    }
}
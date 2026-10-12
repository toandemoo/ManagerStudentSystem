using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManagerStudentSystem.Dtos
{
    public class IdempotencyResult
    {
        public string RequestHash { get; }
        public int ResponseStatus { get; }
        public string ResponseBody { get; }
        public TimeSpan Expiration { get; }
    }
}
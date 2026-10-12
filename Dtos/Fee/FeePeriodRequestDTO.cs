using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManagerStudentSystem.Dtos.Fee
{
    public class FeePeriodRequestDTO
    {
        public string FeePeriodCode { get; set; } = null!;
        public string PeriodName { get; set; } = null!;
        public decimal Amount { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
    }
}
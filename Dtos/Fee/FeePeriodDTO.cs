using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManagerStudentSystem.Dtos.Fee
{
    public class FeePeriodDTO
    {
        public int FeePeriodId { get; set; }
        public string FeePeriodCode { get; set; } = null!;
        public string PeriodName { get; set; } = null!;
        public decimal Amount { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public DateOnly DueDate { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? PublishedAt { get; set; }
    }
}
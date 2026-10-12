using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ManagerStudentSystem.Models;

[Table("FeePeriods", Schema = "Taekwondo")]
[Index("FeePeriodCode", Name = "UQ__FeePerio__12F0FDFEAE75FA3E", IsUnique = true)]
public partial class FeePeriod
{
    [Key]
    public int FeePeriodId { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string FeePeriodCode { get; set; } = null!;

    [StringLength(50)]
    public string PeriodName { get; set; } = null!;

    [Column(TypeName = "decimal(10, 2)")]
    public decimal Amount { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public DateOnly DueDate { get; set; }

    public bool IsActive { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreatedAt { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? PublishedAt { get; set; }

    [InverseProperty("FeePeriod")]
    public virtual ICollection<StudentFee> StudentFees { get; set; } = new List<StudentFee>();
}

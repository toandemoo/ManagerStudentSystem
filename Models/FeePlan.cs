using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ManagerStudentSystem.Models;

[Table("FeePlans", Schema = "Taekwondo")]
public partial class FeePlan
{
    [Key]
    public int FeePlanId { get; set; }

    [StringLength(50)]
    public string FeePlanName { get; set; } = null!;

    [Column(TypeName = "decimal(10, 2)")]
    public decimal FeeAmountMonthly { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal Amount { get; set; }

    [Column(TypeName = "decimal(5, 2)")]
    public decimal DisCountPercentage { get; set; }

    public bool IsActive { get; set; }

    [StringLength(255)]
    public string? Description { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreatedAt { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UpdatedAt { get; set; }

    [InverseProperty("FeePlan")]
    public virtual ICollection<StudentFeePlan> StudentFeePlans { get; set; } = new List<StudentFeePlan>();
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ManagerStudentSystem.Models;

[PrimaryKey("StudentId", "FeePlanId")]
[Table("StudentFeePlans", Schema = "Taekwondo")]
public partial class StudentFeePlan
{
    [Key]
    public int StudentId { get; set; }

    [Key]
    public int FeePlanId { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreatedAt { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UpdatedAt { get; set; }

    [ForeignKey("FeePlanId")]
    [InverseProperty("StudentFeePlans")]
    public virtual FeePlan FeePlan { get; set; } = null!;

    [ForeignKey("StudentId")]
    [InverseProperty("StudentFeePlans")]
    public virtual Student Student { get; set; } = null!;
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ManagerStudentSystem.Models;

[Table("StudentFees", Schema = "Taekwondo")]
[Index("StudentId", "FeePeriodId", Name = "UQ_StudentFees", IsUnique = true)]
public partial class StudentFee
{
    [Key]
    public int StudentFeeId { get; set; }

    public int StudentId { get; set; }

    public int FeePeriodId { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal Amount { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal DiscountAmount { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal FinalAmount { get; set; }

    [StringLength(50)]
    public string Status { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime? IssuedAt { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DueDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? PaidAt { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreatedAt { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UpdatedAt { get; set; }

    [ForeignKey("FeePeriodId")]
    [InverseProperty("StudentFees")]
    public virtual FeePeriod FeePeriod { get; set; } = null!;

    [ForeignKey("StudentId")]
    [InverseProperty("StudentFees")]
    public virtual Student Student { get; set; } = null!;
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ManagerStudentSystem.Models;

[Table("Students", Schema = "Taekwondo")]
[Index("UserId", Name = "UQ__Students__1788CC4D1C921463", IsUnique = true)]
[Index("StudentCode", Name = "UQ__Students__1FC886040ABC80F1", IsUnique = true)]
public partial class Student
{
    [Key]
    public int StudentId { get; set; }

    public int UserId { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string StudentCode { get; set; } = null!;

    public int LevelId { get; set; }

    [StringLength(50)]
    public string Status { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime CreatedAt { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UpdatedAt { get; set; }

    [InverseProperty("Student")]
    public virtual ICollection<ExamRegistration> ExamRegistrations { get; set; } = new List<ExamRegistration>();

    [InverseProperty("Student")]
    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();

    [ForeignKey("LevelId")]
    [InverseProperty("Students")]
    public virtual Level Level { get; set; } = null!;

    [InverseProperty("Student")]
    public virtual ICollection<StudentFeePlan> StudentFeePlans { get; set; } = new List<StudentFeePlan>();

    [InverseProperty("Student")]
    public virtual ICollection<StudentFee> StudentFees { get; set; } = new List<StudentFee>();

    [ForeignKey("UserId")]
    [InverseProperty("Student")]
    public virtual User User { get; set; } = null!;
}

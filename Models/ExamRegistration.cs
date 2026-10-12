using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ManagerStudentSystem.Models;

[Table("ExamRegistrations", Schema = "Taekwondo")]
public partial class ExamRegistration
{
    [Key]
    public int ExamRegistrationId { get; set; }

    public int ExamId { get; set; }

    public int StudentId { get; set; }

    public DateOnly RegistrationDate { get; set; }

    [StringLength(20)]
    public string Status { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    [ForeignKey("ExamId")]
    [InverseProperty("ExamRegistrations")]
    public virtual Exam Exam { get; set; } = null!;

    [ForeignKey("StudentId")]
    [InverseProperty("ExamRegistrations")]
    public virtual Student Student { get; set; } = null!;
}

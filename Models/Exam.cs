using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ManagerStudentSystem.Models;

[Table("Exams", Schema = "Taekwondo")]
public partial class Exam
{
    [Key]
    public int ExamId { get; set; }

    [StringLength(100)]
    public string ExamName { get; set; } = null!;

    public DateOnly ExamDate { get; set; }

    [StringLength(100)]
    public string Location { get; set; } = null!;

    public DateOnly RegistrationDeadline { get; set; }

    [StringLength(20)]
    public string Status { get; set; } = null!;

    [StringLength(255)]
    public string Description { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    [InverseProperty("Exam")]
    public virtual ICollection<ExamRegistration> ExamRegistrations { get; set; } = new List<ExamRegistration>();
}

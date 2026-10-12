using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ManagerStudentSystem.Models;

[Table("Levels", Schema = "Taekwondo")]
public partial class Level
{
    [Key]
    public int LevelId { get; set; }

    [StringLength(50)]
    public string LevelName { get; set; } = null!;

    [Column("DESCRIPTION")]
    [StringLength(255)]
    public string? Description { get; set; }

    [InverseProperty("Level")]
    public virtual ICollection<Student> Students { get; set; } = new List<Student>();
}

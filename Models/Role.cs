using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ManagerStudentSystem.Models;

[Table("Roles", Schema = "Taekwondo")]
public partial class Role
{
    [Key]
    public int RoleId { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string RoleName { get; set; } = null!;

    [StringLength(255)]
    public string? Description { get; set; }

    [ForeignKey("RoleId")]
    [InverseProperty("Roles")]
    public virtual ICollection<User> Users { get; set; } = new List<User>();
}

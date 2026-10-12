using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ManagerStudentSystem.Models;

[Table("RefreshTokens", Schema = "Taekwondo")]
public partial class RefreshToken
{
    [Key]
    public int RefreshTokenId { get; set; }

    public int UserId { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string TokenHash { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime? CreateAt { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ExpireIn { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? RevoteAt { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("RefreshTokens")]
    public virtual User User { get; set; } = null!;
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ManagerStudentSystem.Models;

[Table("Items", Schema = "Taekwondo")]
public partial class Item
{
    [Key]
    public int ItemId { get; set; }

    [StringLength(100)]
    public string ItemName { get; set; } = null!;

    public byte ItemTypeId { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal Price { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    [ForeignKey("ItemTypeId")]
    [InverseProperty("Items")]
    public virtual ItemType ItemType { get; set; } = null!;
}

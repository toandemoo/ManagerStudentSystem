using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ManagerStudentSystem.Models;

[Table("ItemTypes", Schema = "Taekwondo")]
public partial class ItemType
{
    [Key]
    public byte ItemTypeId { get; set; }

    [StringLength(50)]
    public string ItemTypeName { get; set; } = null!;

    [StringLength(255)]
    public string? Description { get; set; }

    [InverseProperty("ItemType")]
    public virtual ICollection<InvoiceDetail> InvoiceDetails { get; set; } = new List<InvoiceDetail>();

    [InverseProperty("ItemType")]
    public virtual ICollection<Item> Items { get; set; } = new List<Item>();
}

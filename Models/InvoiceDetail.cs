using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ManagerStudentSystem.Models;

[Table("InvoiceDetails", Schema = "Taekwondo")]
public partial class InvoiceDetail
{
    [Key]
    public int InvoiceDetailId { get; set; }

    public int InvoiceId { get; set; }

    public byte ItemTypeId { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal UnitPrice { get; set; }

    public int Quantity { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal TotalPrice { get; set; }

    [ForeignKey("InvoiceId")]
    [InverseProperty("InvoiceDetails")]
    public virtual Invoice Invoice { get; set; } = null!;

    [ForeignKey("ItemTypeId")]
    [InverseProperty("InvoiceDetails")]
    public virtual ItemType ItemType { get; set; } = null!;
}

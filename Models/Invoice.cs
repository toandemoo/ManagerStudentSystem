using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ManagerStudentSystem.Models;

[Table("Invoices", Schema = "Taekwondo")]
[Index("InvoiceCode", Name = "UQ__Invoices__0D9D7FF35FEB5A73", IsUnique = true)]
public partial class Invoice
{
    [Key]
    public int InvoiceId { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string InvoiceCode { get; set; } = null!;

    public int StudentId { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal SubTotal { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal DiscountAmount { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal TotalAmount { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal? PaidAmount { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal? RemainingAmount { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string Status { get; set; } = null!;

    public DateTime? IssuedAt { get; set; }

    public DateTime? DueAt { get; set; }

    public DateTime? PaidAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    [InverseProperty("Invoice")]
    public virtual ICollection<InvoiceDetail> InvoiceDetails { get; set; } = new List<InvoiceDetail>();

    [InverseProperty("Invoice")]
    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    [ForeignKey("StudentId")]
    [InverseProperty("Invoices")]
    public virtual Student Student { get; set; } = null!;
}

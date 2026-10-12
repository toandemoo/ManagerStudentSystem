using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ManagerStudentSystem.Models;

[Table("Refunds", Schema = "Taekwondo")]
[Index("RefundCode", Name = "UQ__Refunds__7DFE7F49BED3966E", IsUnique = true)]
public partial class Refund
{
    [Key]
    public int RefundId { get; set; }

    public int PaymentId { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string RefundCode { get; set; } = null!;

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Amount { get; set; }

    [StringLength(500)]
    public string? Reason { get; set; }

    public byte Status { get; set; }

    [StringLength(200)]
    [Unicode(false)]
    public string? ProviderRefundId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? CompletedAt { get; set; }

    [ForeignKey("PaymentId")]
    [InverseProperty("Refunds")]
    public virtual Payment Payment { get; set; } = null!;
}

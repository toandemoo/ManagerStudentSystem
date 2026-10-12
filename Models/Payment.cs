using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ManagerStudentSystem.Models;

[Table("Payments", Schema = "Taekwondo")]
[Index("PaymentCode", Name = "UQ__Payments__106D3BA8DC3AF5BC", IsUnique = true)]
public partial class Payment
{
    [Key]
    public int PaymentId { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string PaymentCode { get; set; } = null!;

    public int InvoiceId { get; set; }

    public int ProviderId { get; set; }

    public int PaymentMethodId { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal Amount { get; set; }

    public DateTime PaymentDate { get; set; }

    [StringLength(3)]
    [Unicode(false)]
    public string Currency { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string Status { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? PaidAt { get; set; }

    [ForeignKey("InvoiceId")]
    [InverseProperty("Payments")]
    public virtual Invoice Invoice { get; set; } = null!;

    [InverseProperty("PaymentCodeNavigation")]
    public virtual ICollection<PaymentAttempt> PaymentAttempts { get; set; } = new List<PaymentAttempt>();

    [ForeignKey("PaymentMethodId")]
    [InverseProperty("Payments")]
    public virtual PaymentMethod PaymentMethod { get; set; } = null!;

    [InverseProperty("Payment")]
    public virtual ICollection<PaymentTransaction> PaymentTransactions { get; set; } = new List<PaymentTransaction>();

    [ForeignKey("ProviderId")]
    [InverseProperty("Payments")]
    public virtual PaymentProvider Provider { get; set; } = null!;

    [InverseProperty("Payment")]
    public virtual ICollection<Refund> Refunds { get; set; } = new List<Refund>();
}

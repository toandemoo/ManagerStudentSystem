using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ManagerStudentSystem.Models;

[Table("PaymentTransaction", Schema = "Taekwondo")]
[Index("ProviderId", "ProviderTransactionId", Name = "UQ_ProviderTransaction", IsUnique = true)]
public partial class PaymentTransaction
{
    [Key]
    public int PaymentTransactionId { get; set; }

    public int PaymentId { get; set; }

    public int? PaymentAttemptId { get; set; }

    public int ProviderId { get; set; }

    [StringLength(200)]
    [Unicode(false)]
    public string ProviderTransactionId { get; set; } = null!;

    public byte TransactionType { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Amount { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string Status { get; set; } = null!;

    public DateTime? TransactionAt { get; set; }

    public DateTime CreatedAt { get; set; }

    [ForeignKey("PaymentId")]
    [InverseProperty("PaymentTransactions")]
    public virtual Payment Payment { get; set; } = null!;

    [ForeignKey("PaymentAttemptId")]
    [InverseProperty("PaymentTransactions")]
    public virtual PaymentAttempt? PaymentAttempt { get; set; }

    [ForeignKey("ProviderId")]
    [InverseProperty("PaymentTransactions")]
    public virtual PaymentProvider Provider { get; set; } = null!;
}

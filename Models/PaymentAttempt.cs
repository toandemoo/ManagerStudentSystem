using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ManagerStudentSystem.Models;

[Table("PaymentAttempts", Schema = "Taekwondo")]
[Index("PaymentCode", "AttemptNumber", Name = "UQ_PaymentAttempts", IsUnique = true)]
public partial class PaymentAttempt
{
    [Key]
    public int PaymentAttemptId { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string PaymentCode { get; set; } = null!;

    public int AttemptNumber { get; set; }

    [StringLength(200)]
    [Unicode(false)]
    public string? ProviderTransactionId { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Amount { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string Status { get; set; } = null!;

    [StringLength(2000)]
    public string? PaymentUrl { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? ErrorCode { get; set; }

    [StringLength(1000)]
    public string? ErrorMessage { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? StartedAt { get; set; }

    public DateTime? CompletedAt { get; set; }

    [ForeignKey("PaymentCode")]
    [InverseProperty("PaymentAttempts")]
    public virtual Payment PaymentCodeNavigation { get; set; } = null!;

    [InverseProperty("PaymentAttempt")]
    public virtual ICollection<PaymentTransaction> PaymentTransactions { get; set; } = new List<PaymentTransaction>();
}

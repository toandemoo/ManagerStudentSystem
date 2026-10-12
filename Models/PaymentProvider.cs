using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ManagerStudentSystem.Models;

[Table("PaymentProviders", Schema = "Taekwondo")]
public partial class PaymentProvider
{
    [Key]
    public int PaymentProviderId { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string PaymentProviderCode { get; set; } = null!;

    [StringLength(50)]
    public string PaymentProviderName { get; set; } = null!;

    public bool IsActive { get; set; }

    public DateTime CreateAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    [InverseProperty("Provider")]
    public virtual ICollection<PaymentTransaction> PaymentTransactions { get; set; } = new List<PaymentTransaction>();

    [InverseProperty("Provider")]
    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    [InverseProperty("Provider")]
    public virtual ICollection<WebhookEvent> WebhookEvents { get; set; } = new List<WebhookEvent>();
}

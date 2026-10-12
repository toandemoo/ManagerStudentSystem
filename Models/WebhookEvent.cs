using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ManagerStudentSystem.Models;

[Table("WebhookEvents", Schema = "Taekwondo")]
[Index("ProviderId", "EventId", Name = "UQ_WebhookEvents", IsUnique = true)]
public partial class WebhookEvent
{
    [Key]
    public int WebhookEventId { get; set; }

    public int ProviderId { get; set; }

    [StringLength(200)]
    [Unicode(false)]
    public string EventId { get; set; } = null!;

    [StringLength(100)]
    [Unicode(false)]
    public string? EventType { get; set; }

    [StringLength(200)]
    [Unicode(false)]
    public string? ProviderTransactionId { get; set; }

    public string Payload { get; set; } = null!;

    [StringLength(1000)]
    [Unicode(false)]
    public string? Signature { get; set; }

    public byte Status { get; set; }

    public DateTime ReceivedAt { get; set; }

    public DateTime? ProcessedAt { get; set; }

    [StringLength(1000)]
    public string? ErrorMessage { get; set; }

    [ForeignKey("ProviderId")]
    [InverseProperty("WebhookEvents")]
    public virtual PaymentProvider Provider { get; set; } = null!;
}

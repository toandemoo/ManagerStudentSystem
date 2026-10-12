using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ManagerStudentSystem.Dtos;

public partial class RefundDTO
{
    public int RefundId { get; set; }
    public int PaymentId { get; set; }
    public string RefundCode { get; set; } = null!;
    public decimal Amount { get; set; }
    public string? Reason { get; set; }
    public byte Status { get; set; }
    public string? ProviderRefundId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ManagerStudentSystem.Dtos;

public partial class PaymentDTO
{
    public int PaymentId { get; set; }
    public string PaymentCode { get; set; } = null!;
    public int InvoiceId { get; set; }
    public int ProviderId { get; set; }
    public int PaymentMethodId { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }
    public string Currency { get; set; } = null!;
    public byte Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

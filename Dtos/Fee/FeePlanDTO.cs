using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Humanizer;
using Microsoft.EntityFrameworkCore;

namespace ManagerStudentSystem.Dtos;

public partial class FeePlanDTO
{
    public int FeePlanId { get; set; }
    public string FeePlanName { get; set; } = null!;
    public decimal FeeAmountMonthly { get; set; }
    public decimal DisCountPercentage { get; set; }
    public string? Description { get; set; }
}

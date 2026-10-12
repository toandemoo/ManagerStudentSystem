using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ManagerStudentSystem.Dtos
{
    public class PaymentMethodDTO
    {
        public string PaymentMethodName { get; set; } = null!;
        public bool IsActive { get; set; }
    }
}

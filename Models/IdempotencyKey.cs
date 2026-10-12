using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ManagerStudentSystem.Models;

[Table("IdempotencyKeys", Schema = "Taekwondo")]
[Index("KeyValue", Name = "UQ__Idempote__9371A2E3B22D5D2C", IsUnique = true)]
public partial class IdempotencyKey
{
    [Key]
    public int IdempotencyKeyId { get; set; }

    [StringLength(200)]
    [Unicode(false)]
    public string KeyValue { get; set; } = null!;

    [StringLength(200)]
    [Unicode(false)]
    public string Endpoint { get; set; } = null!;

    [StringLength(200)]
    [Unicode(false)]
    public string RequestHash { get; set; } = null!;

    public int ResponseStatusCode { get; set; }

    public string ResponseBody { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime ExpireAt { get; set; }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ManagerStudentSystem.Dtos;

public partial class LevelDTO
{
    public int LevelId { get; set; }
    public string LevelName { get; set; } = null!;
    public string? Description { get; set; }
}

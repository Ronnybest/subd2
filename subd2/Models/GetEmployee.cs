using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace subd2.Models;

[Keyless]
public partial class GetEmployee
{
    [Column("ID")]
    public int Id { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? FullName { get; set; }

    [StringLength(70)]
    [Unicode(false)]
    public string? Address { get; set; }

    public double? Salary { get; set; }

    [Column("JobTItle")]
    [StringLength(50)]
    [Unicode(false)]
    public string? JobTitle { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? TelephoneNumber { get; set; }
}

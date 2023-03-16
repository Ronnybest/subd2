using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace subd2.Models;

[Table("Budget")]
public partial class Budget
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("Budget", TypeName = "decimal(18, 5)")]
    public decimal? Budget1 { get; set; }

    [Column("Percent", TypeName = "float")]
    public float? Percent { get; set; }

    [Column("PercentForPayment", TypeName = "float")]
    public float? PercentForPayment { get; set; }
    
}

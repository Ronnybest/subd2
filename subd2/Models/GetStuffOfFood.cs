using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace subd2.Models;

[Keyless]
public partial class GetStuffOfFood
{
    [Column("ID")]
    public int Id { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? Dish { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? RawMat { get; set; }

    [Column(TypeName = "decimal(10, 4)")]
    public decimal? Count { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? Name { get; set; }

    [Column(TypeName = "decimal(12, 5)")]
    public decimal? WeHave { get; set; }

    [Column("ID_Raw")]
    public int IdRaw { get; set; }

    [Column("summ", TypeName = "decimal(18, 5)")]
    public decimal? Summ { get; set; }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace subd2.Models;
public partial class HistoryOfProduct
{
    [Key]
    [Column("ID_Counter")]
    public int ID_Counter { get; set; }

    [Column("ID")]
    public int? Id { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string? Product { get; set; }

    [Column(TypeName = "decimal(12, 5)")]
    public decimal? Count { get; set; }

    [Column(TypeName = "decimal(18, 5)")]
    public decimal? Summ { get; set; }

    [Column(TypeName = "date")]
    public DateTime? Date { get; set; }

    [StringLength(30)]
    [Unicode(false)]
    public string? Employee { get; set; }

    [StringLength(30)]
    [Unicode(false)]
    public string? Action { get; set; }

    [Column("IDProduct")]
    public int? Idproduct { get; set; }

  
}

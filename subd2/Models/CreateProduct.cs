using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace subd2.Models;

[Table("Create_product")]
public partial class CreateProduct
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    public int? Product { get; set; }

    [Column(TypeName = "decimal(12, 5)")]
    public decimal? Count { get; set; }

    [Column(TypeName = "date")]
    public DateTime? Date { get; set; }

    public int? Employee { get; set; }

    [ForeignKey("Employee")]
    [InverseProperty("CreateProducts")]
    public virtual Stuff? EmployeeNavigation { get; set; }

    [ForeignKey("Product")]
    [InverseProperty("CreateProducts")]
    public virtual FinishedProduct? ProductNavigation { get; set; }
}

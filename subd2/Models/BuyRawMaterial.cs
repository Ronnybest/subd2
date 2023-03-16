using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace subd2.Models;

[Table("Buy_raw_materials")]
public partial class BuyRawMaterial
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    public int? RawMaterials { get; set; }

    [Column(TypeName = "decimal(12, 5)")]
    public decimal? Count { get; set; }

    [Column(TypeName = "decimal(18, 5)")]
    public decimal? Summ { get; set; }

    [Column(TypeName = "date")]
    public DateTime? Date { get; set; }

    public int? Employee { get; set; }

    [ForeignKey("Employee")]
    [InverseProperty("BuyRawMaterials")]
    public virtual Stuff? EmployeeNavigation { get; set; }

    [ForeignKey("RawMaterials")]
    [InverseProperty("BuyRawMaterials")]
    public virtual RawMaterial? RawMaterialsNavigation { get; set; }
}

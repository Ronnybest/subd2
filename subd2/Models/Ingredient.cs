using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace subd2.Models;

public partial class Ingredient
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    public int? NameProduction { get; set; }

    public int? RawMaterials { get; set; }

    [Column(TypeName = "decimal(10, 4)")]
    public decimal? Count { get; set; }

    [ForeignKey("NameProduction")]
    [InverseProperty("Ingredients")]
    public virtual FinishedProduct? NameProductionNavigation { get; set; }

    [ForeignKey("RawMaterials")]
    [InverseProperty("Ingredients")]
    public virtual RawMaterial? RawMaterialsNavigation { get; set; }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace subd2.Models;

[Table("Raw_materials")]
[Index("Name", Name = "IX_Raw_materials", IsUnique = true)]
public partial class RawMaterial
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? Name { get; set; }

    [Column(TypeName = "decimal(12, 5)")]
    public decimal? Count { get; set; }

    [Column(TypeName = "decimal(18, 5)")]
    public decimal? Sum { get; set; }

    public int? Unit { get; set; }

    [Column(TypeName = "decimal(36, 18)")]
    public decimal? Cost { get; set; }

    [InverseProperty("RawMaterialsNavigation")]
    public virtual ICollection<BuyRawMaterial> BuyRawMaterials { get; } = new List<BuyRawMaterial>();

    [InverseProperty("RawMaterialsNavigation")]
    public virtual ICollection<Ingredient> Ingredients { get; } = new List<Ingredient>();

    [ForeignKey("Unit")]
    [InverseProperty("RawMaterials")]
    public virtual Unit? UnitNavigation { get; set; }
}

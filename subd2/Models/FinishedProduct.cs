using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace subd2.Models;

[Table("Finished_products")]
[Index("Name", Name = "IX_Finished_products", IsUnique = true)]
public partial class FinishedProduct
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? Name { get; set; }

    [Column("UnitID")]
    public int? UnitId { get; set; }

    [Column(TypeName = "decimal(12, 5)")]
    public decimal? Count { get; set; }

    [Column(TypeName = "decimal(18, 5)")]
    public decimal? Sum { get; set; }

    [Column(TypeName = "decimal(36, 18)")]
    public decimal? Cost { get; set; }

    public double? Sebes { get; set; }

    [InverseProperty("ProductNavigation")]
    public virtual ICollection<CreateProduct> CreateProducts { get; } = new List<CreateProduct>();

    [InverseProperty("NameProductionNavigation")]
    public virtual ICollection<Ingredient> Ingredients { get; } = new List<Ingredient>();

    [InverseProperty("ProductNavigation")]
    public virtual ICollection<SellProduct> SellProducts { get; } = new List<SellProduct>();

    [ForeignKey("UnitId")]
    [InverseProperty("FinishedProducts")]
    public virtual Unit? Unit { get; set; }
}

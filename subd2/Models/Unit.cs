using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace subd2.Models;

[Table("Unit")]
public partial class Unit
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? Name { get; set; }

    [InverseProperty("Unit")]
    public virtual ICollection<FinishedProduct> FinishedProducts { get; } = new List<FinishedProduct>();

    [InverseProperty("UnitNavigation")]
    public virtual ICollection<RawMaterial> RawMaterials { get; } = new List<RawMaterial>();
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace subd2.Models;

[Table("Stuff")]
[Index("FullName", Name = "IX_Stuff", IsUnique = true)]
public partial class Stuff
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? FullName { get; set; }

    public int? JobTitle { get; set; }

    public double? Salary { get; set; }

    [StringLength(70)]
    [Unicode(false)]
    public string? Address { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? TelephoneNumber { get; set; }

    public int? BuyRawMatCount { get; set; }
    public int? CreateProductCount { get; set; }
    public int? SellProductCount { get; set; }
    [InverseProperty("EmployeeNavigation")]
    public virtual ICollection<BuyRawMaterial> BuyRawMaterials { get; } = new List<BuyRawMaterial>();

    [InverseProperty("EmployeeNavigation")]
    public virtual ICollection<CreateProduct> CreateProducts { get; } = new List<CreateProduct>();

    [ForeignKey("JobTitle")]
    [InverseProperty("Stuffs")]
    public virtual JobsTitle? JobTitleNavigation { get; set; }

    [InverseProperty("EmployeeNavigation")]
    public virtual ICollection<SellProduct> SellProducts { get; } = new List<SellProduct>();
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace subd2.Models;

[Index("JobTitle", Name = "IX_JobsTitles", IsUnique = true)]
public partial class JobsTitle
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("JobTItle")]
    [StringLength(50)]
    [Unicode(false)]
    public string? JobTitle { get; set; }

    [InverseProperty("JobTitleNavigation")]
    public virtual ICollection<Stuff> Stuffs { get; } = new List<Stuff>();
}

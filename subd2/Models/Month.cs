using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace subd2.Models
{
    [Table("Month")]
    public partial class Month
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }
        public string? Months { get; set; }
        [InverseProperty("MonthNavigation")]
        public virtual ICollection<Payments> Payments { get; } = new List<Payments>();
    }
}

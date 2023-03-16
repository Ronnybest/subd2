using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace subd2.Models
{
    [Table ("Recipe")]
    public partial class Recipe
    {   
        [Key]
        [Column("ID")]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? RawMaterial { get; set; }

        [Column(TypeName = "decimal(10, 4)")]
        [Required(ErrorMessage = "Count is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "Count must be a positive number.")]
        public decimal? Count { get; set; }

        public string? Unit { get; set; }
        public int? UnitId { get; set; }
        public int? RawMatId { get; set; }
        public int? ProductId { get; set; }
    }
}

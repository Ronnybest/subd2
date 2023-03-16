using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace subd2.Models
{
    [Table("Payments")]
    public partial class Payments
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }
        public int? EmployeeID { get; set; }
        public double? Salary { get; set; }
        public int? BuyRawMatCount { get; set; }
        public int? SellProductCount { get; set; }
        public int? CreateProductCount { get; set; }
        public double? AdditionalPercent { get; set; }
        public int? Year { get; set; }
        public int? Month { get; set; }
        public bool? IsPaid{ get; set; }
        public double? Result { get; set; }
    }
}
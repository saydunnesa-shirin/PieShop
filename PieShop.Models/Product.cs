using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PieShop.Models
{
    public class Product : AuditEntity<int, int>
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [ForeignKey("ProductCategoryId")]
        public virtual ProductCategory ProductCategory { get; set; }
        public int ProductCategoryId { get; set; }
        [MaxLength(130)]
        public string ProductName { get; set; }
        [Column(TypeName ="float")]
        public double Price { get; set; }
        [Column(TypeName = "float")]
        public double Qty { get; set; }
    }
}

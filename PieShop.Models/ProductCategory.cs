using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PieShop.Models
{
    public class ProductCategory: AuditEntity<int,int>
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [MaxLength(130)]
        public string CategoryName { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}

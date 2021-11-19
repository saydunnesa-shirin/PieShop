using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PieShop.Models.Auth
{
    public class Permission : AuditEntity<int, int>
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [ForeignKey("UserTypeId")]
        public virtual UserType UserType { get; set; }
        [Required]
        public int UserTypeId { get; set; }
        [ForeignKey("AccessApiId")]
        public virtual AccessApi AccessApi { get; set; }
        [Required]
        public int AccessApiId { get; set; }

    }
}

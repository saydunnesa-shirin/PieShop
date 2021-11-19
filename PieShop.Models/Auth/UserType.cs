using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PieShop.Models.Auth
{
    public class UserType : AuditEntity<int, int>
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [MaxLength(120)]
        public string UserTypeName { get; set; }
        public virtual ICollection<SystemUser> SystemUsers { get; set; }
        public virtual ICollection<Permission> Permissions { get; set; }
    }
}

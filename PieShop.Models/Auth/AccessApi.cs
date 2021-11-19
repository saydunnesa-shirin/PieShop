using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PieShop.Models.Auth
{
    public class AccessApi : AuditEntity<int, int>
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [ForeignKey("SubModuleId")]
        public virtual SubModule SubModule { get; set; }
        [Required]
        public int SubModuleId { get; set; }
        [Required]
        [MaxLength(125)]
        public string Title { get; set; }
        [Required]
        [MaxLength(200)]
        public string URL { get; set; }

        public virtual ICollection<Permission> Permissions { get; set; }
    }
}

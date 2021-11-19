using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PieShop.Models.Auth
{
    public class SubModule : AuditEntity<int, int>
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string SubModuleName { get; set; }
        [ForeignKey("ModuleId")]
        public virtual Module Module { get; set; }
        [Required]
        public int ModuleId { get; set; }
        public string IconURL { get; set; }
        public string URL { get; set; }

        public virtual ICollection<AccessApi> AccessApis { get; set; }
    }
}

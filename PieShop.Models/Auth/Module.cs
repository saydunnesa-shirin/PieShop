using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PieShop.Models.Auth
{
    public class Module : AuditEntity<int, int>
    {
        [Key]
        public int Id { get; set; }
        public string ModuleName { get; set; }
        public string IconURL { get; set; }
        public virtual ICollection<SubModule> SubModules { get; set; }
    }
}

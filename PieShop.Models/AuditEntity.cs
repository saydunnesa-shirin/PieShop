using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PieShop.Models
{
    public class AuditEntity<TCretedBy, TUpdatedBy>
    {
        public TCretedBy CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public TUpdatedBy UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int Status { get; set; }
    }
}

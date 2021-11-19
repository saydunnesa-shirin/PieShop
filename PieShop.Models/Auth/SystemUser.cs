using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PieShop.Models.Auth
{
    public class SystemUser : AuditEntity<int, int>
    {
        [Required]
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string UserName { get; set; }
        [ForeignKey("UserTypeId")]
        public virtual UserType UserType { get; set; }
        [Required]
        public int UserTypeId { get; set; }
        [Required]
        [MaxLength(150)]
        public string FullName { get; set; }
        [MaxLength(70)]
        public string Email { get; set; }
        [MaxLength(30)]
        public string ContactNo { get; set; }
        public string Password { get; set; }
        [MaxLength(200)]
        public string Photo { get; set; }
        [Column(TypeName = "date")]
        public DateTime Birthday { get; set; }
    }
}

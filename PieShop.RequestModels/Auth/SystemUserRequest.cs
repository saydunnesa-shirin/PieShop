using Microsoft.AspNetCore.Http;
using PieShop.Models.Auth;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PieShop.RequestModels.Auth
{
    public class SystemUserRequest:SystemUser
    {
        public string OldPassword { get; set; }
        public IFormFile File { get; set; }
    }
}

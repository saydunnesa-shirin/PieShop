using PieShop.Models.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PieShop.ViewModels.Auth
{
    public class VMSystemUser: SystemUser
    {
        public string UserTypeName { get; set; }
    }
}

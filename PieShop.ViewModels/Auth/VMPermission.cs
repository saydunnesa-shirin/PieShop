using PieShop.Models.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PieShop.ViewModels.Auth
{
    public class VMPermission: Permission
    {
        public int ModuleId { get; set; }
        public string ModuleName { get; set; }
        public IEnumerable<VMSubModule> ModuleDetails { get; set; }
    }

    public class VMSubModule
    {
        public int SubModuleId { get; set; }
        public string SubModuleName { get; set; }
        public IEnumerable<VMAccessApi> SubModuleDetails { get; set; }
    }

    public class VMAccessApi
    {
        public int PermissionId { get; set; }
        public int AccessApiId { get; set; }
        public bool Status { get; set; }
        public string Title { get; set; }
        public string URL { get; set; }
    }
}

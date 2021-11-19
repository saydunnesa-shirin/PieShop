using PieShop.Models.Auth;
using PieShop.RequestModels.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PieShop.Mappers.Auth
{
    public class VmToPermissionMapper : IMapper<PermissionRequest, Permission>
    {
        public Permission Map(PermissionRequest from)
        {
            return new Permission
            {
                Id = from.Id,
                UserTypeId = from.UserTypeId,
                AccessApiId = from.AccessApiId,
                CreatedBy = from.CreatedBy,
                CreatedDate = from.CreatedDate,
                UpdatedBy = from.UpdatedBy,
                UpdatedDate = from.UpdatedDate,
                Status = from.Status
            };
        }
    }

    public class VmToPermissionMapperList : IMapper<List<PermissionRequest>, List<Permission>>
    {
        public List<Permission> Map(List<PermissionRequest> froms)
        {
            List<Permission> list = new List<Permission>();
            foreach (var from in froms)
            {
                list.Add(new Permission
                {
                    Id = from.Id,
                    UserTypeId = from.UserTypeId,
                    AccessApiId = from.AccessApiId,
                    CreatedBy = from.CreatedBy,
                    CreatedDate = from.CreatedDate,
                    UpdatedBy = from.UpdatedBy,
                    UpdatedDate = from.UpdatedDate,
                    Status = from.Status
                });
            }
            return list;
        }
    }
}

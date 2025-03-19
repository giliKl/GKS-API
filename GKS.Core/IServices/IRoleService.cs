using GKS.Core.DTOS;
using GKS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GKS.Core.IServices
{
    public interface IRoleService
    {
        //Get
        public Task<RoleDto> GetRoleByNameAsync(string roleName);
        public Task<IEnumerable<RoleDto>> GetRolesAsync();
        public Task<bool> IsRoleHasPermissionAsync(string roleName, string permission);

        //Post
        public Task<bool> AddPermissionForRoleAsync(string roleName, string permission);
        public Task<bool> AddRoleAsync(RoleDto role);

        //Put
        public Task<bool> UpdateRoleAsync(int id, RoleDto role);

        //Delete
        public Task<bool> DeleteRoleAsync(int id);

    }

}

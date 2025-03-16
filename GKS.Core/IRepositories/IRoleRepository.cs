using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GKS.Core.Entities;


namespace GKS.Core.IRepositories
{
    public interface IRoleRepository
    {
        public Task<bool> IsRoleHasPermissinAsync(string roleName, string permission);
        public Task<IEnumerable<Role>> GetRolesAsync();
        public Task<Role> GetRoleByNameAsync(string roleName);
        public Task<bool> AddPermissionForRoleAsync(string roleName, Permission permission);
        public Task<bool> AddRoleAsync(Role role);
        public Task<bool> UpdateRoleAsync(int id, Role role);
        public Task<bool> DeleteRoleAsync(int id);



    }
}

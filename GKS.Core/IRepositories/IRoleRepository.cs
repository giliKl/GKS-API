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
        //Get
        public Task<IEnumerable<Role>> GetRolesAsync();
        public Task<Role> GetRoleByNameAsync(string roleName);
        public Task<bool> IsRoleHasPermissionAsync(string roleName, string permission);

        //Post
        public Task<bool> AddPermissionForRoleAsync(string roleName, Permission permission);
        public Task<bool> AddRoleAsync(Role role);

        //Put
        public Task<bool> UpdateRoleAsync(int id, Role role);

        //Delete
        public Task<bool> DeleteRoleAsync(int id);



    }
}

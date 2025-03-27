using GKS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GKS.Core.IRepositories
{
    public interface IPermissionRepository
    {
        //Get
        public Task<List<Permission>> GetPermissionsAsync();
        public Task<Permission> GetPermissionByIdAsync(int id);
        public Task<Permission> GetPermissionByNameAsync(string name);

        //Post
        public Task<Permission> AddPermissionAsync(Permission permission);

        //Put
        public Task<Permission> UpdatePermissionAsync(int id, Permission permission);

        //Delete
        public Task<bool> RemovePermissionAsync(int id);
    }
}

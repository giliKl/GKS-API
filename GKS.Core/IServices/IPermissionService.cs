using GKS.Core.DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GKS.Core.IServices
{
    public interface IPermissionService
    {
        //Get
        public Task<List<PermissionDto>> GetPermissionsAsync();
        public Task<PermissionDto> GetPermissionByIdAsync(int id);
        public Task<PermissionDto> GetPermissionByNameAsync(string name);

        //Post
        public Task<PermissionDto> AddPermissionAsync(PermissionDto permission);

        //Put
        public Task<PermissionDto> UpdatePermissionAsync(int id, PermissionDto permission);

        //Delete
        public Task<bool> RemovePermissionAsync(int id);
    }
}

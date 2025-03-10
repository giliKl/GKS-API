using GKS.Core.DTOS;
using GKS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GKS.Core.IRepositories
{
    public interface IUserRepository
    {
        //Get
        public Task<IEnumerable<User>> GetAllUsersAsync();
        public Task<User> GetUserByIdAsync(int id);
        public Task<User> GetUserByEmailAsync(string email);

        //Put
        public Task<User> AddUserAsync(User user);

        //Post
        public Task<User> LogInAsync(string email, string password);
        public Task<bool> UpdatePasswordAsync(int id, string password);
        public Task<bool> UpDateNameAsync(int id, string name);
        public Task<bool> UpdateRoleAsync(int id, Role role);

        //Delete
        public Task<bool> DeleteUserAsync(int id);

    }
}

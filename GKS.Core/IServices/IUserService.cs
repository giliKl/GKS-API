﻿using GKS.Core.DTOS;
using GKS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GKS.Core.IServices
{
    public interface IUserService
    {
        //Get
        public Task<IEnumerable<UserDto>> GetAllUsersAsync();
        public Task<UserDto> GetUserByIdAsync(int id);
        public Task<UserDto> GetUserByEmailAsync(string email);

        //Put
        public Task<UserDto> AddUserAsync(UserDto user);

        //Post
        public Task<UserDto> LogInAsync(string email, string password);
        public Task<bool> UpdatePasswordAsync(int id, string password);
        public Task<bool> UpDateNameAsync(int id, string name);
        public Task<bool> UpdateRoleAsync(int id, Role role);

        //Delete
        public Task<bool> DeleteUserAsync(int id);

    }
}

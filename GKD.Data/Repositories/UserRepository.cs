﻿using GKS.Core.Entities;
using GKS.Core.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GKD.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        readonly IDataContext _dataContext;
        public UserRepository(IDataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<IEnumerable<User>> GetAllUsersAsync() { return await _dataContext._Users.ToListAsync(); }
        public async Task<User> GetUserByEmailAsync(string email) { return await _dataContext._Users.FirstOrDefaultAsync(user => user.Email == email); }
        public async Task<User> GetUserByIdAsync(int id) { return await _dataContext._Users.FirstOrDefaultAsync(user => user.Id == id); }
        public async Task<User> AddUserAsync(User user) { await _dataContext._Users.AddAsync(user); await _dataContext.SaveChangesAsync(); return user; }

    }
}

using GKS.Core.Entities;
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

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            try
            {
                return await _dataContext._Users.ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAllUsersAsync: {ex.Message}");
                return new List<User>(); // מחזיר רשימה ריקה במקרה של שגיאה
            }
        }

        public async Task<User> GetUserByEmailAsync(string email) 
        {
            try
            {
                return await _dataContext._Users.FirstOrDefaultAsync(user => user.Email == email);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetUserByEmailAsync: {ex.Message}");
                return null;
            }
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            try
            {
                return await _dataContext._Users.FirstOrDefaultAsync(user => user.Id == id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetUserByIdAsync: {ex.Message}");
                return null;
            }
        }

        public async Task<User> LogInAsync(string email, string password)
        {
            try
            {
                return await _dataContext._Users.FirstOrDefaultAsync(user => user.Email == email && user.Password == password);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in LogInAsync: {ex.Message}");
                return null;
            }
        }

        public async Task<User> AddUserAsync(User user) 
        {
            try
            {
                await _dataContext._Users.AddAsync(user);
                await _dataContext.SaveChangesAsync();
                return user;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in AddUserAsync: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> UpdateRoleAsync(int id, Role role)
        {
            try
            {
                var user = await _dataContext._Users.FirstOrDefaultAsync(user => user.Id == id);
                if (user == null)
                    return false;
                user.Roles.Add(role);
                await _dataContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<bool> UpdatePasswordAsync(int id, string password)
        {
            try
            {
                var user = await _dataContext._Users.FirstOrDefaultAsync(user => user.Id == id);
                if (user == null) // בודק אם המשתמש נמצא
                {
                    return false;
                }

                user.Password = password; // מעדכן את הסיסמה
                await _dataContext.SaveChangesAsync(); // שומר את השינויים במסד הנתונים

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in UpdatePasswordAsync: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpDateNameAsync(int id, string name)
        {
            try
            {
                var user =await _dataContext._Users.FirstOrDefaultAsync(user => user.Id == id);
                if (user == null)
                {
                    return false;
                }
                user.Name = name;
                await _dataContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in UpdatePasswordAsync: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            try
            {
                var user = await _dataContext._Users.FirstOrDefaultAsync(user => user.Id == id);
                if (user == null)
                {
                    return false;
                }
                user.IsActive = false;
                await _dataContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in DeleteUserAsync: {ex.Message}");
                return false;
            }
        }
    }
}

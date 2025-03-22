using AutoMapper;
using GKS.Core.DTOS;
using GKS.Core.Entities;
using GKS.Core.IRepositories;
using GKS.Core.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GKS.Service.Services
{
    public class UserService : IUserService
    {
        readonly IUserRepository _userRepository;
        readonly IUserActivityRepository _userActivityRepository;
        readonly IRoleRepository _roleRepository;
        readonly IMapper _mapper;

        public UserService(IMapper mapper, IUserRepository userRepository, IRoleRepository roleRepository, IUserActivityRepository userActivityRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _userActivityRepository = userActivityRepository;
            _mapper = mapper;
        }

        //Get
        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllUsersAsync();
            users = users.ToList().Where(user => !user.Roles.Any(role => role.RoleName == "Admin"));
            return _mapper.Map<UserDto[]>(users);
        }

        public async Task<UserDto> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> GetUserByEmailAsync(string email)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            return _mapper.Map<UserDto>(user);
        }


        //Post
        public async Task<UserDto> AddUserAsync(UserDto user, string[] roles)
        {
            var existingUser = await _userRepository.GetUserByEmailAsync(user.Email);

            if (existingUser != null)
            {
                throw new Exception("Email already exists");
            }

            var res = await _userRepository.AddUserAsync(_mapper.Map<User>(user));
            if (res != null)
            {
                for (int i = 0; i < roles.Length; i++)
                {
                    await _userRepository.UpdateRoleAsync(res.Id, await _roleRepository.GetRoleByNameAsync(roles[i]));
                }
                await _userActivityRepository.LogActivityAsync(user.Id, "Register");
            }

            return _mapper.Map<UserDto>(res);
        }

        public async Task<UserDto> LogInAsync(string email, string password)
        {
            var user = await _userRepository.LogInAsync(email, password);
            if (user != null)
            {
                await _userActivityRepository.LogActivityAsync(user.Id, "Login");
            }
            return _mapper.Map<UserDto>(user);
        }


        //Put
        public async Task<bool> EnableUserAsync(int id)
        {
            return await _userRepository.EnableUserAsync(id);
        }

        public async Task<bool> UpDateNameAsync(int id, string name)
        {
            return await _userRepository.UpDateNameAsync(id, name);
        }

        public async Task<bool> UpdatePasswordAsync(int id, string password)
        {
            return await _userRepository.UpdatePasswordAsync(id, password);
        }

        public async Task<bool> UpdateRoleAsync(int id, Role role)
        {
            return await _userRepository.UpdateRoleAsync(id, role);
        }


        //Delete
        public async Task<bool> DeleteUserAsync(int id)
        {
            return await _userRepository.DeleteUserAsync(id);
        }
    }
}

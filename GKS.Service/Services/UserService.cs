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
        readonly IMapper _mapper;

        public UserService(IMapper mapper, IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        //Get
        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllUsersAsync();
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

        //Put
        public async Task<UserDto> AddUserAsync(UserDto user)
        {
            var res = await _userRepository.AddUserAsync(_mapper.Map<User>(user));//המרת המשתנה מ userdto  to user
            return _mapper.Map<UserDto>(res);
        }

        //Delete
        public async Task<bool> DeleteUserAsync(int id)
        {
            return await _userRepository.DeleteUserAsync(id);
        }

        //Post
        public async Task<UserDto> LogInAsync(string email, string password)
        {
            var user = await _userRepository.LogInAsync(email, password);
            return _mapper.Map<UserDto>(user);
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
    }
}

using AutoMapper;
using GKS.Core.DTOS;
using GKS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GKS.Core
{
    public class ProfileMapping:Profile
    {
        public ProfileMapping()
        {
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<UserFile, UserFileDto>().ReverseMap();
            CreateMap<Permission, PermissionDto>().ReverseMap();
            CreateMap<Role, RoleDto>().ReverseMap();
        }
    }
}

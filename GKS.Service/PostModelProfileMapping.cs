using AutoMapper;
using GKS.Core.DTOS;
using GKS.Core.Entities;
using GKS.Service.Post_Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GKS.Service
{
    public class PostModelProfileMapping:Profile
    {
        public PostModelProfileMapping()
        {
            CreateMap<UserPostModel, UserDto>().ReverseMap();
            CreateMap<UserFilePostModel, UserFileDto>().ReverseMap();
        }
    }
}

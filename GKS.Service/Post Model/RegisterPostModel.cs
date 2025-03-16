using GKS.Core.DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GKS.Service.Post_Model
{
    public class RegisterPostModel
    {
        public UserDto User { get; set; }
        public string[] Roles { get; set; }
    }
}

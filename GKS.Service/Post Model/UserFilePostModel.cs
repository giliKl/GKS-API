using GKS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GKS.Service.Post_Model
{
    public class UserFilePostModel
    {
        public int Id { get; set; }
        public int OwnerId { get; set; }
        public string Name { get; set; }
        public string FilePassword { get; set; }
        public bool IsActive { get; set; }

    }
}

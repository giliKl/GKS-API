using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GKS.Core.DTOS
{
    public class UserFileDto
    {
        public int Id { get; set; }
        public int OwnerId { get; set; }
        public string Name { get; set; }
        public string FileLink { get; set; }
        public string EncryptedLink { get; set; }
        public string FilePassword { get; set; }
        public DateOnly CreateedAt { get; set; }
        public bool IsActive { get; set; }

    }
}

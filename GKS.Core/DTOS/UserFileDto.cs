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
        public string Name { get; set; }
        public int OwnerId { get; set; }
        public string EncryptedLink { get; set; }
        public string FilePassword { get; set; }
        public string FileType { get; set; }
        public DateOnly CreatedAt { get; set; }
        public DateOnly UpdateAt { get; set; }
        public bool IsActive { get; set; }

    }
}

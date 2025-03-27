using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GKS.Core.Entities
{
    public class UserFile
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int OwnerId { get; set; }

        [ForeignKey("OwnerId")]
        public User User { get; set; }

        [Required]
        public string Name { get; set; }
        [Required]
        public string FileLink { get; set; }
        [Required]
        public string EncryptedLink { get; set; }
        [Required]
        public string FilePassword { get; set; }
        [Required]
        public string FileType { get; set; }
        [Required]
        public DateOnly CreatedAt { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        [Required]
        public DateOnly UpdateAt { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        public bool IsActive { get; set; } = true;
        public ICollection<string> EmailAloowed { get; set; } = new List<string>();

    }
}

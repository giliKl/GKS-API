using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GKS.Core.Entities
{
    public class Role
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string RoleName { get; set; }

        public string Description { get; set; }

        [Required]
        public DateOnly CreatedAt { get; set; }

        public DateOnly UpdatedAt { get; set; }

        public ICollection<User>? Users { get; set; } = new List<User>();
        public ICollection<Permission>? Permissions { get; set; } = new List<Permission>();

    }
}

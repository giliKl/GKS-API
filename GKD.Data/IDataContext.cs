using GKS.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GKD.Data
{
    public interface IDataContext
    {
        public DbSet<User> _Users { get; set; }
        public DbSet<UserFile> _Files { get; set; }

        Task<int> SaveChangesAsync();
    }
}

using GKS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GKS.Core.IRepositories
{
    public interface IUserFileRepository
    {
        public Task<List<UserFile>> GetAllFilesAsync();
        public Task<UserFile> GetFileByIdAsync(int id);
        public Task<UserFile> GetFileByNameAsync(string name);
        public Task<UserFile[]> GetUserFilesByUserIdAsync(int userId);
        public Task<bool> IsFileNameExistsAsync(int ownerId, string fileName);
        public Task<UserFile> GetFileByUrlAsync(string fileUrl);

        //PUT
        public Task<UserFile> AddFileAsync(UserFile file);
        public Task<bool> updateFileNameAsync(UserFile userFile);


        //DELETE
        public Task<bool> DeleteFileAsync(int id);

    }
}

using GKS.Core.DTOS;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;


namespace GKS.Core.IServices
{
    public interface IFileService
    {
        // GET
        public Task<IEnumerable<UserFileDto>> GetAllUserFilesAsync();
        public Task<UserFileDto> GetUserFileByIdAsync(int id);
        public Task<IEnumerable<UserFileDto>> GetUserFilesByUserIdAsync(int userId);
        public Task<bool> IsFileNameExist(int id, string name);

        // Post
        public Task<string> UploadFileAsync(IFormFile file, string fileName, string password, int userId);
        public Task<FileContentResult> DecryptFileAsync(string encryptedLink, string password);

        // PUT
        public Task<bool> UpdateFileNameAsync(int fileId, string newFileName);

        // DELETE
        public Task<bool> DeleteUserFileAsync(int id);

    }
}

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
        public Task<bool> IsFileNameExistAsync(int id, string name);
        public Task<List<UserFileDto>> GetFileshareByEmailAsync(string email);


        // Post
        public Task<string> UploadFileAsync(IFormFile file, string fileName, string password, int userId);
        public Task<FileContentResult> GetDecryptFileAsync(SharingFileDto decryption);
        public Task<bool> CheckingIsAllowedViewAsync(string email, SharingFileDto sharingFile);


        // PUT
        public Task<bool> UpdateFileNameAsync(int fileId, string newFileName);
        public Task<SharingFileDto> SharingFileAsync(int id, string email);

        // DELETE
        public Task<bool> DeleteUserFileAsync(int id);

    }
}

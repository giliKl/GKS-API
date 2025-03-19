using AutoMapper;
using GKS.Core.DTOS;
using GKS.Core.Entities;
using GKS.Core.IRepositories;
using GKS.Core.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;


namespace GKS.Service.Services
{
    public class UserFileService : IFileService
    {
        private readonly IUserFileRepository _userFileRepository;
        private readonly FileStorageService _fileStorageService;
        private readonly string _encryptionKey;
        readonly IMapper _mapper;

        public UserFileService(IUserFileRepository fileRepository, FileStorageService fileStorageService, IConfiguration configuration, IMapper mapper)
        {
            _userFileRepository = fileRepository;
            _fileStorageService = fileStorageService;
            _mapper = mapper;
            _encryptionKey = configuration["ENCRYPTION_KEY"];

        }

        //Get
        public async Task<IEnumerable<UserFileDto>> GetAllUserFilesAsync()
        {
            var res = await _userFileRepository.GetAllFilesAsync();
            return _mapper.Map<UserFileDto[]>(res);
        }

        public async Task<UserFileDto> GetUserFileByIdAsync(int id)
        {
            var res = await _userFileRepository.GetFileByIdAsync(id);
            return _mapper.Map<UserFileDto>(res);
        }

        public async Task<IEnumerable<UserFileDto>> GetUserFilesByUserIdAsync(int userId)
        {
            var res = await _userFileRepository.GetUserFilesByUserIdAsync(userId);
            return _mapper.Map<UserFileDto[]>(res);
        }

        public async Task<bool> IsFileNameExist(int id, string name)
        {
            return await _userFileRepository.IsFileNameExistsAsync(id, name);
        }


        //Post
        public async Task<FileContentResult> DecryptFileAsync(string encryptedLink, string password)
        {
            // פענוח הקישור כדי לקבל את הנתיב לקובץ ב-S3
            string fileUrl = DecryptLink(encryptedLink, _encryptionKey);

            // חיפוש הקובץ במסד הנתונים
            var userFile = await _userFileRepository.GetFileByUrlAsync(fileUrl);
            if (userFile == null || userFile.FilePassword != password)
            {
                return null; // סיסמה לא נכונה או קובץ לא נמצא
            }

            // הורדת הקובץ המוצפן מ-S3
            var encryptedFileBytes = await _fileStorageService.DownloadFileAsync(fileUrl);
            if (encryptedFileBytes == null)
            {
                return null; // הקובץ לא נמצא ב-S3
            }

            // פענוח הקובץ
            byte[] decryptedFile = DecryptFile(encryptedFileBytes, _encryptionKey);

            return new FileContentResult(decryptedFile, userFile.FileType)
            {
                FileDownloadName = "decrypted_file" // שם ברירת מחדל לקובץ, אפשר להחליף בשם שמגיע ממסד הנתונים
            };

        }

        public async Task<string> UploadFileAsync(IFormFile file, string fileName, string password, int userId)
        {
            string fileType = file.ContentType;
            // הצפנת הקובץ
            byte[] encryptedData = EncryptFile(file, _encryptionKey, userId, fileName);

            // העלאה ל-S3
            // יצירת קישור ציבורי ל-S3
            string fileUrl = await _fileStorageService.UploadFileAsync(file,fileName,encryptedData);
            if (fileUrl == null) {
                return null;
            }
            // הצפנת הקישור
            string encryptedLink = EncryptLink(fileUrl, _encryptionKey);
            
            // שמירה במסד הנתונים
            await _userFileRepository.AddFileAsync(new UserFile
            {
                OwnerId = userId,
                Name = fileName,
                FileLink = fileUrl,
                EncryptedLink = encryptedLink,
                FilePassword = password,
                FileType = fileType
            });

            return encryptedLink;
        }


        //Put
        public async Task<bool> UpdateFileNameAsync(int fileId, string newFileName)
        {
            var userFile = await _userFileRepository.GetFileByIdAsync(fileId);
            if (userFile == null)
            {
                return false;
            }
            string oldFilePath = userFile.Name;
            string newFilePath = $"{newFileName}";
            try
            {
                var newLink = await _fileStorageService.UpdateFileNameAsync(oldFilePath, newFilePath);
                if (newLink == null)
                {
                    return false;
                }
                userFile.FileLink = newLink;
                userFile.EncryptedLink = EncryptLink(userFile.FileLink, _encryptionKey);
                userFile.Name = newFileName;
                return await _userFileRepository.updateFileNameAsync(userFile);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating file name in S3: {ex.Message}");
                return false;
            }
        }


        //Delete
        public async Task<bool> DeleteUserFileAsync(int id)
        {
            try
            {
                var userFile = await _userFileRepository.GetFileByIdAsync(id);
                if (userFile == null)
                {
                    return false;
                }
                //הוצאת המפתח של הקובץ מהקישור
                var fileKey = userFile.FileLink.Contains("s3.amazonaws.com") ?
                 userFile.FileLink.Split(new[] { ".s3.amazonaws.com/" }, StringSplitOptions.None).Last() :
                 userFile.FileLink;

                if (!await _fileStorageService.DeleteFileAsync(fileKey))
                {
                    return false;
                }

                return await _userFileRepository.DeleteFileAsync(id);
            }
            catch (Exception ex)
            {
                return false;
            }
        }



        private byte[] EncryptFile(IFormFile file, string key, int userId, string fileName)
        {
            using (var memoryStream = new MemoryStream())
            {
                file.CopyTo(memoryStream);
                byte[] fileBytes = memoryStream.ToArray();

                using (var aes = Aes.Create())
                {
                    aes.Key = Encoding.UTF8.GetBytes(key);
                    aes.IV = new byte[16]; // ניתן לשפר עם IV דינמי

                    using (var encryptor = aes.CreateEncryptor())
                    {
                        return encryptor.TransformFinalBlock(fileBytes, 0, fileBytes.Length);
                    }
                }
            }
        }
        private byte[] DecryptFile(byte[] encryptedData, string key)
        {
            using (var aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = new byte[16]; // אותו IV ששימש בהצפנה

                using (var decryptor = aes.CreateDecryptor())
                {
                    return decryptor.TransformFinalBlock(encryptedData, 0, encryptedData.Length);
                }
            }
        }

        private string EncryptLink(string data, string key)
        {
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);
            using (var aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = new byte[16];

                using (var encryptor = aes.CreateEncryptor())
                {
                    byte[] encryptedBytes = encryptor.TransformFinalBlock(dataBytes, 0, dataBytes.Length);
                    return Convert.ToBase64String(encryptedBytes);
                }
            }
        }

        private string DecryptLink(string encryptedLink, string key)
        {
            byte[] encryptedBytes = Convert.FromBase64String(encryptedLink);

            using (var aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = new byte[16]; // אותו IV ששימש בהצפנה

                using (var decryptor = aes.CreateDecryptor())
                {
                    byte[] decryptedBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
                    return Encoding.UTF8.GetString(decryptedBytes);
                }
            }
        }


    }
}

using AutoMapper;
using GKD.Data.Migrations;
using GKS.Core.DTOS;
using GKS.Core.Entities;
using GKS.Core.IRepositories;
using GKS.Core.IServices;
using GKS.Service.Post_Model;
using GKS.Service.Services;
using Microsoft.AspNetCore.Mvc;
using Sprache;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GKS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IFileService _fileService;
        readonly IMapper _mapper;
        public FileController(IFileService fileService, IMapper mapper)
        {
            _fileService = fileService;
            _mapper = mapper;
        }

        // GET
        [HttpGet]
        public async Task<IActionResult> GetAllUserFiles()
        {
            var files = await _fileService.GetAllUserFilesAsync();
            return Ok(files);
        }

        [HttpGet("user/{id}")]
        public async Task<ActionResult<UserFileDto[]>> GetUserFilesByUserId(int id)
        {

            if (id < 0)
                return BadRequest();
            var userFiles = await _fileService.GetUserFilesByUserIdAsync(id);
            if (userFiles == null)
            {
                return NotFound($"User with ID {id} not found.");
            }
            return Ok(userFiles);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFileById(int id)
        {
            var file = await _fileService.GetUserFileByIdAsync(id);
            if (file == null)
                return NotFound("File not found.");

            return Ok(file);
        }

        //Post
        [HttpPost("IsFile/{id}")]
        public async Task<ActionResult> IsFileExist(int id, [FromBody] string name)
        {
            var result = await _fileService.IsFileNameExist(id, name);
            return Ok(result);
        }

        // POST 

        [HttpPost("upload/{id}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadFile(int id, [FromForm] UploadFileDto request)
        {
            if (request.File == null || request.File.Length == 0)
                return BadRequest("File is required.");

            var userId = id; // לממש בהתאם
            var result = await _fileService.UploadFileAsync(request.File, request.FileName, request.Password, userId);
            return Ok(new { encryptedLink = result });
        }


        [HttpPost("decrypt-file")]
        public async Task<IActionResult> DecryptFile([FromBody] DecryptionPostModel request)
        {
            var result = await _fileService.DecryptFileAsync(request.EncryptedLink, request.Password);
            if (result == null)
            {
                return Unauthorized("Invalid password or file not found.");
            }

            // החזרת הקובץ להורדה
            return result;

        }


        // PUT 
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFileName(int id, [FromBody] string newFileName)
        {
            var result = await _fileService.UpdateFileNameAsync(id, newFileName);
            if (!result)
                return BadRequest("Failed to update file name.");

            return Ok(result);
        }


        // DELETE 
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFile(int id)
        {
            var result = await _fileService.DeleteUserFileAsync(id);
            if (!result)
                return NotFound("File not found.");

            return Ok(result);
        }
    }
}

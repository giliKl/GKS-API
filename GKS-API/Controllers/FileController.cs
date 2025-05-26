using GKS.Core.DTOS;
using GKS.Core.IServices;
using GKS.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace GKS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IFileService _fileService;

        public FileController(IFileService fileService)
        {
            _fileService = fileService;
        }

        // GET
        [HttpGet]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> GetAllUserFilesAsync()
        {
            var files = await _fileService.GetAllUserFilesAsync();
            return Ok(files);
        }

        [HttpGet("user/{id}")]
        [Authorize(Policy = "UserOnly")]
        public async Task<ActionResult<UserFileDto[]>> GetUserFilesByUserIdAsync(int id)
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
        [Authorize(Policy = "UserOrAdmin")]
        public async Task<IActionResult> GetFileByIdAsync(int id)
        {
            var file = await _fileService.GetUserFileByIdAsync(id);
            if (file == null)
                return NotFound("File not found.");

            return Ok(file);
        }

        [HttpGet("filesShared/{email}")]
        [Authorize(Policy = "UserOnly")]
        public async Task<ActionResult> GetFileshareByEmailAsync(string email)
        {
            var file = await _fileService.GetFileshareByEmailAsync(email);
            return Ok(file);
        }

        //Post
        [HttpPost("IsFile/{id}")]
        [Authorize(Policy = "UserOrAdmin")]
        public async Task<ActionResult> IsFileExistAsync(int id, [FromBody] string name)
        {
            var result = await _fileService.IsFileNameExistAsync(id, name);
            return Ok(result);
        }

        [HttpPost("CheckingIsAllowedView/{email}")]
        [Authorize(Policy = "UserOrAdmin")]
        public async Task<ActionResult> CheckingIsAllowedViewAsync(string email, [FromBody] SharingFileDto sharingFileDto)
        {
            var file = await _fileService.GetUserFileByIdAsync(sharingFileDto.Id);
            if (file == null)
                return NotFound("File not found.");

            bool isAllowed = await _fileService.CheckingIsAllowedViewAsync(email, sharingFileDto);
            if (!isAllowed)
                return Unauthorized("You are not allowed to view this file.");

            sharingFileDto.Password = file.FilePassword;

            var result = await _fileService.GetDecryptFileAsync(sharingFileDto);
            if (result == null)
                return NotFound("File not found.");

            return File(result.FileContents, result.ContentType, result.FileDownloadName);
        }


        [HttpPost("upload/{id}")]
        [Consumes("multipart/form-data")]
        [Authorize(Policy = "UserOnly")]
        public async Task<IActionResult> UploadFileAsync(int id, [FromForm] UploadFileDto request)
        {
            if (request.File == null || request.File.Length == 0)
                return BadRequest("File is required.");

            var userId = id; // לממש בהתאם
            var result = await _fileService.UploadFileAsync(request.File, request.FileName, request.Password, userId);
            return Ok(new { encryptedLink = result });
        }


        [HttpPost("decrypt-file")]
        [Authorize(Policy = "UserOnly")]
        public async Task<IActionResult> DecryptFileAsync([FromBody] SharingFileDto request)
        {
            var result = await _fileService.GetDecryptFileAsync(request);
            if (result == null)
            {
                return Unauthorized("Invalid password or file not found.");
            }

            // החזרת הקובץ להורדה
            return result;

        }


        // PUT 
        [HttpPut("Sharing/{id}")]
        [Authorize(Policy = "UserOnly")]
        public async Task<ActionResult> SharingFileAsync(int id, [FromBody] string email)
        {
            var result = await _fileService.SharingFileAsync(id, email);
            if (result == null)
                return NotFound("File not found.");
            return Ok(result);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "UserOnly")]
        public async Task<IActionResult> UpdateFileNameAsync(int id, [FromBody] string newFileName)
        {
            var result = await _fileService.UpdateFileNameAsync(id, newFileName);
            if (!result)
                return BadRequest("Failed to update file name.");

            return Ok(result);
        }


        // DELETE 
        [HttpDelete("{id}")]
        [Authorize(Policy = "UserOrAdmin")]
        public async Task<IActionResult> DeleteFileAsync(int id)
        {
            var result = await _fileService.DeleteUserFileAsync(id);
            if (!result)
                return NotFound("File not found.");

            return Ok(result);
        }
    }
}

using AutoMapper;
using GKS.Core.DTOS;
using GKS.Core.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GKS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        readonly IUserService _userService;
        readonly IMapper _mapper;
        public UserController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        // GET
        [HttpGet]
        //[Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUsersAsync()
        {
            try
            {
                var users = await _userService.GetAllUsersAsync();

                if (users == null || !users.Any())
                {
                    return NotFound("No users found.");
                }
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUserByIdAsync(int id)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(id);

                if (user == null)
                {
                    return Unauthorized("Invalid credentials.");
                }
                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("email")]
        public async Task<ActionResult<UserDto>> GetUserByEmailAsync([FromQuery] string email)
        {
            try
            {
                var user = await _userService.GetUserByEmailAsync(email);

                if (user == null)
                {
                    return Unauthorized("Invalid credentials.");
                }
                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        //PUT 
        [HttpPut("{id}/enableuser")]
        public async Task<ActionResult> EnableUserAsync(int id)
        {
            try
            {
                var enabled = await _userService.EnableUserAsync(id);
                if (!enabled)
                {
                    return NotFound("User not found.");
                }
                return NoContent(); // Success with no content to return
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}/name")]
        public async Task<ActionResult> UpDateNameAsync(int id, [FromBody] string name)
        {
            try
            {
                var updated = await _userService.UpDateNameAsync(id, name);
                if (!updated)
                {
                    return NotFound("User not found.");
                }
                return NoContent(); // Success with no content to return
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}/password")]
        public async Task<ActionResult> UpdatePasswordAsync(int id, [FromBody] string password)
        {
            try
            {
                var updated = await _userService.UpdatePasswordAsync(id, password);
                if (!updated)
                {
                    return NotFound("User not found.");
                }
                return NoContent(); // Success with no content to return
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }



        //DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        //[Authorize(Policy = "EditorOrAdmin")]
        public async Task<ActionResult> DeleteUserAsync(int id)
        {
            try
            {
                var deleted = await _userService.DeleteUserAsync(id);
                if (!deleted)
                {
                    return NotFound("User not found.");
                }
                return NoContent(); 
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}

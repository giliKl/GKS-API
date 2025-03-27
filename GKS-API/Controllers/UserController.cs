using GKS.Core.DTOS;
using GKS.Core.Entities;
using GKS.Core.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace GKS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;

        }

        // GET
        [HttpGet]
        [Authorize(Policy = "AdminOnly")]
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
        [Authorize(Policy = "UserOrAdmin")]
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
        [Authorize(Policy = "UserOrAdmin")]
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
        [HttpPut("enable/{id}")]
        [Authorize(Policy = "AdminOnly")]
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

        [HttpPut("disable/{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult<bool>> DisableUserAsync(int id)
        {
            var res = await _userService.DisableUserAsync(id);
            if (!res)
                return NotFound();
            return Ok(res);
        }

        [HttpPut("{id}/name")]
        [Authorize(Policy = "UserOrAdmin")]
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
        [Authorize(Policy = "UserOrAdmin")]
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

        [HttpPut("{id}/updaterole")]
        public async Task<bool> UpdateRoleAsync(int id,[FromBody] RoleDto role)
        {
            return await _userService.UpdateRoleAsync(id, role);
        }




        //DELETE 
        [HttpDelete("{id}")]
        [Authorize(Policy = "UserOnly")]
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

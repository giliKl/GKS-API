using AutoMapper;
using GKS.Core.IServices;
using GKS.Service;
using GKS.Service.Post_Model;
using GKS.Service.Shared;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;


namespace GKS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAuthService _authService;

        public AuthController(IUserService userService, IAuthService authService)
        {
            _userService = userService;
            _authService = authService;
        }

        // 🔹 פונקציית התחברות
        [HttpPost("login")]
        public async Task<ActionResult> LoginAsync([FromBody] LoginModel loginModel)
        {
            if (!EmailValidator.IsValidEmail(loginModel.Email))
                return BadRequest(new { success = false, error = "Email Not valid" });

            var user = await _userService.GetUserByEmailAsync(loginModel.Email);
            if (user == null)
                return Unauthorized(new { success = false, error = "Invalid credentials" });

            if (!user.IsActive)
                return Unauthorized(new { success = false, error = "User is not active" });

            var token = _authService.GenerateJwtToken(user.Name, loginModel.Roles);

            return Ok(new
            {
                success = true,
                Token = token,
                User= user 
            });
        }

        // 🔹 פונקציית רישום משתמש חדש
        [HttpPost("register")]
        public async Task<ActionResult> RegisterAsync([FromBody] RegisterPostModel userModel)
        {
            try
            {
                var user = await _userService.AddUserAsync(userModel.User, userModel.Roles);
                if (user == null)
                    return BadRequest(new { error = "User registration failed" });

                var token = _authService.GenerateJwtToken(user.Name, userModel.Roles);
                return Ok(new { Token = token, User = user });
            }
            catch (EmailAlreadyExistsException ex)
            {
                return Conflict(new { error = $"Email '{userModel.User.Email}' is already in use" });
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An unexpected error occurred", detail = ex.Message });
            }
        }

        // 🔹 פונקציה לרענון טוקן אם הוא פג תוקף
        [HttpPost("refresh-token")]
        public ActionResult RefreshToken([FromBody] string token)
        {
            var newToken = _authService.RefreshToken(token);
            if (newToken == null)
                return Unauthorized("Token is still valid or invalid");

            return Ok(new { Token = newToken });
        }


    }

}


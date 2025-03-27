using AutoMapper;
using GKS.Core.IServices;
using GKS.Service;
using GKS.Service.Post_Model;
using Microsoft.AspNetCore.Mvc;


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
                return BadRequest("Email Not valid");

            var user = await _userService.LogInAsync(loginModel.Email, loginModel.Password);
            if (user == null)
                return NotFound("User not found");

            if (!user.IsActive)
                return Unauthorized("User is not active");

            var token = _authService.GenerateJwtToken(user.Name, loginModel.Roles);
            return Ok(new { Token = token, User = user });
        }

        // 🔹 פונקציית רישום משתמש חדש
        [HttpPost("register")]
        public async Task<ActionResult> RegisterAsync([FromBody] RegisterPostModel userModel)
        {
            var user = await _userService.AddUserAsync(userModel.User, userModel.Roles);
            if (user == null)
                return BadRequest("User registration failed");

            var token = _authService.GenerateJwtToken(user.Name, userModel.Roles);
            return Ok(new { Token = token, User = user });
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


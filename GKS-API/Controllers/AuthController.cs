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
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;
        public AuthController(IConfiguration configuration, IUserService userService, IAuthService authService)
        {
            _configuration = configuration;
            _userService = userService;
            _authService = authService;
        }


        [HttpPost]
        public async Task<ActionResult> Login([FromBody] LoginModel loginModel)
        {
            if (!EmailValidator.IsValidEmail(loginModel.Email))
            {
                return BadRequest("Email Not valid");
            }
            var res = await _userService.LogInAsync(loginModel.Email, loginModel.Password);
            if (res == null)
                return NotFound();
            if(res.IsActive==false)
                return Unauthorized();

            var token = _authService.GenerateJwtToken(res.Name, loginModel.Roles);
                return Ok(new { Token = token, user = res });
            
        }


        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] RegisterPostModel user)
        {
            var res = await _userService.AddUserAsync(user.User, user.Roles);
            if (res == null)
                return BadRequest();
            
                var token = _authService.GenerateJwtToken(res.Name, user.Roles);
                return Ok(new { Token = token, user = res });
          
        }


    }

}


using AutoMapper;
using GKS.Core.DTOS;
using GKS.Core.Entities;
using GKS.Core.IServices;
using GKS.Service;
using GKS.Service.Post_Model;
using GKS.Service.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

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
        public AuthController(IConfiguration configuration, IUserService userService, IMapper mapper, IAuthService authService)
        {
            _configuration = configuration;
            _userService = userService;
            _authService = authService;
            _mapper = mapper;
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
            var res = await _userService.AddUserAsync(_mapper.Map<UserDto>(user));
            if (res == null)
                return BadRequest();
            
                var token = _authService.GenerateJwtToken(res.Name, user.Roles);
                return Ok(new { Token = token, user = res });
          
        }


    }

}


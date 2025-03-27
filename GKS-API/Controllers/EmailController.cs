using GKS.Core.Entities;
using GKS.Core.IServices;
using Microsoft.AspNetCore.Mvc;


namespace GKS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;

        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }


        [HttpPost("send")]
        public async Task<IActionResult> SendEmailAsync([FromBody] EmailRequest request)
        {
            await _emailService.SendEmailAsync(request);
            return Ok();
        }
    }
}

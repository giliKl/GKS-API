using GKS.Core.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace GKS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserActivityController : ControllerBase
    {
        private readonly IUserActivityService _service;

        public UserActivityController(IUserActivityService service)
        {
            _service = service;
        }

        [HttpGet("user-monthly-usage/{Id}")]
        [Authorize(Policy = "UserOnly")]
        public async Task<IActionResult> GetUserMonthlyUsageAsync( int Id, [FromQuery] int year, [FromQuery] int month)
        {
            var result = await _service.GetUserMonthlyUsageAsync(Id, year, month);
            return Ok(result);
        }

        [HttpGet("user-yearly-usage/{Id}")]
        [Authorize(Policy = "UserOnly")]
        public async Task<IActionResult> GetUserYearlyUsageAsync( int Id, [FromQuery] int year)
        {
            var result = await _service.GetUserYearlyUsageAsync(Id, year);
            return Ok(result);
        }

        [HttpGet("yearly-usage")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> GetYearlyUsageAsync([FromQuery] int year)
        {
            var result = await _service.GetYearlyUsageAsync(year);
            return Ok(result);
        }

        [HttpGet("monthly-usage")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult<Dictionary<int, int>>> GetMonthlyUsageAsync([FromQuery] int year, [FromQuery] int month)
        {
            var result = await _service.GetMonthlyUsageAsync(year, month);
            return Ok(result);
        }

        
    }
}

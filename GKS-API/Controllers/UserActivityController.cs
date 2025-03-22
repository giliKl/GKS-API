﻿using GKS.Core.IServices;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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
        public async Task<IActionResult> GetUserMonthlyUsage( int Id, [FromQuery] int year, [FromQuery] int month)
        {
            var result = await _service.GetUserMonthlyUsageAsync(Id, year, month);
            return Ok(result);
        }

        [HttpGet("user-yearly-usage/{Id}")]
        public async Task<IActionResult> GetUserYearlyUsage( int Id, [FromQuery] int year)
        {
            var result = await _service.GetUserYearlyUsageAsync(Id, year);
            return Ok(result);
        }

        [HttpGet("yearly-usage")]
        public async Task<IActionResult> GetYearlyUsage([FromQuery] int year)
        {
            var result = await _service.GetYearlyUsageAsync(year);
            return Ok(result);
        }

        [HttpGet("monthly-usage")]
        public async Task<ActionResult<Dictionary<int, int>>> GetMonthlyUsage([FromQuery] int year, [FromQuery] int month)
        {
            var result = await _service.GetMonthlyUsageAsync(year, month);
            return Ok(result);
        }

        
    }
}

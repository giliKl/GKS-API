using GKS.Core.DTOS;
using GKS.Core.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace GKS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionController : ControllerBase
    {
        readonly IPermissionService _permissionService;

        public PermissionController(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }


        // GET
        [HttpGet]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult> GetAllPermissionAsync()
        {
            return Ok(await _permissionService.GetPermissionsAsync());
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult> GetPermissionByIdAsync(int id)
        {
            var res = await _permissionService.GetPermissionByIdAsync(id);
            if (res == null) return NotFound();
            return Ok(res);
        }

        [HttpGet("/name/{name}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult> GetByNameAsync(string name)
        {
            var res = await _permissionService.GetPermissionByNameAsync(name);
            if (res == null) return NotFound();
            return Ok(res);
        }


        // POST 
        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult> AddPermissionAsync([FromBody] PermissionDto permission)
        {
            var res = await _permissionService.AddPermissionAsync(permission);
            if (res == null) return BadRequest();
            return Ok(res);
        }


        // PUT 
        [HttpPut("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult> UpdatePermissionAsync(int id, [FromBody] PermissionDto permission)
        {
            var res = await _permissionService.UpdatePermissionAsync(id, permission);
            if (res == null) return BadRequest();
            return Ok(res);
        }


        // DELETE 
        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult> DeletePermissionAsync(int id)
        {
            var res = await _permissionService.RemovePermissionAsync(id);
            if (!res) return NotFound();
            return Ok();
        }

    }
}

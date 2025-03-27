using GKS.Core.DTOS;
using GKS.Core.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;



namespace GKS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        //Get
        [HttpGet]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult> GetRolesAsync()
        {
            return Ok(await _roleService.GetRolesAsync());
        }

        [HttpGet("{roleName}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult> GetRoleByNameAsync(string roleName)
        {
            return Ok(await _roleService.GetRoleByNameAsync(roleName));
        }

        [HttpGet("{roleName}/Ispermission")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult> GetRoleHasPermissionAsync(string roleName, [FromQuery] string permission)
        {
            return Ok(await _roleService.IsRoleHasPermissionAsync(roleName, permission));
        }


        //Post
        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult> AddRoleAsync([FromBody] RoleDto role)
        {
            return Ok(await _roleService.AddRoleAsync(role));
        }

        [HttpPost("addPermission/{roleName}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult> AddPermissionForRoleAsync(string roleName, [FromBody] string permission)
        {
            return Ok(await _roleService.AddPermissionForRoleAsync(roleName, permission));
        }


        //Put
        [HttpPut("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult> UpdateRoleAsync(int id, [FromBody] RoleDto role)
        {
            return Ok(await _roleService.UpdateRoleAsync(id, role));
        }

        //Delete
        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult> DeleteRoleAsync(int id)
        {
            return Ok(await _roleService.DeleteRoleAsync(id));
        }
    }
}

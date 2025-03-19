using AutoMapper;
using GKS.Core.DTOS;
using GKS.Core.Entities;
using GKS.Core.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;


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
        public async Task<ActionResult> GetRolesAsync()
        {
            return Ok(await _roleService.GetRolesAsync());
        }

        [HttpGet("{roleName}")]
        public async Task<ActionResult> GetRoleByNameAsync(string roleName)
        {
            return Ok(await _roleService.GetRoleByNameAsync(roleName));
        }

        [HttpGet("{roleName}/Ispermission")]
        public async Task<ActionResult> GetRoleHasPermissoinAsync(string roleName, [FromQuery] string permission)
        {
            return Ok(await _roleService.IsRoleHasPermissionAsync(roleName, permission));
        }


        //Post
        [HttpPost]
        public async Task<ActionResult> AddRoleAsync([FromBody] RoleDto role)
        {
            return Ok(await _roleService.AddRoleAsync(role));
        }

        [HttpPost("addPermission/{roleName}")]
        public async Task<ActionResult> AddPermissionForRoleAsync(string roleName, [FromBody] string permission)
        {
            return Ok(await _roleService.AddPermissionForRoleAsync(roleName, permission));
        }


        //Put
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateRoleAsync(int id, [FromBody] RoleDto role)
        {
            return Ok(await _roleService.UpdateRoleAsync(id, role));
        }

        //Delete
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteRoleAsync(int id)
        {
            return Ok(await _roleService.DeleteRoleAsync(id));
        }
    }
}

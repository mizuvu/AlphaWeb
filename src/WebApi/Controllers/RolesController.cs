using Application.Contracts.Authorization;
using Application.Contracts.Identity;
using Application.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[Authorize(Policy = Permissions.Roles.View)]
public class RolesController : ControllerBase
{
    private readonly IRoleService _roleService;

    public RolesController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAsync(CancellationToken cancellationToken)
    {
        return Ok(await _roleService.GetAllAsync(cancellationToken));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync([FromRoute] string id)
    {
        return Ok(await _roleService.GetByIdAsync(id));
    }

    [HttpPost]
    [Authorize(Policy = Permissions.Roles.Create)]
    public async Task<IActionResult> CreateAsync([FromBody] RoleDto request)
    {
        return Ok(await _roleService.CreateAsync(request));
    }

    [HttpPut("{id}")]
    [Authorize(Policy = Permissions.Roles.Update)]
    public async Task<IActionResult> UpdateAsync([FromRoute] string id, [FromBody] RoleDto request)
    {
        if (id != request.Id)
            return BadRequest(id);

        return Ok(await _roleService.UpdateAsync(request));
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = Permissions.Roles.Delete)]
    public async Task<IActionResult> DeleteAsync([FromRoute] string id)
    {
        return Ok(await _roleService.DeleteAsync(id));
    }
}
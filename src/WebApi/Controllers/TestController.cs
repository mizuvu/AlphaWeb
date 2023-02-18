/*
using Application.Features.Notifications.Queries;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[AllowAnonymous]
public class TestController : ApiControllerBase
{
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync(string id)
    {
        var data = await Mediator.Send(new GetNotificationById(id));
        return Ok(data);
    }
}
*/
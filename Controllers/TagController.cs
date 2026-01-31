using bookmark_manager_app.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace bookmark_manager_app.Controllers;

[ApiController]
[Authorize]
[Route("/api/tags")]
public class TagController(TagService tagService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IDictionary<string, int>>> GetTagUsageCountsAsync()
    {
        return Ok(await tagService.GetTagUsageCountsAsync());
    }
}
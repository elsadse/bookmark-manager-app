using bookmark_manager_app.DTOs;
using bookmark_manager_app.Exceptions;
using bookmark_manager_app.Interfaces;
using bookmark_manager_app.Models;
using Microsoft.AspNetCore.Mvc;

namespace bookmark_manager_app.Controllers;

[ApiController]
[Route("api/tags")]
public class TagsController : ControllerBase
{
    private readonly IBookmarkService _bookmarkService;
    private readonly ILogger<UsersController> _logger;

    public TagsController(IBookmarkService bookmarkService, ILogger<UsersController> logger)
    {
        _bookmarkService = bookmarkService;
        _logger = logger;
    }

    [HttpPost()]
    public async Task<ActionResult<Tag>> AddTag(string name)
    {
        if (name is not string || name.Length > 25)
            throw new BadRequestException("name must be a string and length of name must be under 25 characters");
        var tag = await _bookmarkService.AddTagToBookmarkAsync(name);
        return CreatedAtAction(
            nameof(AddTag),
            new { Name = tag.Name },
            tag);

    }

}

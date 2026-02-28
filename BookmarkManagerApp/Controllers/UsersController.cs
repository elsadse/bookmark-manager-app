using BookmarkManagerApp.Controllers.Responses;
using BookmarkManagerApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookmarkManagerApp.Controllers;

[ApiController]
[Route("/api/users")]
public class UserController(UserService userService) : ControllerBase
{
    [HttpGet("{id:long}", Name = nameof(GetUserByIdAsync))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetUserByIdResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<ActionResult<GetUserByIdResponse>> GetUserByIdAsync(long id)
    {
        var user = await userService.GetUserByIdAsync(id);
        return Ok(GetUserByIdResponse.FromModel(user));
    }
}
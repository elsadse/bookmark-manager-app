using bookmark_manager_app.DTOs;
using bookmark_manager_app.Exceptions;
using bookmark_manager_app.Interfaces;
using bookmark_manager_app.Models;
using Microsoft.AspNetCore.Mvc;

namespace bookmark_manager_app.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILogger<UsersController> _logger;

    public UsersController(IUserService userService, ILogger<UsersController> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUserById(int id)
    {
        if (id <= 0)
            throw new BadRequestException("User ID must be positive");
        var user = await _userService.GetUserByIdAsync(id);
        if (user == null) throw new NotFoundException("User ID not found");
        return Ok(user);
    }

    [HttpPost]
    public async Task<ActionResult<UserDto>> CreateUser([FromBody] UserCreateDto command)
    {

        if (!ModelState.IsValid)
        {
            var errors = ModelState
            .Where(e => e.Value?.Errors.Count > 0)
            .ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
            );
            throw new ValidationException(errors);
        }
        var user = await _userService.CreateUserAsync(command);
        return CreatedAtAction(nameof(GetUserById), new { id = user.UserId }, user);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] UserUpdateDto command)
    {

        if (!ModelState.IsValid)
        {
            var errors = ModelState
            .Where(e => e.Value?.Errors.Count > 0)
            .ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
            );
            throw new ValidationException(errors);
        }
        if (id <= 0)
            throw new BadRequestException("User ID must be positive");

        await _userService.UpdateUserAsync(id, command);
        return NoContent();

    }
}
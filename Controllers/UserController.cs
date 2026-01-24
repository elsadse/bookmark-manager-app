using bookmark_manager_app.Models;
using bookmark_manager_app.Services;
using Microsoft.AspNetCore.Mvc;

namespace bookmark_manager_app.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILogger<UserController> _logger;

    public UserController(IUserService userService, ILogger<UserController> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUserById(int id)
    {
        try
        {
            _logger.LogInformation("Getting user with ID: {UserId}", id);
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                _logger.LogWarning("User with ID {UserId} not found", id);
                return NotFound(new ApiResponse
                {
                    Success = false,
                    Error = $"User with ID {id} not found"
                });
            }

            return user;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting user with ID: {UserId}", id);
            return StatusCode(StatusCodes.Status500InternalServerError,
                new ApiResponse
                {
                    Success = false,
                    Error = "Internal server error"
                });
        }
    }

    [HttpPost]
    public async Task<ActionResult<User>> CreateUser(UserCreateDto userDto)
    {
        try
        {
            _logger.LogInformation("Creating new user with email: {Email}", userDto.Email);
            var user = await _userService.CreateUserAsync(userDto);
            if (user == null)
            {
                _logger.LogWarning("Failed to create user with email: {Email} - Email already exists", userDto.Email);
                return Conflict(new ApiResponse
                {
                    Success = false,
                    Error = "Email already exists"
                });
            }
            _logger.LogInformation("User created successfully with ID: {UserId}", user.Id);
            return CreatedAtAction(
                nameof(GetUserById),
                new { id = user.Id },
                new ApiResponse<User>
                {
                    Success = true,
                    Data = user
                });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating user with email: {Email}", userDto.Email);
            return StatusCode(StatusCodes.Status500InternalServerError,
                new ApiResponse
                {
                    Success = false,
                    Error = "Internal server error"
                }
            );
        }

    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(int id, UserUpdateDto userUpdate)
    {
        try
        {
            _logger.LogInformation("Updating user with ID: {UserId}", id);
            var success = await _userService.UpdateUserAsync(id, userUpdate);
            if (!success)
            {
                _logger.LogWarning("Failed to update user with ID: {UserId} - User not found or email conflict", id);
                return NotFound(new ApiResponse
                {
                    Success = false,
                    Error = "User not found or update failed"
                });
            }
            _logger.LogInformation("User with ID {UserId} updated successfully", id);
            return StatusCode(StatusCodes.Status200OK,
                new ApiResponse
                {
                    Success = true,
                }
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating user with ID: {UserId}", id);
            return StatusCode(StatusCodes.Status500InternalServerError,
                new ApiResponse
                {
                    Success = false,
                    Error = "Internal server error"
                }
            );
        }
    }
}
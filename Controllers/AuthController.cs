using System.Security.Claims;
using bookmark_manager_app.Controllers.Requests;
using bookmark_manager_app.Controllers.Responses;
using bookmark_manager_app.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace bookmark_manager_app.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(AuthService authService,
    IValidator<UserRegistrationRequest> userRegistrationRequestValidator) : ControllerBase
{
    [HttpPost("register")]
    public async Task<ActionResult<UserRegistrationResponse>> CreateUser(UserRegistrationRequest userRegistrationRequest)
    {
        await userRegistrationRequestValidator.ValidateAndThrowAsync(userRegistrationRequest);

        var user = await authService.RegisterAsync(userRegistrationRequest.Fullname, userRegistrationRequest.Email,
            userRegistrationRequest.Password);
        return CreatedAtRoute(nameof(UserController.GetUserByIdAsync), new { id = user.UserId },
            new UserRegistrationResponse(user.Fullname, user.Email));
    }

}
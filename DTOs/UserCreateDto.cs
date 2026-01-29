using System.ComponentModel.DataAnnotations;

namespace bookmark_manager_app.DTOs;

public record UserCreateDto
{
    [Required(ErrorMessage = "Full name is required")]
    public required string FullName { get; init; }

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Please enter a valid email address")]
    public required string Email { get; init; }

    [Required(ErrorMessage = "Password is required")]
     public required string Password { get; init; }
}
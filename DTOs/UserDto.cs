using System.ComponentModel.DataAnnotations;

namespace bookmark_manager_app.DTOs;

public record UserDto
{
    public int UserId { get; init; }
    public  string FullName { get; init; }= string.Empty;
    public required string Email { get; init; }
}
using System.ComponentModel.DataAnnotations;

namespace bookmark_manager_app.DTOs;

public record BookmarkUpdateDto
{
    public string? Title { get; init; }

    [Url(ErrorMessage = "Please enter a valid URL")]
    public string? Url { get; init; }

    [StringLength(280, ErrorMessage = "Description cannot exceed 280 characters")]
    public string? Description { get; init; }
    public ICollection<int>? TagIds { get; init; } 
}
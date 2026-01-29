using System.ComponentModel.DataAnnotations;

namespace bookmark_manager_app.DTOs;

public record BookmarkCreateDto
{
    [Required(ErrorMessage = "Title is required")]
    public required string Title { get; init; }

    [Required(ErrorMessage = "URL is required")]
    [Url(ErrorMessage = "Please enter a valid URL")]
    public required string Url { get; init; }

    [Required(ErrorMessage = "Description is required")]
    [StringLength(280, ErrorMessage = "Description cannot exceed 280 characters")]
    public required string Description { get; init; } 

    [Required(ErrorMessage = "TagIds collection is required (can be empty)")]
    public required ICollection<int> TagIds { get; init; }
}
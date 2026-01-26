namespace bookmark_manager_app.DTOs;

public record BookmarkPatchDto
{
    public bool? IsPinned { get; init; }
    public bool? IsArchived { get; init; }
}
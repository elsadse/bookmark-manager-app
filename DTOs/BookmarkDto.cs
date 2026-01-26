namespace bookmark_manager_app.DTOs;

public record BookmarkDto
{
    public int Id { get; init; }
    public int UserId { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Url { get; init; } = string.Empty;
    public string? Description { get; init; }
    public bool IsPinned { get; init; }
    public bool IsArchived { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; init; }
    public List<string> TagName { get; init; } = new();
    public int VisitCount { get; init; }
    public DateTime? LastVisitedAt { get; init; }
}
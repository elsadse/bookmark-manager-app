using System.ComponentModel.DataAnnotations;
using bookmark_manager_app.DTOs;

namespace bookmark_manager_app.Models;

public sealed class Bookmark
{
    public int BookmarkId { get; private set; }
    public int UserId { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string Url { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public bool IsPinned { get; private set; } = false;
    public bool IsArchived { get; private set; } = false;
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; private set; }
    public User? User { get; private set; }
    private readonly List<Visit> _visits = new();
    public IReadOnlyCollection<Visit> Visits => _visits.AsReadOnly();
    private readonly List<BookmarkTag> _bookmarkTags = new();
    public IReadOnlyCollection<BookmarkTag> BookmarkTags => _bookmarkTags.AsReadOnly();

    private Bookmark() { }

    private Bookmark(int userId, string title, string url, string? description)
    {
        UserId = userId;
        Title = title ?? throw new ArgumentNullException(nameof(title));
        Url = url ?? throw new ArgumentNullException(nameof(url));
        Description = description ?? throw new ArgumentNullException(nameof(url));
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = CreatedAt;
    }
    public static Bookmark Create(int userId, BookmarkCreateDto dto)
    {
        ValidateCreateInputs(dto);
        return new Bookmark(
            userId: userId,
            title: dto.Title,
            url: dto.Url,
            description: dto.Description
        );
    }

    public void Update(BookmarkUpdateDto dto)
    {
        bool hasChanges = false;
        if (dto.Title is not null && dto.Title.ToLower() != Title.ToLower())
        {
            Title = dto.Title;
            hasChanges = true;
        }
        if (dto.Url is not null && dto.Url.ToLower() != Url.ToLower())
        {
            Url = dto.Url;
            hasChanges = true;
        }
        if (dto.Description is not null && dto.Description.ToLower() != Description.ToLower())
        {
            Description = dto.Description;
            hasChanges = true;
        }
        if (hasChanges)
            UpdatedAt = DateTime.UtcNow;
    }

    public void Patch(BookmarkPatchDto dto)
    {
        bool hasChanges = false;
        if (dto.IsPinned.HasValue && dto.IsPinned != IsPinned)
        {
            IsPinned = dto.IsPinned.Value;
            hasChanges = true;
        }
        if (dto.IsArchived.HasValue && dto.IsArchived != IsArchived)
        {
            IsArchived = dto.IsArchived.Value;
            hasChanges = true;
        }
        if (hasChanges)
            UpdatedAt = DateTime.UtcNow;
    }

    public void AddVisit( )
    {
        _visits.Add(new Visit(BookmarkId));
    }

    private static void ValidateCreateInputs(BookmarkCreateDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Title))
            throw new ArgumentException("Title is required", nameof(dto.Title));
        if (string.IsNullOrWhiteSpace(dto.Url))
            throw new ArgumentException("URL is required", nameof(dto.Url));
        if (!Uri.TryCreate(dto.Url, UriKind.Absolute, out _))
            throw new ArgumentException("Invalid URL format", nameof(dto.Url));
        if (string.IsNullOrWhiteSpace(dto.Description))
            throw new ArgumentException("Description is required", nameof(dto.Description));
        if (dto.Description?.Length > 280)
            throw new ArgumentException("Description cannot exceed 280 characters", nameof(dto.Description));
    }
}
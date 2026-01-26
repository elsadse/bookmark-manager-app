using bookmark_manager_app.DTOs;

namespace bookmark_manager_app.Models;

public sealed class Visit
{
    public int VisitId { get; private set; }
    public int BookmarkId { get; private set; }
    public DateTime VisitDateAt { get; private set; }
    public Bookmark? Bookmark { get; private set; }

    private Visit() { }

    public Visit(int bookmarkId)
    {
        if (bookmarkId <= 0)
            throw new ArgumentException("BookmarkId must be a positive number", nameof(bookmarkId));
        BookmarkId = bookmarkId;
        VisitDateAt  = DateTime.UtcNow;
    }

}

using bookmark_manager_app.Models;

namespace bookmark_manager_app.Controllers.Responses;

public record GetAllBookmarksResponse(
    long? BookmarkId,
    string Title,
    string Url,
    string Description,
    bool IsPinned,
    bool IsArchived,
    string[] Tags,
    int VisitCount,
    DateTimeOffset? LastVisitTime,
    DateTimeOffset CreationTime
)
{
    public static GetAllBookmarksResponse FromModel(Bookmark bookmark) => new(
        bookmark.BookmarkId,
        bookmark.Title,
        bookmark.Url,
        bookmark.Description,
        bookmark.IsPinned,
        bookmark.IsArchived,
        bookmark.Tags
            .Select(t => t.Name)
            .ToArray(),
        bookmark.Visits.Count(),
        bookmark.Visits.OrderByDescending(v => v.VisitTime).Select(v => v.VisitTime).FirstOrDefault(),
        bookmark.CreationTime
    );
}
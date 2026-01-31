namespace bookmark_manager_app.Controllers.Responses;

public record GetBookmarkByIdResponse(string Title, string Url, string Description, bool IsPinned, bool IsArchived, IEnumerable<string> Tags, int VisitCount, DateTimeOffset? LastVisitTime);
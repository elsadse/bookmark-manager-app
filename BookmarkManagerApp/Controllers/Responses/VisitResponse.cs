namespace BookmarkManagerApp.Controllers.Responses;

public record VisitResponse(long BookmarkId, DateTimeOffset VisitTime);
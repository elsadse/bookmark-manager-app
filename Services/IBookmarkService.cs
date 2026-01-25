using bookmark_manager_app.Models;

namespace bookmark_manager_app.Services;

public interface IBookmarkService
{
    Task<Bookmark?> CreateBookmarkAsync(int userId, BookmarkCreateDto bookmarkDto);
    Task<Bookmark?> GetBookmarkByIdAsync(int bookmarkId);
    Task<IEnumerable<Bookmark>> GetBookmarkAsync(int userId);
    Task<bool> UpdateBookmarkAsync(int bookmarkId, BookmarkUpdateDto bookmarkUpdate);
    Task<bool> DeleteBookmarkAsync(int bookmarkId);
    Task<bool> TogglePinAsync(int bookmarkId);
    Task<bool> ToggleArchiveAsync(int bookmarkId);
    Task<Visit?> AddVisitAsync(int bookmarkId);
    Task<int> GetVisitCountAsync(int bookmarkId);
    Task<DateTime?> GetLastVisitedAsync(int bookmarkId);
}
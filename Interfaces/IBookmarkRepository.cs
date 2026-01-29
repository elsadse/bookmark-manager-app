using bookmark_manager_app.DTOs;
using bookmark_manager_app.Models;

namespace bookmark_manager_app.Interfaces;

public interface IBookmarkRepository
{
    Task<Bookmark?> GetByIdAsync(int bookmarkId, int userId);
    Task<BookmarkDto?> GetByIdWithDetailsAsync(int bookmarkId, int userId);
    Task<Bookmark?> GetByIdWithTagsAndVisitsAsync(int bookmarkId, int userId);
    Task<IEnumerable<Bookmark>> GetAllByUserIdAsync(int userId);
    Task<IEnumerable<BookmarkDto>> GetBookmarksWithDetailsByUserIdAsync(int userId);
    Task<Bookmark> CreateAsync(Bookmark bookmark);
    Task UpdateAsync(Bookmark bookmark, int userId);
    Task DeleteAsync(int bookmarkId, int userId);
    Task<bool> ExistsAsync(int bookmarkId);
}
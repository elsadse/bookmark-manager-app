using bookmark_manager_app.DTOs;
using bookmark_manager_app.Models;

namespace bookmark_manager_app.Interfaces;

public interface IBookmarkService
{
    Task<BookmarkDto?> CreateBookmarkAsync(int userId, BookmarkCreateDto command);
    Task<BookmarkDto?> GetBookmarkByIdAsync(int bookmarkId, int userId);
    Task<IEnumerable<BookmarkDto>> GetBookmarkAsync(int userId);
    Task UpdateBookmarkAsync(int bookmarkId, int userId, BookmarkUpdateDto command);
    Task DeleteBookmarkAsync(int bookmarkId, int userId);
    Task PatchBookmarkAsync(int bookmarkId, int userId, BookmarkPatchDto command);
    Task<Visit> AddVisitToBookmarkAsync(int bookmarkId, int userId);
    Task<Tag> AddTagToBookmarkAsync(string tagName);
}
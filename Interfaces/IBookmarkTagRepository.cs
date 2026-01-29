using bookmark_manager_app.Models;

namespace bookmark_manager_app.Interfaces;

public interface IBookmarkTagRepository
{
    Task<BookmarkTag?> GetByIdAsync(int bookmarkId, int tagId);
    Task<IEnumerable<BookmarkTag>> GetByBookmarkIdAsync(int bookmarkId);
    Task<IEnumerable<BookmarkTag>> GetByTagIdAsync(int tagId);
    Task<BookmarkTag> CreateAsync(BookmarkTag bookmarkTag);
    Task CreateRangeAsync(IEnumerable<BookmarkTag> bookmarkTags);
    Task DeleteAsync(int bookmarkId, int tagId);
    Task DeleteByBookmarkIdAsync(int bookmarkId);
    Task<bool> ExistsAsync(int bookmarkId, int tagId);
}
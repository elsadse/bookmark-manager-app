using bookmark_manager_app.Models;
using bookmark_manager_app.Persistence;
using Microsoft.EntityFrameworkCore;

namespace bookmark_manager_app.Repositories;

public class BookmarkRepository(BookmarkDbContext context)
{
    public async Task<Bookmark> CreateAsync(Bookmark bookmark)
    {
        await context.Bookmarks.AddAsync(bookmark);
        await context.SaveChangesAsync();
        return bookmark;
    }

    public async Task<bool> ExistsByUserIdAndTitleAndUrl(long userId, string title, string url) =>
        await context.Bookmarks
            .AsNoTracking()
            .AnyAsync(b => b.UserId == userId && b.Title == title && b.Url == url);

    public async Task DeleteAsync(Bookmark bookmark)
    {
        context.Bookmarks.Remove(bookmark);
        await context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Bookmark>> GetAllByUserIdAsync(long userId) =>
        await context.Bookmarks
            .AsNoTracking()
            .Where(b => b.UserId == userId)
            .Include(bt => bt.Tags)
            .Include(b => b.Visits)
            .ToListAsync();

    public async Task<Bookmark?> GetByIdAsync(long bookmarkId) =>
        await context.Bookmarks.AsNoTracking()
            .Include(x => x.Tags)
            .Include(b => b.Visits)
            .FirstOrDefaultAsync(x => x.BookmarkId == bookmarkId);

    public async Task UpdateTogglePinAsync(long bookmarkId)
    {
       await context.Bookmarks
        .Where(b => b.BookmarkId == bookmarkId)
        .ExecuteUpdateAsync(setters => setters
            .SetProperty(b => b.IsPinned, b => !b.IsPinned)
        );
    }

    public async Task UpdateToggleArchiveAsync(long bookmarkId)
    {
       await context.Bookmarks
        .Where(b => b.BookmarkId == bookmarkId)
        .ExecuteUpdateAsync(setters => setters
            .SetProperty(b => b.IsArchived, b => !b.IsArchived)
        );
    }
}
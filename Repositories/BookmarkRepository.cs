using bookmark_manager_app.DTOs;
using bookmark_manager_app.Interfaces;
using bookmark_manager_app.Models;
using bookmark_manager_app.Persistence;
using Microsoft.EntityFrameworkCore;

namespace bookmark_manager_app.Repositories;

public class BookmarkRepository : IBookmarkRepository
{
    private readonly BookmarkDbContext _dbContext;

    public BookmarkRepository(BookmarkDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Bookmark?> GetByIdAsync(int bookmarkId, int userId)
    {
        var bookmark = await _dbContext.Bookmarks
            .AsNoTracking()
            .Include(b => b.BookmarkTags).ThenInclude(bt => bt.Tag)
            .Include(b => b.Visits)
            .FirstOrDefaultAsync(b => b.BookmarkId == bookmarkId && b.UserId == userId);
        if (bookmark is null) return null;
        return bookmark;
    }

    public async Task<BookmarkDto?> GetByIdWithDetailsAsync(int bookmarkId, int userId)
    {
        var bookmark = await _dbContext.Bookmarks
            .AsNoTracking()
            .Include(b => b.BookmarkTags).ThenInclude(bt => bt.Tag)
            .Include(b => b.Visits)
            .FirstOrDefaultAsync(b => b.BookmarkId == bookmarkId && b.UserId == userId);
        return bookmark is null ? null : ToDto(bookmark);
    }

    public async Task<Bookmark?> GetByIdWithTagsAndVisitsAsync(int bookmarkId, int userId)
    {
        return await _dbContext.Bookmarks
            .Include(b => b.BookmarkTags)
                .ThenInclude(bt => bt.Tag)
            .Include(b => b.Visits)
            .FirstOrDefaultAsync(b => b.BookmarkId == bookmarkId && b.UserId == userId);
    }

    public async Task<IEnumerable<Bookmark>> GetAllByUserIdAsync(int userId)
    {
        return await _dbContext.Bookmarks
            .AsNoTracking()
            .Where(b => b.UserId == userId)
            .OrderByDescending(b => b.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<BookmarkDto>> GetBookmarksWithDetailsByUserIdAsync(int userId)
    {
        var bookmarks = await _dbContext.Bookmarks
            .AsNoTracking()
            .Where(b => b.UserId == userId)
            .Include(b => b.BookmarkTags).ThenInclude(bt => bt.Tag)
            .Include(b => b.Visits)
            .OrderByDescending(b => b.CreatedAt)
            .ToListAsync();
        return bookmarks.Select(ToDto);
    }

    public async Task<Bookmark> CreateAsync(Bookmark bookmark)
    {
        await _dbContext.Bookmarks.AddAsync(bookmark);
        await _dbContext.SaveChangesAsync();
        return bookmark;
    }

    public async Task UpdateAsync(Bookmark bookmark, int userId)
    {
        if (bookmark.UserId != userId)
            throw new UnauthorizedAccessException("Bookmark does not belong to this user");
        _dbContext.Bookmarks.Update(bookmark);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(int bookmarkId, int userId)
    {
        var bookmark = await GetByIdAsync(bookmarkId, userId);
        if (bookmark != null)
        {
            _dbContext.Bookmarks.Remove(bookmark);
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(int bookmarkId)
    {
        return await _dbContext.Bookmarks.AnyAsync(b => b.BookmarkId == bookmarkId);
    }

    private static BookmarkDto ToDto(Bookmark bookmark)
    {
        return new BookmarkDto
        {
            Id = bookmark.BookmarkId,
            UserId = bookmark.UserId,
            Title = bookmark.Title,
            Url = bookmark.Url,
            Description = bookmark.Description,
            IsPinned = bookmark.IsPinned,
            IsArchived = bookmark.IsArchived,
            CreatedAt = bookmark.CreatedAt,
            UpdatedAt = bookmark.UpdatedAt,
            TagName = bookmark.BookmarkTags
                .Select(bt => bt.Tag!.Name)
                .Where(name => !string.IsNullOrEmpty(name))
                .ToList(),
            VisitCount = bookmark.Visits.Count,
            LastVisitedAt = bookmark.Visits
                .OrderByDescending(v => v.VisitDateAt)
                .FirstOrDefault()?.VisitDateAt
        };
    }
}
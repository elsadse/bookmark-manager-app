using bookmark_manager_app.Interfaces;
using bookmark_manager_app.Models;
using bookmark_manager_app.Persistence;
using Microsoft.EntityFrameworkCore;

namespace bookmark_manager_app.Repositories;

public class BookmarkTagRepository : IBookmarkTagRepository
{
    private readonly BookmarkDbContext _dbContext;

    public BookmarkTagRepository(BookmarkDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<BookmarkTag?> GetByIdAsync(int bookmarkId, int tagId)
    {
        return await _dbContext.BookmarkTags
            .Include(bt => bt.Bookmark)
            .Include(bt => bt.Tag)
            .FirstOrDefaultAsync(bt => bt.BookmarkId == bookmarkId && bt.TagId == tagId);
    }

    public async Task<IEnumerable<BookmarkTag>> GetByBookmarkIdAsync(int bookmarkId)
    {
        return await _dbContext.BookmarkTags
            .Include(bt => bt.Tag)
            .Where(bt => bt.BookmarkId == bookmarkId)
            .OrderBy(bt => bt.Tag!.Name)
            .ToListAsync();
    }

    public async Task<IEnumerable<BookmarkTag>> GetByTagIdAsync(int tagId)
    {
        return await _dbContext.BookmarkTags
            .Include(bt => bt.Bookmark)
            .Where(bt => bt.TagId == tagId)
            .OrderByDescending(bt => bt.Bookmark!.CreatedAt)
            .ToListAsync();
    }

    public async Task<BookmarkTag> CreateAsync(BookmarkTag bookmarkTag)
    {
        await _dbContext.BookmarkTags.AddAsync(bookmarkTag);
        await _dbContext.SaveChangesAsync();
        return bookmarkTag;
    }

    public async Task CreateRangeAsync(IEnumerable<BookmarkTag> bookmarkTags)
    {
        await _dbContext.BookmarkTags.AddRangeAsync(bookmarkTags);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(int bookmarkId, int tagId)
    {
        var bookmarkTag = await GetByIdAsync(bookmarkId, tagId);
        if (bookmarkTag != null)
        {
            _dbContext.BookmarkTags.Remove(bookmarkTag);
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task DeleteByBookmarkIdAsync(int bookmarkId)
    {
        var bookmarkTags = await GetByBookmarkIdAsync(bookmarkId);
        if (bookmarkTags.Any())
        {
            _dbContext.BookmarkTags.RemoveRange(bookmarkTags);
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(int bookmarkId, int tagId)
    {
        return await _dbContext.BookmarkTags
            .AnyAsync(bt => bt.BookmarkId == bookmarkId && bt.TagId == tagId);
    }

}
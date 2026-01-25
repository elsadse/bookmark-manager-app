using bookmark_manager_app.Data;
using bookmark_manager_app.Models;
using Microsoft.EntityFrameworkCore;

namespace bookmark_manager_app.Services;

public class BookmarkService : IBookmarkService
{
    private readonly BookmarkDbContext _context;
    private readonly ILogger<BookmarkService> _logger;

    public BookmarkService(BookmarkDbContext context, ILogger<BookmarkService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Bookmark?> CreateBookmarkAsync(int userId, BookmarkCreateDto bookmarkDto)
    {
        try
        {
            //verify if user exist
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                _logger.LogWarning("User with ID {UserId} not found", userId);
                return null;
            }
            //verify if bookmark exist
            var existingBookmark = await _context.Bookmarks.FirstOrDefaultAsync(b => b.UserId == userId && (b.Title == bookmarkDto.Title || b.Url == bookmarkDto.Url));
            if (existingBookmark != null)
            {
                _logger.LogWarning("Bookmark already exists for user {UserId}", userId);
                return null;
            }
            //create bookmark 
            var bookmark = new Bookmark
            {
                UserId = userId,
                Title = bookmarkDto.Title,
                Url = bookmarkDto.Url,
                Description = bookmarkDto.Description,
                CreatedAt = DateTime.UtcNow
            };
            _context.Bookmarks.Add(bookmark);
            await _context.SaveChangesAsync();
            // Add tags in bookmark_tags
            if (bookmarkDto.TagIds != null && bookmarkDto.TagIds.Any())
            {
                foreach (var tagId in bookmarkDto.TagIds)
                {
                    var tag = await _context.Tags.FindAsync(tagId);
                    if (tag != null)
                    {
                        var bookmarkTag = new BookmarkTag
                        {
                            BookmarkId = bookmark.BookmarkId,
                            TagId = tagId
                        };
                        _context.BookmarkTags.Add(bookmarkTag);
                    }
                }
                await _context.SaveChangesAsync();
            }
            _logger.LogInformation("Bookmark created with ID: {BookmarkId}", bookmark.BookmarkId);
            return bookmark;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating bookmark for user {UserId}", userId);
            return null;
        }

    }

    public async Task<Bookmark?> GetBookmarkByIdAsync(int bookmarkId)
    {
        try
        {
            return await _context.Bookmarks
                .Include(b => b.User)
                .Include(b => b.BookmarkTags)
                    .ThenInclude(bt => bt.Tag)
                .Include(b => b.Visits)
                .FirstOrDefaultAsync(b => b.BookmarkId == bookmarkId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting bookmark with ID: {BookmarkId}", bookmarkId);
            return null;
        }
    }

    public async Task<IEnumerable<Bookmark>> GetBookmarkAsync(int userId)
    {
        try
        {
            return await _context.Bookmarks
                .Include(b => b.BookmarkTags)
                    .ThenInclude(bt => bt.Tag)
                .Include(b => b.Visits)
                .Where(b => b.UserId == userId)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting bookmarks for user {UserId}", userId);
            return Enumerable.Empty<Bookmark>();
        }
    }

    public async Task<bool> UpdateBookmarkAsync(int bookmarkId, BookmarkUpdateDto bookmarkUpdate)
    {
        try
        {
            bool hasChanges = false;

            var bookmark = await _context.Bookmarks.Include(b => b.BookmarkTags).FirstOrDefaultAsync(b => b.BookmarkId == bookmarkId);
            if (bookmark == null)
            {
                _logger.LogWarning("Bookmark with ID {BookmarkId} not found", bookmarkId);
                return false;
            }

            if (!string.IsNullOrEmpty(bookmarkUpdate.Title) && bookmarkUpdate.Title != bookmark.Title)
            {
                bookmark.Title = bookmarkUpdate.Title;
                hasChanges = true;
            }
            if (!string.IsNullOrEmpty(bookmarkUpdate.Url) && bookmarkUpdate.Url != bookmark.Url)
            {
                bookmark.Url = bookmarkUpdate.Url;
                hasChanges = true;
            }
            if (!string.IsNullOrEmpty(bookmarkUpdate.Description) && bookmarkUpdate.Description != bookmark.Description)
            {
                bookmark.Description = bookmarkUpdate.Description;
                hasChanges = true;
            }
            if (bookmarkUpdate.TagIds != null)
            {
                //delete tags in bookmark_tags
                var existingTags = bookmark.BookmarkTags.ToList();
                foreach (var existingTag in existingTags)
                {
                    _context.BookmarkTags.Remove(existingTag);
                }
                //create tags in bookmark_tags
                foreach (var tagId in bookmarkUpdate.TagIds)
                {
                    var tag = await _context.Tags.FindAsync(tagId);
                    if (tag != null)
                    {
                        var bookmarkTag = new BookmarkTag
                        {
                            BookmarkId = bookmarkId,
                            TagId = tagId
                        };
                        _context.BookmarkTags.Add(bookmarkTag);
                    }
                }
                hasChanges = true;
                if (hasChanges)
                {
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Bookmark with ID {BookmarkId} updated", bookmarkId);
                    return true;
                }
            }
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating bookmark with ID: {BookmarkId}", bookmarkId);
            return false;
        }
    }

    public async Task<bool> DeleteBookmarkAsync(int bookmarkId)
    {
        try
        {
            var bookmark = await _context.Bookmarks.FindAsync(bookmarkId);
            if (bookmark == null)
            {
                _logger.LogWarning("Bookmark with ID {BookmarkId} not found", bookmarkId);
                return false;
            }
            _context.Bookmarks.Remove(bookmark);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Bookmark with ID {BookmarkId} deleted", bookmarkId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting bookmark with ID: {BookmarkId}", bookmarkId);
            return false;
        }
    }

    public async Task<bool> TogglePinAsync(int bookmarkId)
    {
        try
        {
            var bookmark = await _context.Bookmarks.FindAsync(bookmarkId);
            if (bookmark == null)
            {
                _logger.LogWarning("Bookmark with ID {BookmarkId} not found", bookmarkId);
                return false;
            }
            bookmark.IsPinned = !bookmark.IsPinned;
            await _context.SaveChangesAsync();
            _logger.LogInformation("Bookmark with ID {BookmarkId} pin toggled to {IsPinned}", bookmarkId, bookmark.IsPinned);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error toggling pin for bookmark with ID: {BookmarkId}", bookmarkId);
            return false;
        }
    }

    public async Task<bool> ToggleArchiveAsync(int bookmarkId)
    {
        try
        {
            var bookmark = await _context.Bookmarks.FindAsync(bookmarkId);
            if (bookmark == null)
            {
                _logger.LogWarning("Bookmark with ID {BookmarkId} not found", bookmarkId);
                return false;
            }
            bookmark.IsArchived = !bookmark.IsArchived;
            await _context.SaveChangesAsync();
            _logger.LogInformation("Bookmark with ID {BookmarkId} archive toggled to {IsArchived}", bookmarkId, bookmark.IsArchived);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error toggling archive for bookmark with ID: {BookmarkId}", bookmarkId);
            return false;
        }
    }

    public async Task<Visit?> AddVisitAsync(int bookmarkId)
    {
        try
        {
            var bookmark = await _context.Bookmarks.FindAsync(bookmarkId);
            if (bookmark == null)
            {
                _logger.LogWarning("Bookmark with ID {BookmarkId} not found", bookmarkId);
                return null;
            }
            var visit = new Visit
            {
                BookmarkId = bookmarkId,
                VisitDateAt = DateTime.UtcNow
            };
            _context.Visits.Add(visit);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Visit added for bookmark with ID {BookmarkId}", bookmarkId);
            return visit;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding visit for bookmark with ID: {BookmarkId}", bookmarkId);
            return null;
        }
    }

    public async Task<int> GetVisitCountAsync(int bookmarkId)
    {
        try
        {
            return await _context.Visits.CountAsync(v => v.BookmarkId == bookmarkId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting visit count for bookmark with ID: {BookmarkId}", bookmarkId);
            return 0;
        }
    }

    public async Task<DateTime?> GetLastVisitedAsync(int bookmarkId)
    {
        try
        {
            return (await _context.Visits.Where(v => v.BookmarkId == bookmarkId).OrderByDescending(v => v.BookmarkId).FirstOrDefaultAsync()).VisitDateAt;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting last visited date for bookmark with ID: {BookmarkId}", bookmarkId);
            return null;
        }
    }
}

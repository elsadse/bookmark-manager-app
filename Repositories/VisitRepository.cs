using bookmark_manager_app.Models;
using bookmark_manager_app.Persistence;
using Microsoft.EntityFrameworkCore;

namespace bookmark_manager_app.Repositories;

public class VisitRepository(BookmarkDbContext context)
{
    public async Task<Visit> CreateAsync(Visit visit)
    {
        await context.Visits.AddAsync(visit);
        await context.SaveChangesAsync();
        return visit;
    }

    public async Task<int> GetVisitCountByBookmarkIdAsync(long bookmarkId)
    {
        return await context.Visits.AsNoTracking().CountAsync(v => v.BookmarkId == bookmarkId);
    }

    public async Task<DateTimeOffset?> GetLastVisitDateByBookmarkIdAsync(long bookmarkId)
    {
        return await context.Visits.AsNoTracking()
            .Where(v => v.BookmarkId == bookmarkId)
            .OrderByDescending(v => v.VisitTime)
            .Select(v => v.VisitTime)
            .FirstOrDefaultAsync();
    }
}
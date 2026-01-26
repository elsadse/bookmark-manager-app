using bookmark_manager_app.Interfaces;
using bookmark_manager_app.Models;
using bookmark_manager_app.Persistence;
using Microsoft.EntityFrameworkCore;

namespace bookmark_manager_app.Repositories;

public class VisitRepository : IVisitRepository
{
    private readonly BookmarkDbContext _dbContext;

    public VisitRepository(BookmarkDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Visit> CreateAsync(Visit visit)
    {
        await _dbContext.Visits.AddAsync(visit);
        await _dbContext.SaveChangesAsync();
        return visit;
    }

    public async Task<int> GetVisitCountByBookmarkIdAsync(int bookmarkId)
    {
        return await _dbContext.Visits.CountAsync(v => v.BookmarkId == bookmarkId);
    }

    public async Task<DateTime?> GetLastVisitDateByBookmarkIdAsync(int bookmarkId)
    {
        return await _dbContext.Visits
            .Where(v => v.BookmarkId == bookmarkId)
            .OrderByDescending(v => v.VisitDateAt)
            .Select(v => v.VisitDateAt)
            .FirstOrDefaultAsync();
    }

    public async Task CreateRangeAsync(IEnumerable<Visit> visits)
    {
        await _dbContext.Visits.AddRangeAsync(visits);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<Visit?> GetByIdAsync(int visitId)
    {
        return await _dbContext.Visits
            .Include(v => v.Bookmark)
            .FirstOrDefaultAsync(v => v.VisitId == visitId);
    }

    public async Task DeleteAsync(int visitId)
    {
        var visit = await GetByIdAsync(visitId);
        if (visit != null)
        {
            _dbContext.Visits.Remove(visit);
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<Visit>> GetByBookmarkIdAsync(int bookmarkId)
    {
        return await _dbContext.Visits
            .Where(v => v.BookmarkId == bookmarkId)
            .OrderByDescending(v => v.VisitDateAt)
            .ToListAsync();
    }


    public async Task DeleteByBookmarkIdAsync(int bookmarkId)
    {
        var visits = await GetByBookmarkIdAsync(bookmarkId);
        if (visits.Any())
        {
            _dbContext.Visits.RemoveRange(visits);
            await _dbContext.SaveChangesAsync();
        }
    }
}
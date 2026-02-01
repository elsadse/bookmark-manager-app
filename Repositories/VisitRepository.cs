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
}
using bookmark_manager_app.Models;
using bookmark_manager_app.Persistence;
using Microsoft.EntityFrameworkCore;

namespace bookmark_manager_app.Repositories;

public class TagRepository(BookmarkDbContext context)
{
    public async Task<IEnumerable<Tag>> GetByNames(IEnumerable<string> names)
    {
        return await context.Tags.Where(t => names.Contains(t.Name)).ToListAsync();
    }

    public async Task<bool> ExistsByName(string name)
    {
        return await context.Tags.AsNoTracking().AnyAsync(t => t.Name == name);
    }

    public async Task<IDictionary<string, int>> GetTagUsageCountsAsync(long userId)
    {
        return await context.Bookmarks
            .Where(b => b.UserId == userId)
            .SelectMany(b => b.Tags)
            .GroupBy(tag => tag.Name)
            .Select(g => new
            {
                TagName = g.Key,
                Count = g.Count()
            })
            .OrderBy(x => x.TagName)
            .ToDictionaryAsync(
                x => x.TagName,
                x => x.Count
            );
    }

}
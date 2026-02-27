using BookmarkManagerApp.Models;
using BookmarkManagerApp.Persistence;
using BookmarkManagerApp.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace BookmarkManagerApp.Repositories;

public class TagRepository(BookmarkDbContext context) : ITagRepository
{
    public async Task<IEnumerable<Tag>> GetByNames(IEnumerable<string> names) =>
        await context.Tags.Where(t => names.Contains(t.Name)).ToListAsync();

    public async Task<IEnumerable<Tag>> GetTagAllForUserAsync(long userId) =>
        await context.Tags
            .AsNoTracking()
            .Where(t => t.Bookmarks.Any(b => b.UserId == userId))
            .ToListAsync();

}

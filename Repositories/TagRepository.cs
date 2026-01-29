using bookmark_manager_app.Interfaces;
using bookmark_manager_app.Models;
using bookmark_manager_app.Persistence;
using Microsoft.EntityFrameworkCore;

namespace bookmark_manager_app.Repositories;

public class TagRepository : ITagRepository
{
    private readonly BookmarkDbContext _dbContext;

    public TagRepository(BookmarkDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Tag?> GetByNameAsync(string name)
    {
        return await _dbContext.Tags
            .FirstOrDefaultAsync(t => t.Name.ToLower() == name.ToLower().Trim());
    }

    public async Task<IEnumerable<Tag>> GetTagsByIdsAsync(IEnumerable<int> tagIds)
    {
        return await _dbContext.Tags
            .Where(t => tagIds.Contains(t.TagId))
            .ToListAsync();
    }

    public async Task<Tag?> GetByIdAsync(int tagId)
    {
        return await _dbContext.Tags
            .FirstOrDefaultAsync(t => t.TagId == tagId);
    }

    public async Task<IEnumerable<Tag>> GetAllAsync()
    {
        return await _dbContext.Tags
            .OrderBy(t => t.Name)
            .ToListAsync();
    }

    public async Task<Tag> CreateAsync(Tag tag)
    {
        await _dbContext.Tags.AddAsync(tag);
        await _dbContext.SaveChangesAsync();
        return tag;
    }

    public async Task UpdateAsync(Tag tag)
    {
        _dbContext.Tags.Update(tag);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(int tagId)
    {
        var tag = await GetByIdAsync(tagId);
        if (tag != null)
        {
            _dbContext.Tags.Remove(tag);
            await _dbContext.SaveChangesAsync();
        }
    }
}
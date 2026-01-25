using bookmark_manager_app.Data;
using bookmark_manager_app.Models;
using Microsoft.EntityFrameworkCore;

namespace bookmark_manager_app.Services;

public class TagService : ITagService
{
    private readonly BookmarkDbContext _context;
    private readonly ILogger<TagService> _logger;

    public TagService(BookmarkDbContext context, ILogger<TagService> logger)
    {
        _context = context;
        _logger = logger;
    }
    public async Task<Tag?> CreateTagAsync(TagCreateDto tagDto)
    {
        try
        {
            var existingTag = await _context.Tags.FirstOrDefaultAsync(t => t.Name.ToLower() == tagDto.Name.ToLower());
            if (existingTag != null)
            {
                _logger.LogWarning("Tag {Name} already exists", tagDto.Name);
                return existingTag;
            }
            var tag = new Tag
            {
                Name = tagDto.Name
            };
            _context.Tags.Add(tag);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Tag created with ID: {TagId}", tag.TagId);
            return tag;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating tag {Name}", tagDto.Name);
            return null;
        }
    }

    public async Task<IEnumerable<Tag>> GetAllTagsAsync()
    {
        try
        {
            return await _context.Tags.OrderBy(t => t.Name).ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all tags");
            return Enumerable.Empty<Tag>();
        }
    }
}
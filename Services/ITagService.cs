using bookmark_manager_app.Models;

namespace bookmark_manager_app.Services;

public interface ITagService
{
    Task<Tag?> CreateTagAsync(TagCreateDto tagDto);
    Task<IEnumerable<Tag>> GetAllTagsAsync();
}
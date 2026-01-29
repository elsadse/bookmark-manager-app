using bookmark_manager_app.Models;

namespace bookmark_manager_app.Interfaces;

public interface ITagRepository
{
    Task<IEnumerable<Tag>> GetTagsByIdsAsync(IEnumerable<int> tagIds);
    Task<Tag?> GetByIdAsync(int tagId);
    Task<Tag?> GetByNameAsync(string name);
    Task<IEnumerable<Tag>> GetAllAsync();
    Task<Tag> CreateAsync(Tag tag);
    Task UpdateAsync(Tag tag);
    Task DeleteAsync(int tagId);
}
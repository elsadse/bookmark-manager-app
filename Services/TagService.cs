using bookmark_manager_app.Models;
using bookmark_manager_app.Repositories;
namespace bookmark_manager_app.Services;

public class TagService(TagRepository tagRepository)
{
    public async Task<IEnumerable<Tag>> GetTagsAsync()
    {
        return await tagRepository.GetAllAsync();;
    }
}
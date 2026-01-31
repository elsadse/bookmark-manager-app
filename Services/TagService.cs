using bookmark_manager_app.Repositories;
using bookmark_manager_app.Services.Utils;

namespace bookmark_manager_app.Services;

public class TagService(TagRepository tagRepository, UserContext userContext)
{
    public async Task<IDictionary<string, int>> GetTagUsageCountsAsync()
    {
        return await tagRepository.GetTagUsageCountsAsync(userContext.UserId);
    }
}
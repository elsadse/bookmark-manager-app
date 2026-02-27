using BookmarkManagerApp.Models;
using BookmarkManagerApp.Repositories.Contracts;
using BookmarkManagerApp.Services.Contracts;
namespace BookmarkManagerApp.Services;

public class TagService(ITagRepository tagRepository, IUserContext userContext)
{
    public async Task<IEnumerable<Tag>> GetTagsAsync()
    {
        return await tagRepository.GetTagAllForUserAsync(userContext.UserId);
    }
}
using BookmarkManagerApp.Models;

namespace BookmarkManagerApp.Repositories.Contracts;

public interface ITagRepository
{
    Task<IEnumerable<Tag>> GetByNames(IEnumerable<string> names);
    Task<IEnumerable<Tag>> GetTagAllForUserAsync(long userId);
}
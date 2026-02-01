using bookmark_manager_app.Exceptions;
using bookmark_manager_app.Models;
using bookmark_manager_app.Repositories;
using bookmark_manager_app.Services.Utils;

namespace bookmark_manager_app.Services;

public class BookmarkService(
    BookmarkRepository bookmarkRepository, VisitRepository visitRepository,
    UserContext userContext,
    TagRepository tagRepository)
{
    public async Task<IEnumerable<Bookmark>> GetAllByUserIdAsync() =>
        await bookmarkRepository.GetAllByUserIdAsync(userContext.UserId);

    public async Task<Bookmark> GetByIdAsync(long bookmarkId)
    {
        var bookmark = await bookmarkRepository.GetByIdAsync(bookmarkId);
        return bookmark ?? throw new NotFoundException("Bookmark not found");
    }

    public async Task<Bookmark> CreateAsync(CreateBookmarkCommand command)
    {
        if (await bookmarkRepository.ExistsByUserIdAndTitleAndUrl(userContext.UserId, command.Title, command.Url))
        {
            throw new ConflictException("Bookmark with this title and/or url already exists");
        }

        var bookmark = new Bookmark
        {
            UserId = userContext.UserId,
            Title = command.Title,
            Url = command.Url,
            Description = command.Description,
            Tags = new List<Tag>(),
        };

        if (command.TagNames.Length == 0) return await bookmarkRepository.CreateAsync(bookmark);

        var existingTags = await tagRepository.GetByNames(command.TagNames);
        var existingTagsByNames = existingTags.ToDictionary(tag => tag.Name);

        foreach (var tagName in command.TagNames)
        {
            bookmark.Tags.Add(
                existingTagsByNames.TryGetValue(tagName, out var tag)
                    ? tag
                    : new Tag { Name = tagName }
            );
        }

        return await bookmarkRepository.CreateAsync(bookmark);
    }

    public async Task<Visit> CreateVisitAsync(long bookmarkId)
    {
        var bookmark = await GetByIdAsync(bookmarkId);
        var visit = new Visit
        {
            BookmarkId = bookmarkId
        };
        bookmark.Visits.Add(visit);
        return await visitRepository.CreateAsync(visit);
    }

    public async Task TogglePinAsync(long bookmarkId)
    {
        await bookmarkRepository.UpdateTogglePinAsync(bookmarkId);
    }

    public async Task ToggleArchiveAsync(long bookmarkId)
    {
        await bookmarkRepository.UpdateToggleArchiveAsync(bookmarkId);
    }
}

public record CreateBookmarkCommand(string Title, string Url, string Description, string[] TagNames);
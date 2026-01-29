using bookmark_manager_app.DTOs;
using bookmark_manager_app.Exceptions;
using bookmark_manager_app.Interfaces;
using bookmark_manager_app.Models;

namespace bookmark_manager_app.Services;

public class BookmarkService : IBookmarkService
{
    private readonly IBookmarkRepository _bookmarkRepository;
    private readonly ITagRepository _tagRepository;
    private readonly IBookmarkTagRepository _bookmarkTagRepository;
    private readonly IVisitRepository _visitRepository;
    private readonly IUserRepository _userRepository;

    public BookmarkService(IBookmarkRepository bookmarkRepository, ITagRepository tagRepository, IBookmarkTagRepository bookmarkTagRepository, IVisitRepository visitRepository, IUserRepository userRepository)
    {
        _bookmarkRepository = bookmarkRepository;
        _tagRepository = tagRepository;
        _bookmarkTagRepository = bookmarkTagRepository;
        _visitRepository = visitRepository;
        _userRepository = userRepository;
    }

    public async Task<BookmarkDto?> CreateBookmarkAsync(int userId, BookmarkCreateDto command)
    {
        var userExists = await _userRepository.ExistsAsync(userId);
        if (!userExists)
            throw new NotFoundException($"User with ID {userId} not found");
        var bookmark = Bookmark.Create(userId, command);
        var createdBookmark = await _bookmarkRepository.CreateAsync(bookmark);
        foreach (int tagId in command.TagIds)
        {
            var existingTag = await _tagRepository.GetByIdAsync(tagId);
            if (existingTag == null)
                throw new NotFoundException($"Tag with ID {tagId} not found");
            var bookmarkTag = BookmarkTag.Create(createdBookmark.BookmarkId, tagId);
            await _bookmarkTagRepository.CreateAsync(bookmarkTag);
        }
        var bookmarkDto = await _bookmarkRepository.GetByIdWithDetailsAsync(createdBookmark.BookmarkId, userId);
        return bookmarkDto;
    }

    public async Task<BookmarkDto?> GetBookmarkByIdAsync(int bookmarkId, int userId)
    {
        var bookmark = await _bookmarkRepository.GetByIdWithDetailsAsync(bookmarkId, userId);
        if (bookmark is null) return null;
        return bookmark;
    }

    public async Task<IEnumerable<BookmarkDto>> GetBookmarkAsync(int userId)
    {
        var userExists = await _userRepository.ExistsAsync(userId);
        if (!userExists)
            throw new NotFoundException($"User with ID {userId} not found");
        return await _bookmarkRepository.GetBookmarksWithDetailsByUserIdAsync(userId);
    }

    public async Task UpdateBookmarkAsync(int bookmarkId, int userId, BookmarkUpdateDto command)
    {
        var existingBookmark = await _bookmarkRepository.GetByIdWithTagsAndVisitsAsync(bookmarkId, userId);
        if (existingBookmark is null)
            throw new NotFoundException($"Bookmark with ID {bookmarkId} not found");
        if (command.TagIds is not null)
        {
            await _bookmarkTagRepository.DeleteByBookmarkIdAsync(bookmarkId);
            foreach (int tagId in command.TagIds)
            {
                var existingTag = await _tagRepository.GetByIdAsync(tagId);
                if (existingTag == null)
                    throw new NotFoundException($"Tag with ID {tagId} not found");
                await _bookmarkTagRepository.CreateAsync(BookmarkTag.Create(bookmarkId, tagId));
            }
        }
        existingBookmark.Update(command);
        await _bookmarkRepository.UpdateAsync(existingBookmark, userId);
    }

    public async Task DeleteBookmarkAsync(int bookmarkId, int userId)
    {
        var bookmark = await _bookmarkRepository.GetByIdAsync(bookmarkId, userId);
        if (bookmark is null)
            throw new NotFoundException($"Bookmark with ID {bookmarkId} not found");
        await _visitRepository.DeleteByBookmarkIdAsync(bookmarkId);
        foreach (var bookmarkTag in bookmark.BookmarkTags)
        {
            await _tagRepository.DeleteAsync(bookmarkTag.TagId);
        }
        await _bookmarkTagRepository.DeleteByBookmarkIdAsync(bookmarkId);
        await _bookmarkRepository.DeleteAsync(bookmarkId, userId);

    }

    public async Task PatchBookmarkAsync(int bookmarkId, int userId, BookmarkPatchDto command)
    {
        var existingBookmark = await _bookmarkRepository.GetByIdAsync(bookmarkId, userId);
        if (existingBookmark is null)
            throw new NotFoundException($"Bookmark with ID {bookmarkId} not found");

        existingBookmark.Patch(command);
        await _bookmarkRepository.UpdateAsync(existingBookmark, userId);
    }

    public async Task<Visit> AddVisitToBookmarkAsync(int bookmarkId, int userId)
    {
        var bookmark = await _bookmarkRepository.GetByIdAsync(bookmarkId, userId);
        if (bookmark == null)
            throw new NotFoundException($"Bookmark with ID {bookmarkId} not found");
        var visit = new Visit(bookmarkId);
        bookmark.AddVisit();
        var createdVisit = await _visitRepository.CreateAsync(visit);
        return createdVisit;
    }

    public async Task<Tag> AddTagToBookmarkAsync(string tagName)
    {
        var existingTag = await _tagRepository.GetByNameAsync(tagName);
        if (existingTag != null)
            throw new ConflictException($"{tagName} already exist");
        var tag = new Tag(tagName);
        return await _tagRepository.CreateAsync(tag);
    }


}

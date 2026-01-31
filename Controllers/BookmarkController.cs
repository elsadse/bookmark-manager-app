using bookmark_manager_app.Controllers.Requests;
using bookmark_manager_app.Controllers.Responses;
using bookmark_manager_app.Models;
using bookmark_manager_app.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace bookmark_manager_app.Controllers;

[ApiController]
[Authorize]
[Route("/api/bookmarks")]
public class BookmarkController(
    BookmarkService bookmarkService,
    IValidator<CreateBookmarkRequest> createBookmarkRequestValidator) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<CreateBookmarkResponse>> CreateAsync(CreateBookmarkRequest request)
    {
        await createBookmarkRequestValidator.ValidateAndThrowAsync(request);

        var bookmark = await bookmarkService.CreateAsync(request.ToCommand());
        var tagNames = bookmark.Tags
            .Select(x => x.Name)
            .ToArray();

        return CreatedAtRoute(nameof(GetByIdAsync), new { Id = bookmark.BookmarkId },
            new CreateBookmarkResponse(bookmark.Title, bookmark.Url, bookmark.Description, tagNames, 0, null));
    }

    [HttpGet("{id:long}", Name = nameof(GetByIdAsync))]
    public async Task<ActionResult<GetBookmarkByIdResponse>> GetByIdAsync(long id)
    {
        var bookmark = await bookmarkService.GetByIdAsync(id);
        var visitCount = await bookmarkService.GetVisitCount(id);
        var lastVisitTime = await bookmarkService.GetLastDateVisit(id);
        return Ok(new GetBookmarkByIdResponse(bookmark.Title, bookmark.Url, bookmark.Description, bookmark.IsPinned,
            bookmark.IsArchived, bookmark.Tags.Select(x => x.Name).ToArray(), visitCount, lastVisitTime));
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<GetAllBookmarksResponse>>> GetAllByUserIdAsync()
    {
        var bookmarks = await bookmarkService.GetAllByUserIdAsync();
        return Ok(bookmarks.Select(GetAllBookmarksResponse.FromModel));
    }

    [HttpPost("{id:long}/visits")]
    public async Task<ActionResult<Visit>> AddVisitAsync(long id)
    {
        var visit = await bookmarkService.CreateVisitAsync(id);
        return StatusCode(StatusCodes.Status201Created, visit);
    }
}
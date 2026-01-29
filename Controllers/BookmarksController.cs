using bookmark_manager_app.DTOs;
using bookmark_manager_app.Exceptions;
using bookmark_manager_app.Interfaces;
using bookmark_manager_app.Models;
using Microsoft.AspNetCore.Mvc;

namespace bookmark_manager_app.Controllers;

[ApiController]
[Route("/api/users/{userId}/bookmarks")]
public class BookmarksController : ControllerBase
{
    private readonly IBookmarkService _bookmarkService;
    private readonly ILogger<BookmarksController> _logger;

    public BookmarksController(IBookmarkService bookmarkService, ILogger<BookmarksController> logger)
    {
        _bookmarkService = bookmarkService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BookmarkDto>>> GetBookmarkByUser(int userId)
    {
        var bookmarks = await _bookmarkService.GetBookmarkAsync(userId);
        return Ok(bookmarks);
    }

    [HttpGet("{bookmarkId}")]
    public async Task<ActionResult<BookmarkDto>> GetBookmark(int userId, int bookmarkId)
    {
        var bookmark = await _bookmarkService.GetBookmarkByIdAsync(bookmarkId, userId);
        if (bookmark == null) return NotFound();
        return Ok(bookmark);
    }

    [HttpPost]
    public async Task<ActionResult<Bookmark>> CreateBookmark(int userId, [FromBody] BookmarkCreateDto command)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState
            .Where(e => e.Value?.Errors.Count > 0)
            .ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
            );
            throw new ValidationException(errors);
        }
        var bookmark = await _bookmarkService.CreateBookmarkAsync(userId, command);
        if (bookmark == null)
            throw new BadRequestException("Failed to create bookmark");
        return CreatedAtAction(
            nameof(GetBookmark),
            new { bookmarkId = bookmark.Id, userId = userId },
            bookmark);
    }

    [HttpPut("{bookmarkId}")]
    public async Task<IActionResult> UpdateBookmark(int userId, int bookmarkId, [FromBody] BookmarkUpdateDto command)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState
            .Where(e => e.Value?.Errors.Count > 0)
            .ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
            );
            throw new ValidationException(errors);
        }
        await _bookmarkService.UpdateBookmarkAsync(bookmarkId, userId, command);
        return NoContent();
    }

    [HttpDelete("{bookmarkId}")]
    public async Task<IActionResult> DeleteBookmark(int userId, int bookmarkId)
    {
        await _bookmarkService.DeleteBookmarkAsync(bookmarkId, userId);
        return NoContent();
    }

    [HttpPatch("{bookmarkId}/pin")]
    public async Task<IActionResult> TogglePin(int userId, int bookmarkId)
    {
        var bookmark = await _bookmarkService.GetBookmarkByIdAsync(bookmarkId, userId);
        if (bookmark == null)
            throw new NotFoundException("Bookmark ID is not found");
        var patchDto = new BookmarkPatchDto { IsPinned = !bookmark.IsPinned };
        await _bookmarkService.PatchBookmarkAsync(bookmarkId, userId, patchDto);
        return Ok(new
        {
            message = $"Bookmark {(bookmark.IsPinned ? "pinned" : "unpinned")} successfully",
            isPinned = patchDto.IsPinned
        });
    }

    [HttpPatch("{bookmarkId}/archive")]
    public async Task<IActionResult> ToggleArchive(int userId, int bookmarkId)
    {
        var bookmark = await _bookmarkService.GetBookmarkByIdAsync(bookmarkId, userId);
        if (bookmark == null)
            throw new NotFoundException("Bookmark ID is not found");
        var patchDto = new BookmarkPatchDto { IsArchived = !bookmark.IsArchived };
        await _bookmarkService.PatchBookmarkAsync(bookmarkId, userId, patchDto);
        return Ok(new
        {
            message = $"Bookmark {(bookmark.IsArchived ? "archived" : "unarchived")} successfully",
            isArchive = patchDto.IsArchived
        });
    }

    [HttpPost("{bookmarkId}/visits")]
    public async Task<ActionResult<Visit>> AddVisit(int userId, int bookmarkId)
    {
        var visit = await _bookmarkService.AddVisitToBookmarkAsync(bookmarkId, userId);
        return CreatedAtAction(
            nameof(AddVisit),
            new { bookmarkId = visit.BookmarkId, userId = userId },
            visit);
    }

}
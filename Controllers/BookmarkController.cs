using System.Runtime.CompilerServices;
using bookmark_manager_app.Models;
using bookmark_manager_app.Services;
using Microsoft.AspNetCore.Mvc;

namespace bookmark_manager_app.Controllers;

[ApiController]
[Route("/api/user/{userId}/[controller]")]
public class BookmarkController : ControllerBase
{
    private readonly IBookmarkService _bookmarkService;
    private readonly ILogger<BookmarkController> _logger;

    public BookmarkController(IBookmarkService bookmarkService, ILogger<BookmarkController> logger)
    {
        _bookmarkService = bookmarkService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Bookmark>>> GetBookmarkByUser(int userId)
    {
        try
        {
            _logger.LogInformation("Getting bookmarks for user {UserId}", userId);
            var bookmarks = await _bookmarkService.GetBookmarkAsync(userId);
            var response = new List<object>();
            foreach (var bookmark in bookmarks)
            {
                int visitCountBookmark = await _bookmarkService.GetVisitCountAsync(bookmark.BookmarkId);
                DateTime? lastVisitedBookmark = await _bookmarkService.GetLastVisitedAsync(bookmark.BookmarkId);
                response.Add(new
                {
                    id = bookmark.BookmarkId,
                    title = bookmark.Title,
                    url = bookmark.Url,
                    description = bookmark.Description,
                    pinned = bookmark.IsPinned,
                    isArchived = bookmark.IsArchived,
                    createdAt = bookmark.CreatedAt,
                    visitCount = visitCountBookmark,
                    lastVisited = lastVisitedBookmark,
                    tags = bookmark.BookmarkTags.Select(bt => bt.Tag?.Name)
                });
            }
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting bookmarks for user {UserId}", userId);
            return StatusCode(500, new ApiResponse
            {
                Success = false,
                Error = "Internal server error"
            });
        }
    }

    [HttpGet("{bookmarkId}")]
    public async Task<ActionResult<Bookmark>> GetBookmark(int userId, int bookmarkId)
    {
        try
        {
            _logger.LogInformation("Getting bookmark {BookmarkId} for user {UserId}", bookmarkId, userId);
            var bookmark = await _bookmarkService.GetBookmarkByIdAsync(bookmarkId);
            if (bookmark == null)
            {
                _logger.LogWarning("Bookmark {BookmarkId} not found", bookmarkId);
                return NotFound(new ApiResponse
                {
                    Success = false,
                    Error = $"Bookmark  with ID {bookmarkId} not found"
                });
            }
            if (bookmark.UserId != userId)
            {
                _logger.LogWarning("Bookmark {BookmarkId} does not belong to user {UserId}", bookmarkId, userId);
                return StatusCode(StatusCodes.Status403Forbidden,
                    new ApiResponse
                    {
                        Success = false,
                        Error = $"Bookmark  with ID {bookmarkId} does not belong to user {userId}"
                    }
                );
            }
            var response = new
            {
                id = bookmark.BookmarkId,
                title = bookmark.Title,
                url = bookmark.Url,
                description = bookmark.Description,
                tags = bookmark.BookmarkTags.Select(bt => bt.Tag?.Name)
            };
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting bookmark {BookmarkId}", bookmarkId);
            return StatusCode(500,
                new ApiResponse
                {
                    Success = false,
                    Error = "Internal server error"
                }
            );
        }
    }

    [HttpPost]
    public async Task<ActionResult<Bookmark>> CreateBookmark(int userId, BookmarkCreateDto bookmarkDto)
    {
        try
        {
            _logger.LogInformation("Creating bookmark for user {UserId}", userId);
            var bookmark = await _bookmarkService.CreateBookmarkAsync(userId, bookmarkDto);
            if (bookmark == null)
            {
                _logger.LogWarning("Failed to create bookmark for user {UserId}", userId);
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Error = "Failed to create bookmark"
                });
            }
            _logger.LogInformation("Bookmark created with ID {BookmarkId} for user {UserId}", bookmark.BookmarkId, userId);
            int visitCountBookmark = await _bookmarkService.GetVisitCountAsync(bookmark.BookmarkId);
            DateTime? lastVisitedBookmark = await _bookmarkService.GetLastVisitedAsync(bookmark.BookmarkId);
            return StatusCode(
               StatusCodes.Status201Created,
               new ApiResponse<object>
               {
                   Success = true,
                   Data = new
                   {
                       id = bookmark.BookmarkId,
                       title = bookmark.Title,
                       url = bookmark.Url,
                       description = bookmark.Description,
                       pinned = bookmark.IsPinned,
                       isArchived = bookmark.IsArchived,
                       createdAt = bookmark.CreatedAt,
                       visitCount = visitCountBookmark,
                       lastVisited = lastVisitedBookmark,
                       tags = bookmark.BookmarkTags.Select(bt => bt.Tag?.Name)
                   }
               }
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating bookmark for user {UserId}", userId);
            return StatusCode(500, new ApiResponse
            {
                Success = false,
                Error = "Internal server error"
            });
        }

    }

    [HttpPut("{bookmarkId}")]
    public async Task<IActionResult> UpdateBookmark(int userId, int bookmarkId, BookmarkUpdateDto bookmarkUpdate)
    {
        try
        {
            _logger.LogInformation("Updating bookmark {BookmarkId} for user {UserId}", bookmarkId, userId);
            // verify if bookmark exists and belongs to the user 
            var existingBookmark = await _bookmarkService.GetBookmarkByIdAsync(bookmarkId);
            if (existingBookmark == null)
            {
                _logger.LogWarning("Bookmark {BookmarkId} not found", bookmarkId);
                return NotFound(new ApiResponse
                {
                    Success = false,
                    Error = $"Bookmark with ID {bookmarkId} not found"
                });
            }
            if (existingBookmark.UserId != userId)
            {
                _logger.LogWarning("Bookmark {BookmarkId} does not belong to user {UserId}", bookmarkId, userId);
                return StatusCode(StatusCodes.Status403Forbidden,
                    new ApiResponse
                    {
                        Success = false,
                        Error = $"Bookmark  with ID {bookmarkId} does not belong to user {userId}"
                    }
                );
            }

            var success = await _bookmarkService.UpdateBookmarkAsync(bookmarkId, bookmarkUpdate);
            if (!success)
            {
                _logger.LogWarning("Failed to update bookmark {BookmarkId}", bookmarkId);
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Error = "Failed to update bookmark"
                });
            }

            _logger.LogInformation("Bookmark {BookmarkId} updated successfully", bookmarkId);
            return StatusCode(StatusCodes.Status204NoContent,
                new ApiResponse
                {
                    Success = true,
                }
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating bookmark {BookmarkId}", bookmarkId);
            return StatusCode(500, new ApiResponse
            {
                Success = false,
                Error = "Internal server error"
            });
        }
    }

    [HttpDelete("{bookmarkId}")]
    public async Task<IActionResult> DeleteBookmark(int userId, int bookmarkId)
    {
        try
        {
            _logger.LogInformation("Deleting bookmark {BookmarkId} for user {UserId}", bookmarkId, userId);
            // Verify bookmark exists and belongs to user
            var existingBookmark = await _bookmarkService.GetBookmarkByIdAsync(bookmarkId);
            if (existingBookmark == null)
            {
                _logger.LogWarning("Bookmark {BookmarkId} not found", bookmarkId);
                return NotFound(new ApiResponse
                {
                    Success = false,
                    Error = $"Bookmark with ID {bookmarkId} not found"
                });
            }
            if (existingBookmark.UserId != userId)
            {
                _logger.LogWarning("Bookmark {BookmarkId} does not belong to user {UserId}", bookmarkId, userId);
                return StatusCode(StatusCodes.Status403Forbidden,
                new ApiResponse
                {
                    Success = false,
                    Error = $"Bookmark {bookmarkId} does not belong to user {userId}"
                });
            }

            var success = await _bookmarkService.DeleteBookmarkAsync(bookmarkId);
            if (!success)
            {
                _logger.LogWarning("Failed to delete bookmark {BookmarkId}", bookmarkId);
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Error = "Failed to delete bookmark"
                });
            }
            _logger.LogInformation("Bookmark {BookmarkId} deleted successfully", bookmarkId);
            return StatusCode(StatusCodes.Status204NoContent, new ApiResponse { Success = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting bookmark {BookmarkId}", bookmarkId);
            return StatusCode(500, new ApiResponse { Success = false, Error = "Internal server error" });
        }
    }

    [HttpPatch("{bookmarkId}/pin")]
    public async Task<IActionResult> TogglePin(int userId, int bookmarkId)
    {
        try
        {
            _logger.LogInformation("Toggling pin for bookmark {BookmarkId}", bookmarkId);
            // verify bookmark exits and belongs to user
            var existingBookmark = await _bookmarkService.GetBookmarkByIdAsync(bookmarkId);
            if (existingBookmark == null)
            {
                _logger.LogWarning("Bookmark {BookmarkId} not found", bookmarkId);
                return NotFound(new ApiResponse
                {
                    Success = false,
                    Error = $"Bookmark with ID {bookmarkId} not found"
                });
            }
            if (existingBookmark.UserId != userId)
            {
                _logger.LogWarning("Bookmark {BookmarkId} does not belong to user {UserId}", bookmarkId, userId);
                return StatusCode(StatusCodes.Status403Forbidden,
                new ApiResponse
                {
                    Success = false,
                    Error = $"Bookmark {bookmarkId} does not belong to user {userId}"
                });
            }

            var success = await _bookmarkService.TogglePinAsync(bookmarkId);
            if (!success)
            {
                _logger.LogWarning("Failed to toggle pin for bookmark {BookmarkId}", bookmarkId);
                return BadRequest(new ApiResponse { Success = false, Error = "Failed to toggle pin" });
            }
            _logger.LogInformation("Pin toggled for bookmark {BookmarkId}", bookmarkId);
            return StatusCode(StatusCodes.Status204NoContent, new ApiResponse { Success = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error toggling pin for bookmark {BookmarkId}", bookmarkId);
            return StatusCode(500, new ApiResponse { Success = false, Error = "Internal server error" });
        }
    }

    [HttpPatch("{bookmarkId}/archive")]
    public async Task<IActionResult> ToggleArchive(int userId, int bookmarkId)
    {
        try
        {
            _logger.LogInformation("Toggling archive for bookmark {BookmarkId}", bookmarkId);
            // verify bookmark exits and belongs to user
            var existingBookmark = await _bookmarkService.GetBookmarkByIdAsync(bookmarkId);
            if (existingBookmark == null)
            {
                _logger.LogWarning("Bookmark {BookmarkId} not found", bookmarkId);
                return NotFound(new ApiResponse
                {
                    Success = false,
                    Error = $"Bookmark with ID {bookmarkId} not found"
                });
            }
            if (existingBookmark.UserId != userId)
            {
                _logger.LogWarning("Bookmark {BookmarkId} does not belong to user {UserId}", bookmarkId, userId);
                return StatusCode(StatusCodes.Status403Forbidden,
                new ApiResponse
                {
                    Success = false,
                    Error = $"Bookmark {bookmarkId} does not belong to user {userId}"
                });
            }

            var success = await _bookmarkService.ToggleArchiveAsync(bookmarkId);
            if (!success)
            {
                _logger.LogWarning("Failed to toggle archive for bookmark {BookmarkId}", bookmarkId);
                return BadRequest(new ApiResponse { Success = false, Error = "Failed to toggle archive" });
            }
            _logger.LogInformation("Archive toggled for bookmark {BookmarkId}", bookmarkId);
            return StatusCode(StatusCodes.Status204NoContent, new ApiResponse { Success = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error toggling archive for bookmark {BookmarkId}", bookmarkId);
            return StatusCode(500, new ApiResponse { Success = false, Error = "Internal server error" });
        }
    }

    [HttpPost("{bookmarkId}/visit")]
    public async Task<ActionResult<Visit>> AddVisit(int userId, int bookmarkId)
    {
        try
        {
            _logger.LogInformation("Adding visit for bookmark {BookmarkId}", bookmarkId);
            // verify bookmark exits and belongs to user
            var existingBookmark = await _bookmarkService.GetBookmarkByIdAsync(bookmarkId);
            if (existingBookmark == null)
            {
                _logger.LogWarning("Bookmark {BookmarkId} not found", bookmarkId);
                return NotFound(new ApiResponse
                {
                    Success = false,
                    Error = $"Bookmark with ID {bookmarkId} not found"
                });
            }
            if (existingBookmark.UserId != userId)
            {
                _logger.LogWarning("Bookmark {BookmarkId} does not belong to user {UserId}", bookmarkId, userId);
                return StatusCode(StatusCodes.Status403Forbidden,
                new ApiResponse
                {
                    Success = false,
                    Error = $"Bookmark {bookmarkId} does not belong to user {userId}"
                });
            }

            var visit = await _bookmarkService.AddVisitAsync(bookmarkId);
            if (visit == null)
            {
                _logger.LogWarning("Failed to add visit for bookmark {BookmarkId}", bookmarkId);
                return BadRequest(new ApiResponse { Success = false, Error = "Failed to add visit" });
            }
            _logger.LogInformation("Visit added for bookmark {BookmarkId}", bookmarkId);
            return Ok(new
            {
                visit.BookmarkId,
                visit.VisitDateAt
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding visit for bookmark {BookmarkId}", bookmarkId);
            return StatusCode(500, new ApiResponse { Success = false, Error = "Internal server error" });
        }
    }

}
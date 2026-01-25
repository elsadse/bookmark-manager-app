using bookmark_manager_app.Models;
using bookmark_manager_app.Services;
using Microsoft.AspNetCore.Mvc;

namespace bookmark_manager_app.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class TagController : ControllerBase
{
    private readonly ITagService _tagService;
    private readonly ILogger<TagController> _logger;

    public TagController(ITagService tagService, ILogger<TagController> logger)
    {
        _tagService = tagService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Tag>>> GetAllTags()
    {
        try
        {
            _logger.LogInformation("Getting all tags");
            var tags = await _tagService.GetAllTagsAsync();
            return Ok(tags);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all tags");
            return StatusCode(500, new ApiResponse { Success = false, Error = "Internal server error" });
        }
    }

    [HttpPost]
    public async Task<ActionResult<Tag>> CreateTag(TagCreateDto tagDto)
    {
        try
        {
            _logger.LogInformation("Creating tag: {Name}", tagDto.Name);
            var tag = await _tagService.CreateTagAsync(tagDto);
            if (tag == null)
            {
                _logger.LogWarning("Failed to create tag: {Name}", tagDto.Name);
                return BadRequest(new ApiResponse { Success = false, Error = "Failed to create tag" });
            }
            _logger.LogInformation("Tag created with ID: {TagId}", tag.TagId);
            return StatusCode(StatusCodes.Status201Created, new ApiResponse<Tag> { Success = true, Data = tag });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating tag: {Name}", tagDto.Name);
            return StatusCode(500, new ApiResponse { Success = false, Error = "Internal server error" });
        }
    }
}
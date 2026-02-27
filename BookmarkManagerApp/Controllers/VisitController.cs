using BookmarkManagerApp.Controllers.Requests;
using BookmarkManagerApp.Controllers.Responses;
using BookmarkManagerApp.Models;
using BookmarkManagerApp.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookmarkManagerApp.Controllers;

[ApiController]
[Authorize]
[Route("/api/visits")]
public class VisitController(VisitService visitService, IValidator<CreateVisitRequest> createVisitRequestValidator) : ControllerBase
{
    [HttpGet("{id:long}", Name = nameof(GetVisitByIdAsync))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TagResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<ActionResult<VisitResponse>> GetVisitByIdAsync(long id)
    {
        var visit = await visitService.GetByIdAsync(id);
        return Ok(new VisitResponse(visit.BookmarkId, visit.VisitTime));
    }
    
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(VisitResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ProblemDetails))]
    public async Task<ActionResult<VisitResponse>> CreateAsync(CreateVisitRequest request)
    {
        await createVisitRequestValidator.ValidateAndThrowAsync(request);
        
        var visit = await visitService.CreateAsync(new Visit{ BookmarkId = request.BookmarkId, VisitTime = request.VisitTime});
        return CreatedAtRoute(nameof(GetVisitByIdAsync), new { Id = visit.VisitId }, new VisitResponse(visit.BookmarkId, visit.VisitTime));
    }
}
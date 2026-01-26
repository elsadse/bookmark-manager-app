using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace bookmark_manager_app.Exceptions.Handlers;

public sealed class HandlerBadRequestException : IExceptionHandler
{
    private readonly ILogger<HandlerBadRequestException> _logger;
    public HandlerBadRequestException(ILogger<HandlerBadRequestException> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not BadRequestException badRequest)
        {
            return false;
        }
        _logger.LogWarning("Bad request: {Message}", badRequest.Message);
        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "Bad Request",
            Detail = badRequest.Message
        }, cancellationToken);
        return true;
    }
}
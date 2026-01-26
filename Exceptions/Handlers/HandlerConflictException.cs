using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace bookmark_manager_app.Exceptions.Handlers;

public sealed class HandlerConflictException : IExceptionHandler
{
    private readonly ILogger<HandlerConflictException> _logger;
    public HandlerConflictException(ILogger<HandlerConflictException> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not ConflictException conflict)
        {
            return false;
        }
        _logger.LogWarning("Conflict detected: {Message}", conflict.Message);
        httpContext.Response.StatusCode = StatusCodes.Status409Conflict;
        await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
        {
            Status = StatusCodes.Status409Conflict,
            Title = "Conflict",
            Detail = conflict.Message
        }, cancellationToken);
        return true;
    }
}
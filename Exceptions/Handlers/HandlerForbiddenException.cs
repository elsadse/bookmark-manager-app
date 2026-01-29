using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace bookmark_manager_app.Exceptions.Handler;

public sealed class HandlerForbiddenException : IExceptionHandler
{
    private readonly ILogger<HandlerForbiddenException> _logger;

    public HandlerForbiddenException(ILogger<HandlerForbiddenException> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not ForbiddenException forbid)
        {
            return false;
        }
        _logger.LogWarning("Forbidden: {Message}", forbid.Message);
        httpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
        await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
        {
            Status = StatusCodes.Status403Forbidden,
            Type = "Forbid",
            Detail = forbid.Message
        }, cancellationToken);

        return true;
    }
}
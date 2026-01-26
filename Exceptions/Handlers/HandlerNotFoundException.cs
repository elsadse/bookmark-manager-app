using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace bookmark_manager_app.Exceptions.Handlers;

public sealed class HandlerNotFoundException : IExceptionHandler
{
    private readonly ILogger<HandlerNotFoundException> _logger;
    public HandlerNotFoundException(ILogger<HandlerNotFoundException> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not NotFoundException notFound)
        {
            return false;
        }
        _logger.LogWarning("Ressource not found: {Message}", notFound.Message);
        httpContext.Response.StatusCode = StatusCodes.Status404NotFound;
        await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
        {
            Status = StatusCodes.Status404NotFound,
            Title = "Resource Not Found",
            Detail = notFound.Message
        }, cancellationToken);
        return true;
    }
}
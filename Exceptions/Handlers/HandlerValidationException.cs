using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace bookmark_manager_app.Exceptions.Handlers;

public sealed class HandlerValidationException : IExceptionHandler
{
    private readonly ILogger<HandlerValidationException> _logger;

    public HandlerValidationException(ILogger<HandlerValidationException> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not CustomValidationException validationException)
        {
            return false;
        }

        _logger.LogWarning("Validation Error: {Message}", validationException.Message);

        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        await httpContext.Response.WriteAsJsonAsync(new ValidationProblemDetails
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "Validation Error",
            Errors= validationException.Errors
        }, cancellationToken);

        return true;
    }
}
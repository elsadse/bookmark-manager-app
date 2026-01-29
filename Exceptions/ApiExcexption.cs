namespace bookmark_manager_app.Exceptions;

public abstract class ApiException : Exception
{
    protected int StatusCode { get; set; }

    public ApiException(int statusCode, string message) : base(message)
    {
        StatusCode = statusCode;
    }

    public ApiException(int statusCode, string message, Exception innerException) : base(message, innerException)
    {
        StatusCode = statusCode;
    }
}

public sealed class NotFoundException : ApiException
{
    public NotFoundException(string message) : base(StatusCodes.Status404NotFound, message)
    {
    }
}

public sealed class BadRequestException : ApiException
{
    public BadRequestException(string message) : base(StatusCodes.Status400BadRequest, message)
    {
    }
}

public sealed class ConflictException : ApiException
{
    public ConflictException(string message) : base(StatusCodes.Status409Conflict, message)
    {
    }
}

public sealed class ForbiddenException : ApiException
{
    public ForbiddenException(string message) : base(StatusCodes.Status403Forbidden, message)
    {
    }
}

public sealed class ValidationException : ApiException
{
    public IDictionary<string, string[]> Errors { get; }

    public ValidationException(IDictionary<string, string[]> errors) : base(StatusCodes.Status400BadRequest, "VALIDATION_ERROR")
    {
        Errors = errors;
    }
}

namespace bookmark_manager_app.Exceptions;

public abstract class ApiException(string message, int httpStatusCode) : Exception
{
    public int HttpStatusCode { get; } = httpStatusCode;
}


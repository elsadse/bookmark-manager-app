namespace bookmark_manager_app.Models;

public class ApiResponse
{
    public bool Success { get; set; }

    public string? Error { get; set; }

    public ApiResponse() { }

    public ApiResponse(bool success)
    {
        Success = success;
    }

    public ApiResponse(bool success, string error)
    {
        Success = success;
        Error = error;
    }
}

public class ApiResponse<T> : ApiResponse
{
    public T? Data { get; set; }

    public ApiResponse() { }

    public ApiResponse(bool success, T data)
    {
        Success = success;
        Data = data;
    }

    public ApiResponse(bool success, string error, T data)
    {
        Success = success;
        Error = error;
        Data = data;
    }

}

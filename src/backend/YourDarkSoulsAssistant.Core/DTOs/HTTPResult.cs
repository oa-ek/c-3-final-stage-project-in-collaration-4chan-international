namespace YourDarkSoulsAssistant.Core.DTOs;

public class HTTPResult<T>
{
    public bool IsSuccess { get; private set; }
    public string? ErrorMessage { get; private set; }
    public T? Data { get; private set; }
    
    private HTTPResult(bool isSuccess, T? data, string? errorMessage)
    {
        IsSuccess = isSuccess;
        Data = data;
        ErrorMessage = errorMessage;
    }
    
    public static HTTPResult<T> Success(T data) => new HTTPResult<T>(true, data, null);
    
    public static HTTPResult<T> Failure(string errorMessage) => new HTTPResult<T>(false, default, errorMessage);
}

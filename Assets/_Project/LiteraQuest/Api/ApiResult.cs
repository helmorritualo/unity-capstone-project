public sealed class ApiResult<T>
{
    public bool IsSuccess { get; }
    public T Data { get; }
    public string ErrorMessage { get; }
    public long StatusCode { get; }

    private ApiResult(bool isSuccess, T data, string errorMessage, long statusCode)
    {
        IsSuccess = isSuccess;
        Data = data;
        ErrorMessage = errorMessage;
        StatusCode = statusCode;
    }

    public static ApiResult<T> Success(T data, long statusCode)
    {
        return new ApiResult<T>(true, data, null, statusCode);
    }

    public static ApiResult<T> Failure(string errorMessage, long statusCode)
    {
        return new ApiResult<T>(false, default, errorMessage, statusCode);
    }
}
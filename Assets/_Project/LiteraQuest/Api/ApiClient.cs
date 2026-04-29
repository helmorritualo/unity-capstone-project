using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public sealed class ApiClient
{
    private readonly ApiConfig config;
    private readonly AuthTokenStore tokenStore;
    private readonly JsonSerializerService jsonSerializer;

    public ApiClient(
        ApiConfig config,
        AuthTokenStore tokenStore,
        JsonSerializerService jsonSerializer
    )
    {
        this.config = config;
        this.tokenStore = tokenStore;
        this.jsonSerializer = jsonSerializer;
    }

    public IEnumerator Get<TResponse>(
        string endpoint,
        Action<ApiResult<TResponse>> callback
    )
    {
        using UnityWebRequest request = UnityWebRequest.Get(BuildUrl(endpoint));

        ApplyHeaders(request);

        yield return Send<TResponse>(request, callback);
    }

    public IEnumerator Post<TRequest, TResponse>(
        string endpoint,
        TRequest body,
        Action<ApiResult<TResponse>> callback
    )
    {
        string json = jsonSerializer.Serialize(body);
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

        using UnityWebRequest request = new UnityWebRequest(
            BuildUrl(endpoint),
            UnityWebRequest.kHttpVerbPOST
        );

        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();

        ApplyHeaders(request);

        yield return Send<TResponse>(request, callback);
    }

    private IEnumerator Send<TResponse>(
        UnityWebRequest request,
        Action<ApiResult<TResponse>> callback
    )
    {
        request.timeout = config.TimeoutSeconds;

        yield return request.SendWebRequest();

        ApiResult<TResponse> result = ParseResponse<TResponse>(request);
        callback?.Invoke(result);
    }

    private string BuildUrl(string endpoint)
    {
        return $"{config.BaseUrl}/{endpoint.TrimStart('/')}";
    }

    private void ApplyHeaders(UnityWebRequest request)
    {
        request.SetRequestHeader("Accept", "application/json");
        request.SetRequestHeader("Content-Type", "application/json");

        if (tokenStore.HasToken)
        {
            request.SetRequestHeader("Authorization", $"Bearer {tokenStore.Token}");
        }
    }

    private ApiResult<TResponse> ParseResponse<TResponse>(UnityWebRequest request)
    {
        string responseText = request.downloadHandler?.text;

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(
       $"API request failed\n" +
       $"URL: {request.url}\n" +
       $"Status: {request.responseCode}\n" +
       $"Result: {request.result}\n" +
       $"Response: {responseText}"
   );
            string errorMessage = ParseErrorMessage(responseText);
            return ApiResult<TResponse>.Failure(errorMessage, request.responseCode);
        }

        if (string.IsNullOrWhiteSpace(responseText))
        {
            return ApiResult<TResponse>.Success(default, request.responseCode);
        }

        try
        {
            TResponse data = jsonSerializer.Deserialize<TResponse>(responseText);
            return ApiResult<TResponse>.Success(data, request.responseCode);
        }
        catch (Exception exception)
        {
            Debug.LogError($"Failed to parse API response: {exception.Message}");
            return ApiResult<TResponse>.Failure(
                "The server returned an invalid response.",
                request.responseCode
            );
        }
    }

    private string ParseErrorMessage(string responseText)
    {
        if (string.IsNullOrWhiteSpace(responseText))
        {
            return "Network error. Please check your connection.";
        }

        try
        {
            ApiError error = jsonSerializer.Deserialize<ApiError>(responseText);

            if (!string.IsNullOrWhiteSpace(error?.message))
            {
                return error.message;
            }
        }
        catch
        {
            // Ignore parse failure and return generic error below.
        }

        return "Something went wrong. Please try again.";
    }
}
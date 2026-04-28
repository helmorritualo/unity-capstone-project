using System.Text;
using System.Collections;
using Newtonsoft.Json;
using UnityEngine.Networking;
public sealed class ApiClient
{
    private readonly ApiConfig config;
    private readonly AuthTokenStore tokenStore;

    public ApiClient(ApiConfig config, AuthTokenStore tokenStore)
    {
        this.config = config;
        this.tokenStore = tokenStore;
    }

    public IEnumerator Get<T>(string path, System.Action<ApiResult<T>> callback)
    {
        using var request = UnityWebRequest.Get(BuildUrl(path));
        ApplyHeaders(request);

        yield return request.SendWebRequest();

        callback(ParseResponse<T>(request));
    }

    public IEnumerator Post<TRequest, TResponse>(
        string path,
        TRequest body,
        System.Action<ApiResult<TResponse>> callback
    )
    {
        string json = JsonConvert.SerializeObject(body);

        using var request = new UnityWebRequest(BuildUrl(path), UnityWebRequest.kHttpVerbPOST);
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();

        ApplyHeaders(request);

        yield return request.SendWebRequest();

        callback(ParseResponse<TResponse>(request));
    }

    private string BuildUrl(string path)
    {
        return $"{config.BaseUrl}/{path.TrimStart('/')}";
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

    private ApiResult<T> ParseResponse<T>(UnityWebRequest request)
    {
        string responseText = request.downloadHandler?.text;

        if (request.result != UnityWebRequest.Result.Success)
        {
            string message = TryParseErrorMessage(responseText);
            return ApiResult<T>.Failure(message, request.responseCode);
        }

        T data = JsonConvert.DeserializeObject<T>(responseText);
        return ApiResult<T>.Success(data, request.responseCode);
    }

    private string TryParseErrorMessage(string json)
    {
        if (string.IsNullOrWhiteSpace(json))
        {
            return "Network error. Please try again.";
        }

        try
        {
            ApiError error = JsonConvert.DeserializeObject<ApiError>(json);
            return string.IsNullOrWhiteSpace(error.message)
                ? "Something went wrong."
                : error.message;
        }
        catch
        {
            return "Something went wrong.";
        }
    }
}
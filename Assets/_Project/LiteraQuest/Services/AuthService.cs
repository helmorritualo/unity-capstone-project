using System.Collections;

public sealed class AuthService
{
    private readonly ApiClient apiClient;
    private readonly AuthTokenStore tokenStore;

    public AuthService(ApiClient apiClient, AuthTokenStore tokenStore)
    {
        this.apiClient = apiClient;
        this.tokenStore = tokenStore;
    }

    public IEnumerator Login(
        string lrn,
        string pin,
        System.Action<ApiResult<StudentLoginResponse>> callback
    )
    {
        var request = new StudentLoginRequest(lrn, pin);

        yield return apiClient.Post<StudentLoginRequest, StudentLoginResponse>(
            "/student/auth/login",
            request,
            result =>
            {
                if (result.IsSuccess && result.Data != null)
                {
                    tokenStore.Save(result.Data.token);
                }

                callback?.Invoke(result);
            }
        );
    }

    public IEnumerator Logout(System.Action<ApiResult<ApiMessageResponse>> callback)
    {
        yield return apiClient.Post<object, ApiMessageResponse>(
            "/student/auth/logout",
            new { },
            result =>
            {
                if (result.IsSuccess && result.Data != null)
                {
                    tokenStore.Clear();
                }

                callback?.Invoke(result);
            }
        );
    }
}
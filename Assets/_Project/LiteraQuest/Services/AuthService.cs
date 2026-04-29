using System;
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
        Action<ApiResult<StudentLoginResponse>> callback
    )
    {
        StudentLoginRequest request = new StudentLoginRequest(lrn, pin);

        yield return apiClient.Post<StudentLoginRequest, StudentLoginResponse>(
            ApiEndpoint.StudentLogin,
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

    public IEnumerator Logout(Action<ApiResult<ApiMessageResponse>> callback)
    {
        yield return apiClient.Post<object, ApiMessageResponse>(
            ApiEndpoint.StudentLogout,
            new { },
            result =>
            {
                tokenStore.Clear();
                callback?.Invoke(result);
            }
        );
    }

    public void ClearLocalSession()
    {
        tokenStore.Clear();
    }
}
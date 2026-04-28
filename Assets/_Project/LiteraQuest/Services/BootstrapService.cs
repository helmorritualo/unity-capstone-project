using System;
using System.Collections;

public sealed class BootstrapService
{
    private readonly ApiClient apiClient;

    public BootstrapService(ApiClient apiClient)
    {
        this.apiClient = apiClient;
    }

    public IEnumerator LoadBootstrap(Action<ApiResult<BootstrapResponse>> callback)
    {
        yield return apiClient.Get<BootstrapResponse>("/student/bootstrap", callback);
    }
}
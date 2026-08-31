using GeminiDotnet.V1Beta.Environments;

namespace GeminiDotnet.V1Beta;

internal sealed class EnvironmentsClient : IEnvironmentsClient
{
    private readonly IGeminiRequester _requester;
    
    internal EnvironmentsClient(IGeminiRequester requester)
    {
        ArgumentNullException.ThrowIfNull(requester);
        _requester = requester;
    }

    public Task<HttpBody> ListEnvironmentsHttpAsync(
        int? pageSize = null,
        string? pageToken = null,
        CancellationToken cancellationToken = default)
    {
        var query = new QueryStringBuilder()
            .Add("pageSize", pageSize)
            .Add("pageToken", pageToken)
            .ToString();
        var path = $"/v1beta/environments{query}";
        return _requester.ExecuteAsync<HttpBody>(HttpMethod.Get, path, cancellationToken);
    }

    public Task<HttpBody> CreateEnvironmentHttpAsync(
        HttpBody request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        const string path = "/v1beta/environments";
        return _requester.ExecuteAsync<HttpBody, HttpBody>(HttpMethod.Post, path, request, cancellationToken);
    }

    public Task<GetEnvironmentFilesResponse> GetEnvironmentFilesHttpByEnvironmentAsync(
        string environment,
        string? path = null,
        bool? recursive = null,
        int? pageSize = null,
        string? pageToken = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(environment);
        var query = new QueryStringBuilder()
            .Add("path", path)
            .Add("recursive", recursive)
            .Add("page_size", pageSize)
            .Add("page_token", pageToken)
            .ToString();
        var requestPath = $"/v1beta/environments/{Uri.EscapeDataString(environment)}/files{query}";
        return _requester.ExecuteAsync<GetEnvironmentFilesResponse>(HttpMethod.Get, requestPath, cancellationToken);
    }

    public Task<GetEnvironmentFilesResponse> GetEnvironmentFilesHttpAsync(
        string environment,
        string path,
        bool? recursive = null,
        int? pageSize = null,
        string? pageToken = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(environment);
        ArgumentNullException.ThrowIfNull(path);
        var query = new QueryStringBuilder()
            .Add("recursive", recursive)
            .Add("page_size", pageSize)
            .Add("page_token", pageToken)
            .ToString();
        var requestPath = $"/v1beta/environments/{Uri.EscapeDataString(environment)}/files/{WildcardPath.Escape(path)}{query}";
        return _requester.ExecuteAsync<GetEnvironmentFilesResponse>(HttpMethod.Get, requestPath, cancellationToken);
    }

    public Task<HttpBody> GetEnvironmentHttpAsync(
        string id,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(id);
        var path = $"/v1beta/environments/{Uri.EscapeDataString(id)}";
        return _requester.ExecuteAsync<HttpBody>(HttpMethod.Get, path, cancellationToken);
    }

    public Task<HttpBody> DeleteEnvironmentHttpAsync(
        string id,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(id);
        var path = $"/v1beta/environments/{Uri.EscapeDataString(id)}";
        return _requester.ExecuteAsync<HttpBody>(HttpMethod.Delete, path, cancellationToken);
    }

    public Task<Empty> DeleteEnvironmentAsync(
        string id,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(id);
        var path = $"/v1beta/environments/{Uri.EscapeDataString(id)}:delete";
        return _requester.ExecuteAsync<Empty>(HttpMethod.Delete, path, cancellationToken);
    }

    public Task<Environment> GetEnvironmentAsync(
        string id,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(id);
        var path = $"/v1beta/environments/{Uri.EscapeDataString(id)}:get";
        return _requester.ExecuteAsync<Environment>(HttpMethod.Get, path, cancellationToken);
    }

}

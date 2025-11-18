using GeminiDotnet.V1Beta.CachedContents;

namespace GeminiDotnet.V1Beta;

internal sealed class CachedContentsClient : ICachedContentsClient
{
    private readonly IGeminiRequester _requester;
    
    internal CachedContentsClient(IGeminiRequester requester)
    {
        ArgumentNullException.ThrowIfNull(requester);
        _requester = requester;
    }

    public Task<ListCachedContentsResponse> ListCachedContentsAsync(
        int? pageSize = null,
        string? pageToken = null,
        CancellationToken cancellationToken = default)
    {
        const string path = "/v1beta/cachedContents";
        return _requester.ExecuteAsync<ListCachedContentsResponse>(HttpMethod.Get, path, cancellationToken);
    }

    public Task<CachedContent> CreateCachedContentAsync(
        CachedContent request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        const string path = "/v1beta/cachedContents";
        return _requester.ExecuteAsync<CachedContent, CachedContent>(HttpMethod.Post, path, request, cancellationToken);
    }

    public Task<CachedContent> GetCachedContentAsync(
        string id,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(id);
        var path = $"/v1beta/cachedContents/{id}";
        return _requester.ExecuteAsync<CachedContent>(HttpMethod.Get, path, cancellationToken);
    }

    public Task<Empty> DeleteCachedContentAsync(
        string id,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(id);
        var path = $"/v1beta/cachedContents/{id}";
        return _requester.ExecuteAsync<Empty>(HttpMethod.Delete, path, cancellationToken);
    }

    public Task<CachedContent> UpdateCachedContentAsync(
        string id,
        CachedContent request,
        string? updateMask = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(id);
        ArgumentNullException.ThrowIfNull(request);
        var path = $"/v1beta/cachedContents/{id}";
        return _requester.ExecuteAsync<CachedContent, CachedContent>(HttpMethod.Patch, path, request, cancellationToken);
    }

}

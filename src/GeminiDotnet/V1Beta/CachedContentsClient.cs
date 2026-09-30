using System.Net.Http.Json;
using System.Net.ServerSentEvents;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using GeminiDotnet.V1Beta.CachedContents;

namespace GeminiDotnet.V1Beta;

internal sealed partial class CachedContentsClient : ICachedContentsClient
{
    private readonly IGeminiRequester _requester;
    
    internal CachedContentsClient(IGeminiRequester requester)
    {
        ArgumentNullException.ThrowIfNull(requester);
        _requester = requester;
    }

    public Task<ListCachedContentsResponse> ListAsync(
        int? pageSize = null,
        string? pageToken = null,
        CancellationToken cancellationToken = default)
    {
        var query = new QueryStringBuilder()
            .Add("pageSize", pageSize)
            .Add("pageToken", pageToken)
            .ToString();
        var path = $"/v1beta/cachedContents{query}";
        return _requester.ExecuteAsync<ListCachedContentsResponse>(HttpMethod.Get, path, cancellationToken);
    }

    public Task<CachedContent> CreateAsync(
        CachedContent request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        const string path = "/v1beta/cachedContents";
        return _requester.ExecuteAsync<CachedContent, CachedContent>(HttpMethod.Post, path, request, cancellationToken);
    }

    public Task<CachedContent> GetAsync(
        string name,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(name);
        if (name.Split('/') is not ["cachedContents", { Length: > 0 }])
        {
            throw new ArgumentException($"'{name}' is not a resource name of the form 'cachedContents/{{cachedContentId}}'.", nameof(name));
        }
        var path = $"/v1beta/{WildcardPath.Escape(name)}";
        return _requester.ExecuteAsync<CachedContent>(HttpMethod.Get, path, cancellationToken);
    }

    public Task<Empty> DeleteAsync(
        string name,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(name);
        if (name.Split('/') is not ["cachedContents", { Length: > 0 }])
        {
            throw new ArgumentException($"'{name}' is not a resource name of the form 'cachedContents/{{cachedContentId}}'.", nameof(name));
        }
        var path = $"/v1beta/{WildcardPath.Escape(name)}";
        return _requester.ExecuteAsync<Empty>(HttpMethod.Delete, path, cancellationToken);
    }

    public Task<CachedContent> PatchAsync(
        string name,
        CachedContent request,
        string? updateMask = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(name);
        if (name.Split('/') is not ["cachedContents", { Length: > 0 }])
        {
            throw new ArgumentException($"'{name}' is not a resource name of the form 'cachedContents/{{cachedContentId}}'.", nameof(name));
        }
        ArgumentNullException.ThrowIfNull(request);
        var query = new QueryStringBuilder()
            .Add("updateMask", updateMask)
            .ToString();
        var path = $"/v1beta/{WildcardPath.Escape(name)}{query}";
        return _requester.ExecuteAsync<CachedContent, CachedContent>(HttpMethod.Patch, path, request, cancellationToken);
    }

}

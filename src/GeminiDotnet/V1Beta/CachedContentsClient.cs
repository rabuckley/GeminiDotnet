using System.Net.Http.Json;
using System.Net.ServerSentEvents;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using GeminiDotnet.V1Beta.AuthTokens;
using GeminiDotnet.V1Beta.CachedContents;
using GeminiDotnet.V1Beta.Corpora;
using GeminiDotnet.V1Beta.Environments;
using GeminiDotnet.V1Beta.EnvironmentsCreate;
using GeminiDotnet.V1Beta.EnvironmentsList;
using GeminiDotnet.V1Beta.Files;
using GeminiDotnet.V1Beta.FileSearchStores;
using GeminiDotnet.V1Beta.FilesRegister;
using GeminiDotnet.V1Beta.GeneratedFiles;
using GeminiDotnet.V1Beta.Models;
using GeminiDotnet.V1Beta.TunedModels;
using File = GeminiDotnet.V1Beta.Files.File;

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
        var query = new QueryStringBuilder()
            .Add("pageSize", pageSize)
            .Add("pageToken", pageToken)
            .ToString();
        var path = $"/v1beta/cachedContents{query}";
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
        var path = $"/v1beta/cachedContents/{Uri.EscapeDataString(id)}";
        return _requester.ExecuteAsync<CachedContent>(HttpMethod.Get, path, cancellationToken);
    }

    public Task<Empty> DeleteCachedContentAsync(
        string id,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(id);
        var path = $"/v1beta/cachedContents/{Uri.EscapeDataString(id)}";
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
        var query = new QueryStringBuilder()
            .Add("updateMask", updateMask)
            .ToString();
        var path = $"/v1beta/cachedContents/{Uri.EscapeDataString(id)}{query}";
        return _requester.ExecuteAsync<CachedContent, CachedContent>(HttpMethod.Patch, path, request, cancellationToken);
    }

}

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
using Environment = GeminiDotnet.V1Beta.EnvironmentsList.Environment;
using File = GeminiDotnet.V1Beta.Files.File;

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
        const string path = "/v1beta/environments";
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

    public Task<HttpBody> GetEnvironmentHttpAsync(
        string id,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(id);
        var path = $"/v1beta/environments/{id}";
        return _requester.ExecuteAsync<HttpBody>(HttpMethod.Get, path, cancellationToken);
    }

    public Task<HttpBody> DeleteEnvironmentHttpAsync(
        string id,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(id);
        var path = $"/v1beta/environments/{id}";
        return _requester.ExecuteAsync<HttpBody>(HttpMethod.Delete, path, cancellationToken);
    }

    public Task<Empty> DeleteEnvironmentAsync(
        string id,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(id);
        var path = $"/v1beta/environments/{id}:delete";
        return _requester.ExecuteAsync<Empty>(HttpMethod.Delete, path, cancellationToken);
    }

    public Task<Environment> GetEnvironmentAsync(
        string id,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(id);
        var path = $"/v1beta/environments/{id}:get";
        return _requester.ExecuteAsync<Environment>(HttpMethod.Get, path, cancellationToken);
    }

}

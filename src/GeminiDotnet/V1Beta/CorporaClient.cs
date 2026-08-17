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

internal sealed class CorporaClient : ICorporaClient
{
    private readonly IGeminiRequester _requester;
    
    internal CorporaClient(IGeminiRequester requester)
    {
        ArgumentNullException.ThrowIfNull(requester);
        _requester = requester;
    }

    public Task<ListCorporaResponse> ListCorporaAsync(
        int? pageSize = null,
        string? pageToken = null,
        CancellationToken cancellationToken = default)
    {
        const string path = "/v1beta/corpora";
        return _requester.ExecuteAsync<ListCorporaResponse>(HttpMethod.Get, path, cancellationToken);
    }

    public Task<Corpus> CreateCorpusAsync(
        Corpus request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        const string path = "/v1beta/corpora";
        return _requester.ExecuteAsync<Corpus, Corpus>(HttpMethod.Post, path, request, cancellationToken);
    }

    public Task<Corpus> GetCorpusAsync(
        string corpus,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(corpus);
        var path = $"/v1beta/corpora/{corpus}";
        return _requester.ExecuteAsync<Corpus>(HttpMethod.Get, path, cancellationToken);
    }

    public Task<Empty> DeleteCorpusAsync(
        string corpus,
        bool? force = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(corpus);
        var path = $"/v1beta/corpora/{corpus}";
        return _requester.ExecuteAsync<Empty>(HttpMethod.Delete, path, cancellationToken);
    }

    public Task<Operation> GetOperationByCorpusAndOperationAsync(
        string corpus,
        string operation,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(corpus);
        ArgumentNullException.ThrowIfNull(operation);
        var path = $"/v1beta/corpora/{corpus}/operations/{operation}";
        return _requester.ExecuteAsync<Operation>(HttpMethod.Get, path, cancellationToken);
    }

    public Task<ListPermissionsResponse> ListPermissionsByCorpusAsync(
        string corpus,
        int? pageSize = null,
        string? pageToken = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(corpus);
        var path = $"/v1beta/corpora/{corpus}/permissions";
        return _requester.ExecuteAsync<ListPermissionsResponse>(HttpMethod.Get, path, cancellationToken);
    }

    public Task<Permission> CreatePermissionByCorpusAsync(
        string corpus,
        Permission request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(corpus);
        ArgumentNullException.ThrowIfNull(request);
        var path = $"/v1beta/corpora/{corpus}/permissions";
        return _requester.ExecuteAsync<Permission, Permission>(HttpMethod.Post, path, request, cancellationToken);
    }

    public Task<Permission> GetPermissionByCorpusAndPermissionAsync(
        string corpus,
        string permission,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(corpus);
        ArgumentNullException.ThrowIfNull(permission);
        var path = $"/v1beta/corpora/{corpus}/permissions/{permission}";
        return _requester.ExecuteAsync<Permission>(HttpMethod.Get, path, cancellationToken);
    }

    public Task<Empty> DeletePermissionByCorpusAndPermissionAsync(
        string corpus,
        string permission,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(corpus);
        ArgumentNullException.ThrowIfNull(permission);
        var path = $"/v1beta/corpora/{corpus}/permissions/{permission}";
        return _requester.ExecuteAsync<Empty>(HttpMethod.Delete, path, cancellationToken);
    }

    public Task<Permission> UpdatePermissionByCorpusAndPermissionAsync(
        string corpus,
        string permission,
        Permission request,
        string updateMask,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(corpus);
        ArgumentNullException.ThrowIfNull(permission);
        ArgumentNullException.ThrowIfNull(request);
        var path = $"/v1beta/corpora/{corpus}/permissions/{permission}";
        return _requester.ExecuteAsync<Permission, Permission>(HttpMethod.Patch, path, request, cancellationToken);
    }

}

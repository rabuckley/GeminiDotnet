using System.Net.Http.Json;
using System.Net.ServerSentEvents;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using GeminiDotnet.V1.FileSearchStores;

namespace GeminiDotnet.V1;

internal sealed partial class FileSearchStoresClient : IFileSearchStoresClient
{
    private readonly IGeminiRequester _requester;
    
    internal FileSearchStoresClient(IGeminiRequester requester)
    {
        ArgumentNullException.ThrowIfNull(requester);
        _requester = requester;
    }

    public IFileSearchStoresDocumentsClient Documents => field ??= new FileSearchStoresDocumentsClient(_requester);

    public IFileSearchStoresOperationsClient Operations => field ??= new FileSearchStoresOperationsClient(_requester);

    public IFileSearchStoresUploadClient Upload => field ??= new FileSearchStoresUploadClient(_requester);

    public Task<ListFileSearchStoresResponse> ListAsync(
        int? pageSize = null,
        string? pageToken = null,
        CancellationToken cancellationToken = default)
    {
        var query = new QueryStringBuilder()
            .Add("pageSize", pageSize)
            .Add("pageToken", pageToken)
            .ToString();
        var path = $"/v1/fileSearchStores{query}";
        return _requester.ExecuteAsync<ListFileSearchStoresResponse>(HttpMethod.Get, path, cancellationToken);
    }

    public Task<FileSearchStore> CreateAsync(
        FileSearchStore request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        const string path = "/v1/fileSearchStores";
        return _requester.ExecuteAsync<FileSearchStore, FileSearchStore>(HttpMethod.Post, path, request, cancellationToken);
    }

    public Task<FileSearchStore> GetAsync(
        string name,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(name);
        if (name.Split('/') is not ["fileSearchStores", { Length: > 0 }])
        {
            throw new ArgumentException($"'{name}' is not a resource name of the form 'fileSearchStores/{{fileSearchStoreId}}'.", nameof(name));
        }
        var path = $"/v1/{WildcardPath.Escape(name)}";
        return _requester.ExecuteAsync<FileSearchStore>(HttpMethod.Get, path, cancellationToken);
    }

    public Task<Empty> DeleteAsync(
        string name,
        bool? force = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(name);
        if (name.Split('/') is not ["fileSearchStores", { Length: > 0 }])
        {
            throw new ArgumentException($"'{name}' is not a resource name of the form 'fileSearchStores/{{fileSearchStoreId}}'.", nameof(name));
        }
        var query = new QueryStringBuilder()
            .Add("force", force)
            .ToString();
        var path = $"/v1/{WildcardPath.Escape(name)}{query}";
        return _requester.ExecuteAsync<Empty>(HttpMethod.Delete, path, cancellationToken);
    }

    public Task<ImportFileOperation> ImportFileAsync(
        string fileSearchStoreName,
        ImportFileRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(fileSearchStoreName);
        if (fileSearchStoreName.Split('/') is not ["fileSearchStores", { Length: > 0 }])
        {
            throw new ArgumentException($"'{fileSearchStoreName}' is not a resource name of the form 'fileSearchStores/{{fileSearchStoreId}}'.", nameof(fileSearchStoreName));
        }
        ArgumentNullException.ThrowIfNull(request);
        var path = $"/v1/{WildcardPath.Escape(fileSearchStoreName)}:importFile";
        return _requester.ExecuteAsync<ImportFileRequest, ImportFileOperation>(HttpMethod.Post, path, request, cancellationToken);
    }

}

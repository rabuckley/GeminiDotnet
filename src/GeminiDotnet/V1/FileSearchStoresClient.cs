using System.Net.Http.Json;
using System.Net.ServerSentEvents;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using GeminiDotnet.V1.Files;
using GeminiDotnet.V1.FileSearchStores;
using GeminiDotnet.V1.FilesRegister;
using GeminiDotnet.V1.Models;
using GeminiDotnet.V1.TunedModels;
using File = GeminiDotnet.V1.Files.File;

namespace GeminiDotnet.V1;

internal sealed class FileSearchStoresClient : IFileSearchStoresClient
{
    private readonly IGeminiRequester _requester;
    
    internal FileSearchStoresClient(IGeminiRequester requester)
    {
        ArgumentNullException.ThrowIfNull(requester);
        _requester = requester;
    }

    public Task<ListFileSearchStoresResponse> ListFileSearchStoresAsync(
        int? pageSize = null,
        string? pageToken = null,
        CancellationToken cancellationToken = default)
    {
        const string path = "/v1/fileSearchStores";
        return _requester.ExecuteAsync<ListFileSearchStoresResponse>(HttpMethod.Get, path, cancellationToken);
    }

    public Task<FileSearchStore> CreateFileSearchStoreAsync(
        FileSearchStore request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        const string path = "/v1/fileSearchStores";
        return _requester.ExecuteAsync<FileSearchStore, FileSearchStore>(HttpMethod.Post, path, request, cancellationToken);
    }

    public Task<FileSearchStore> GetFileSearchStoreAsync(
        string fileSearchStore,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(fileSearchStore);
        var path = $"/v1/fileSearchStores/{fileSearchStore}";
        return _requester.ExecuteAsync<FileSearchStore>(HttpMethod.Get, path, cancellationToken);
    }

    public Task<Empty> DeleteFileSearchStoreAsync(
        string fileSearchStore,
        bool? force = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(fileSearchStore);
        var path = $"/v1/fileSearchStores/{fileSearchStore}";
        return _requester.ExecuteAsync<Empty>(HttpMethod.Delete, path, cancellationToken);
    }

    public Task<ImportFileOperation> ImportFileAsync(
        string fileSearchStore,
        ImportFileRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(fileSearchStore);
        ArgumentNullException.ThrowIfNull(request);
        var path = $"/v1/fileSearchStores/{fileSearchStore}:importFile";
        return _requester.ExecuteAsync<ImportFileRequest, ImportFileOperation>(HttpMethod.Post, path, request, cancellationToken);
    }

    public Task<UploadToFileSearchStoreOperation> UploadToFileSearchStoreAsync(
        string fileSearchStore,
        UploadToFileSearchStoreRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(fileSearchStore);
        ArgumentNullException.ThrowIfNull(request);
        var path = $"/v1/fileSearchStores/{fileSearchStore}:uploadToFileSearchStore";
        return _requester.ExecuteAsync<UploadToFileSearchStoreRequest, UploadToFileSearchStoreOperation>(HttpMethod.Post, path, request, cancellationToken);
    }

    public Task<ListDocumentsResponse> ListDocumentsAsync(
        string fileSearchStore,
        int? pageSize = null,
        string? pageToken = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(fileSearchStore);
        var path = $"/v1/fileSearchStores/{fileSearchStore}/documents";
        return _requester.ExecuteAsync<ListDocumentsResponse>(HttpMethod.Get, path, cancellationToken);
    }

    public Task<Document> GetDocumentAsync(
        string fileSearchStore,
        string document,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(fileSearchStore);
        ArgumentNullException.ThrowIfNull(document);
        var path = $"/v1/fileSearchStores/{fileSearchStore}/documents/{document}";
        return _requester.ExecuteAsync<Document>(HttpMethod.Get, path, cancellationToken);
    }

    public Task<Empty> DeleteDocumentAsync(
        string fileSearchStore,
        string document,
        bool? force = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(fileSearchStore);
        ArgumentNullException.ThrowIfNull(document);
        var path = $"/v1/fileSearchStores/{fileSearchStore}/documents/{document}";
        return _requester.ExecuteAsync<Empty>(HttpMethod.Delete, path, cancellationToken);
    }

    public Task<Operation> GetOperationByFileSearchStoreAndOperationAsync(
        string fileSearchStore,
        string operation,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(fileSearchStore);
        ArgumentNullException.ThrowIfNull(operation);
        var path = $"/v1/fileSearchStores/{fileSearchStore}/operations/{operation}";
        return _requester.ExecuteAsync<Operation>(HttpMethod.Get, path, cancellationToken);
    }

    public Task<DownloadMediaResponse> DownloadMediaAsync(
        string fileSearchStoresId,
        string mediaId,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(fileSearchStoresId);
        ArgumentNullException.ThrowIfNull(mediaId);
        var path = $"/v1/fileSearchStores/{fileSearchStoresId}/media/{mediaId}";
        return _requester.ExecuteAsync<DownloadMediaResponse>(HttpMethod.Get, path, cancellationToken);
    }

    public Task<Operation> GetOperationByFileSearchStoresIdAndOperationsIdAsync(
        string fileSearchStoresId,
        string operationsId,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(fileSearchStoresId);
        ArgumentNullException.ThrowIfNull(operationsId);
        var path = $"/v1/fileSearchStores/{fileSearchStoresId}/upload/operations/{operationsId}";
        return _requester.ExecuteAsync<Operation>(HttpMethod.Get, path, cancellationToken);
    }

}

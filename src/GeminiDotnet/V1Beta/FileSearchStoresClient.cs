using GeminiDotnet.V1Beta.FileSearchStores;

namespace GeminiDotnet.V1Beta;

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
        const string path = "/v1beta/fileSearchStores";
        return _requester.ExecuteAsync<ListFileSearchStoresResponse>(HttpMethod.Get, path, cancellationToken);
    }

    public Task<FileSearchStore> CreateFileSearchStoreAsync(
        FileSearchStore request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        const string path = "/v1beta/fileSearchStores";
        return _requester.ExecuteAsync<FileSearchStore, FileSearchStore>(HttpMethod.Post, path, request, cancellationToken);
    }

    public Task<FileSearchStore> GetFileSearchStoreAsync(
        string fileSearchStore,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(fileSearchStore);
        var path = $"/v1beta/fileSearchStores/{fileSearchStore}";
        return _requester.ExecuteAsync<FileSearchStore>(HttpMethod.Get, path, cancellationToken);
    }

    public Task<Empty> DeleteFileSearchStoreAsync(
        string fileSearchStore,
        bool? force = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(fileSearchStore);
        var path = $"/v1beta/fileSearchStores/{fileSearchStore}";
        return _requester.ExecuteAsync<Empty>(HttpMethod.Delete, path, cancellationToken);
    }

    public Task<ImportFileOperation> ImportFileAsync(
        string fileSearchStore,
        ImportFileRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(fileSearchStore);
        ArgumentNullException.ThrowIfNull(request);
        var path = $"/v1beta/fileSearchStores/{fileSearchStore}:importFile";
        return _requester.ExecuteAsync<ImportFileRequest, ImportFileOperation>(HttpMethod.Post, path, request, cancellationToken);
    }

    public Task<UploadToFileSearchStoreOperation> UploadToFileSearchStoreAsync(
        string fileSearchStore,
        UploadToFileSearchStoreRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(fileSearchStore);
        ArgumentNullException.ThrowIfNull(request);
        var path = $"/v1beta/fileSearchStores/{fileSearchStore}:uploadToFileSearchStore";
        return _requester.ExecuteAsync<UploadToFileSearchStoreRequest, UploadToFileSearchStoreOperation>(HttpMethod.Post, path, request, cancellationToken);
    }

    public Task<ListDocumentsResponse> ListDocumentsAsync(
        string fileSearchStore,
        int? pageSize = null,
        string? pageToken = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(fileSearchStore);
        var path = $"/v1beta/fileSearchStores/{fileSearchStore}/documents";
        return _requester.ExecuteAsync<ListDocumentsResponse>(HttpMethod.Get, path, cancellationToken);
    }

    public Task<Document> GetDocumentAsync(
        string fileSearchStore,
        string document,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(fileSearchStore);
        ArgumentNullException.ThrowIfNull(document);
        var path = $"/v1beta/fileSearchStores/{fileSearchStore}/documents/{document}";
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
        var path = $"/v1beta/fileSearchStores/{fileSearchStore}/documents/{document}";
        return _requester.ExecuteAsync<Empty>(HttpMethod.Delete, path, cancellationToken);
    }

    public Task<QueryDocumentResponse> QueryDocumentAsync(
        string fileSearchStore,
        string document,
        QueryDocumentRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(fileSearchStore);
        ArgumentNullException.ThrowIfNull(document);
        ArgumentNullException.ThrowIfNull(request);
        var path = $"/v1beta/fileSearchStores/{fileSearchStore}/documents/{document}:query";
        return _requester.ExecuteAsync<QueryDocumentRequest, QueryDocumentResponse>(HttpMethod.Post, path, request, cancellationToken);
    }

    public Task<Operation> GetOperationByFileSearchStoreAndOperationAsync(
        string fileSearchStore,
        string operation,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(fileSearchStore);
        ArgumentNullException.ThrowIfNull(operation);
        var path = $"/v1beta/fileSearchStores/{fileSearchStore}/operations/{operation}";
        return _requester.ExecuteAsync<Operation>(HttpMethod.Get, path, cancellationToken);
    }

    public Task<Operation> GetOperationByFileSearchStoresIdAndOperationsIdAsync(
        string fileSearchStoresId,
        string operationsId,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(fileSearchStoresId);
        ArgumentNullException.ThrowIfNull(operationsId);
        var path = $"/v1beta/fileSearchStores/{fileSearchStoresId}/upload/operations/{operationsId}";
        return _requester.ExecuteAsync<Operation>(HttpMethod.Get, path, cancellationToken);
    }

}

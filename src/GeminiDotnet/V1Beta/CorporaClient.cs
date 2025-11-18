using GeminiDotnet.V1Beta.Corpora;

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

    public Task<Corpus> UpdateCorpusAsync(
        string corpus,
        Corpus request,
        string updateMask,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(corpus);
        ArgumentNullException.ThrowIfNull(request);
        var path = $"/v1beta/corpora/{corpus}";
        return _requester.ExecuteAsync<Corpus, Corpus>(HttpMethod.Patch, path, request, cancellationToken);
    }

    public Task<QueryCorpusResponse> QueryCorpusAsync(
        string corpus,
        QueryCorpusRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(corpus);
        ArgumentNullException.ThrowIfNull(request);
        var path = $"/v1beta/corpora/{corpus}:query";
        return _requester.ExecuteAsync<QueryCorpusRequest, QueryCorpusResponse>(HttpMethod.Post, path, request, cancellationToken);
    }

    public Task<Document> UpdateDocumentAsync(
        string corpus,
        string document,
        Document request,
        string updateMask,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(corpus);
        ArgumentNullException.ThrowIfNull(document);
        ArgumentNullException.ThrowIfNull(request);
        var path = $"/v1beta/corpora/{corpus}/documents/{document}";
        return _requester.ExecuteAsync<Document, Document>(HttpMethod.Patch, path, request, cancellationToken);
    }

    public Task<ListChunksResponse> ListChunksAsync(
        string corpus,
        string document,
        int? pageSize = null,
        string? pageToken = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(corpus);
        ArgumentNullException.ThrowIfNull(document);
        var path = $"/v1beta/corpora/{corpus}/documents/{document}/chunks";
        return _requester.ExecuteAsync<ListChunksResponse>(HttpMethod.Get, path, cancellationToken);
    }

    public Task<Chunk> CreateChunkAsync(
        string corpus,
        string document,
        Chunk request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(corpus);
        ArgumentNullException.ThrowIfNull(document);
        ArgumentNullException.ThrowIfNull(request);
        var path = $"/v1beta/corpora/{corpus}/documents/{document}/chunks";
        return _requester.ExecuteAsync<Chunk, Chunk>(HttpMethod.Post, path, request, cancellationToken);
    }

    public Task<BatchCreateChunksResponse> BatchCreateChunksAsync(
        string corpus,
        string document,
        BatchCreateChunksRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(corpus);
        ArgumentNullException.ThrowIfNull(document);
        ArgumentNullException.ThrowIfNull(request);
        var path = $"/v1beta/corpora/{corpus}/documents/{document}/chunks:batchCreate";
        return _requester.ExecuteAsync<BatchCreateChunksRequest, BatchCreateChunksResponse>(HttpMethod.Post, path, request, cancellationToken);
    }

    public Task<Empty> BatchDeleteChunksAsync(
        string corpus,
        string document,
        BatchDeleteChunksRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(corpus);
        ArgumentNullException.ThrowIfNull(document);
        ArgumentNullException.ThrowIfNull(request);
        var path = $"/v1beta/corpora/{corpus}/documents/{document}/chunks:batchDelete";
        return _requester.ExecuteAsync<BatchDeleteChunksRequest, Empty>(HttpMethod.Post, path, request, cancellationToken);
    }

    public Task<BatchUpdateChunksResponse> BatchUpdateChunksAsync(
        string corpus,
        string document,
        BatchUpdateChunksRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(corpus);
        ArgumentNullException.ThrowIfNull(document);
        ArgumentNullException.ThrowIfNull(request);
        var path = $"/v1beta/corpora/{corpus}/documents/{document}/chunks:batchUpdate";
        return _requester.ExecuteAsync<BatchUpdateChunksRequest, BatchUpdateChunksResponse>(HttpMethod.Post, path, request, cancellationToken);
    }

    public Task<Chunk> GetChunkAsync(
        string corpus,
        string document,
        string chunk,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(corpus);
        ArgumentNullException.ThrowIfNull(document);
        ArgumentNullException.ThrowIfNull(chunk);
        var path = $"/v1beta/corpora/{corpus}/documents/{document}/chunks/{chunk}";
        return _requester.ExecuteAsync<Chunk>(HttpMethod.Get, path, cancellationToken);
    }

    public Task<Empty> DeleteChunkAsync(
        string corpus,
        string document,
        string chunk,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(corpus);
        ArgumentNullException.ThrowIfNull(document);
        ArgumentNullException.ThrowIfNull(chunk);
        var path = $"/v1beta/corpora/{corpus}/documents/{document}/chunks/{chunk}";
        return _requester.ExecuteAsync<Empty>(HttpMethod.Delete, path, cancellationToken);
    }

    public Task<Chunk> UpdateChunkAsync(
        string corpus,
        string document,
        string chunk,
        Chunk request,
        string updateMask,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(corpus);
        ArgumentNullException.ThrowIfNull(document);
        ArgumentNullException.ThrowIfNull(chunk);
        ArgumentNullException.ThrowIfNull(request);
        var path = $"/v1beta/corpora/{corpus}/documents/{document}/chunks/{chunk}";
        return _requester.ExecuteAsync<Chunk, Chunk>(HttpMethod.Patch, path, request, cancellationToken);
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

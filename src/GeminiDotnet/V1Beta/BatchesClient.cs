using GeminiDotnet.V1Beta.Batches;

namespace GeminiDotnet.V1Beta;

internal sealed class BatchesClient : IBatchesClient
{
    private readonly IGeminiRequester _requester;
    
    internal BatchesClient(IGeminiRequester requester)
    {
        ArgumentNullException.ThrowIfNull(requester);
        _requester = requester;
    }

    public Task<ListOperationsResponse> ListOperationsByAsync(
        string? filter = null,
        int? pageSize = null,
        string? pageToken = null,
        bool? returnPartialSuccess = null,
        CancellationToken cancellationToken = default)
    {
        const string path = "/v1beta/batches";
        return _requester.ExecuteAsync<ListOperationsResponse>(HttpMethod.Get, path, cancellationToken);
    }

    public Task<Operation> GetOperationByGenerateContentBatchAsync(
        string generateContentBatch,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(generateContentBatch);
        var path = $"/v1beta/batches/{generateContentBatch}";
        return _requester.ExecuteAsync<Operation>(HttpMethod.Get, path, cancellationToken);
    }

    public Task<Empty> DeleteOperationAsync(
        string generateContentBatch,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(generateContentBatch);
        var path = $"/v1beta/batches/{generateContentBatch}";
        return _requester.ExecuteAsync<Empty>(HttpMethod.Delete, path, cancellationToken);
    }

    public Task<Empty> CancelOperationAsync(
        string generateContentBatch,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(generateContentBatch);
        var path = $"/v1beta/batches/{generateContentBatch}:cancel";
        return _requester.ExecuteAsync<Empty>(HttpMethod.Post, path, cancellationToken);
    }

    public Task<EmbedContentBatch> UpdateEmbedContentBatchAsync(
        string generateContentBatch,
        EmbedContentBatch request,
        string? updateMask = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(generateContentBatch);
        ArgumentNullException.ThrowIfNull(request);
        var path = $"/v1beta/batches/{generateContentBatch}:updateEmbedContentBatch";
        return _requester.ExecuteAsync<EmbedContentBatch, EmbedContentBatch>(HttpMethod.Patch, path, request, cancellationToken);
    }

    public Task<GenerateContentBatch> UpdateGenerateContentBatchAsync(
        string generateContentBatch,
        GenerateContentBatch request,
        string? updateMask = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(generateContentBatch);
        ArgumentNullException.ThrowIfNull(request);
        var path = $"/v1beta/batches/{generateContentBatch}:updateGenerateContentBatch";
        return _requester.ExecuteAsync<GenerateContentBatch, GenerateContentBatch>(HttpMethod.Patch, path, request, cancellationToken);
    }

}

using GeminiDotnet.V1.TunedModels;

namespace GeminiDotnet.V1;

internal sealed class TunedModelsClient : ITunedModelsClient
{
    private readonly IGeminiRequester _requester;
    
    internal TunedModelsClient(IGeminiRequester requester)
    {
        ArgumentNullException.ThrowIfNull(requester);
        _requester = requester;
    }

    public Task<AsyncBatchEmbedContentOperation> AsyncBatchEmbedContentByTunedModelAsync(
        string tunedModel,
        AsyncBatchEmbedContentRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(tunedModel);
        ArgumentNullException.ThrowIfNull(request);
        var path = $"/v1/tunedModels/{tunedModel}:asyncBatchEmbedContent";
        return _requester.ExecuteAsync<AsyncBatchEmbedContentRequest, AsyncBatchEmbedContentOperation>(HttpMethod.Post, path, request, cancellationToken);
    }

    public Task<BatchGenerateContentOperation> BatchGenerateContentByTunedModelAsync(
        string tunedModel,
        BatchGenerateContentRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(tunedModel);
        ArgumentNullException.ThrowIfNull(request);
        var path = $"/v1/tunedModels/{tunedModel}:batchGenerateContent";
        return _requester.ExecuteAsync<BatchGenerateContentRequest, BatchGenerateContentOperation>(HttpMethod.Post, path, request, cancellationToken);
    }

    public Task<GenerateContentResponse> GenerateContentByTunedModelAsync(
        string tunedModel,
        GenerateContentRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(tunedModel);
        ArgumentNullException.ThrowIfNull(request);
        var path = $"/v1/tunedModels/{tunedModel}:generateContent";
        return _requester.ExecuteAsync<GenerateContentRequest, GenerateContentResponse>(HttpMethod.Post, path, request, cancellationToken);
    }

    public IAsyncEnumerable<GenerateContentResponse> StreamGenerateContentByTunedModelAsync(
        string tunedModel,
        GenerateContentRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(tunedModel);
        ArgumentNullException.ThrowIfNull(request);
        var path = $"/v1/tunedModels/{tunedModel}:streamGenerateContent?alt=sse";
        return _requester.ExecuteStreamingAsync<GenerateContentRequest, GenerateContentResponse>(HttpMethod.Post, path, request, cancellationToken);
    }

    public Task<ListOperationsResponse> ListOperationsByTunedModelAsync(
        string tunedModel,
        string? filter = null,
        int? pageSize = null,
        string? pageToken = null,
        bool? returnPartialSuccess = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(tunedModel);
        var path = $"/v1/tunedModels/{tunedModel}/operations";
        return _requester.ExecuteAsync<ListOperationsResponse>(HttpMethod.Get, path, cancellationToken);
    }

    public Task<Operation> GetOperationAsync(
        string tunedModel,
        string operation,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(tunedModel);
        ArgumentNullException.ThrowIfNull(operation);
        var path = $"/v1/tunedModels/{tunedModel}/operations/{operation}";
        return _requester.ExecuteAsync<Operation>(HttpMethod.Get, path, cancellationToken);
    }

    public Task<Empty> CancelOperationAsync(
        string tunedModel,
        string operation,
        CancelOperationRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(tunedModel);
        ArgumentNullException.ThrowIfNull(operation);
        ArgumentNullException.ThrowIfNull(request);
        var path = $"/v1/tunedModels/{tunedModel}/operations/{operation}:cancel";
        return _requester.ExecuteAsync<CancelOperationRequest, Empty>(HttpMethod.Post, path, request, cancellationToken);
    }

}

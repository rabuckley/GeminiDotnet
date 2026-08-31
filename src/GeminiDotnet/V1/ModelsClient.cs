using GeminiDotnet.V1.Models;

namespace GeminiDotnet.V1;

internal sealed class ModelsClient : IModelsClient
{
    private readonly IGeminiRequester _requester;
    
    internal ModelsClient(IGeminiRequester requester)
    {
        ArgumentNullException.ThrowIfNull(requester);
        _requester = requester;
    }

    public Task<ListModelsResponse> ListModelsAsync(
        int? pageSize = null,
        string? pageToken = null,
        CancellationToken cancellationToken = default)
    {
        var query = new QueryStringBuilder()
            .Add("pageSize", pageSize)
            .Add("pageToken", pageToken)
            .ToString();
        var path = $"/v1/models{query}";
        return _requester.ExecuteAsync<ListModelsResponse>(HttpMethod.Get, path, cancellationToken);
    }

    public Task<Model> GetModelAsync(
        string model,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(model);
        var path = $"/v1/models/{Uri.EscapeDataString(model)}";
        return _requester.ExecuteAsync<Model>(HttpMethod.Get, path, cancellationToken);
    }

    public Task<AsyncBatchEmbedContentOperation> AsyncBatchEmbedContentAsync(
        string model,
        AsyncBatchEmbedContentRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(model);
        ArgumentNullException.ThrowIfNull(request);
        var path = $"/v1/models/{Uri.EscapeDataString(model)}:asyncBatchEmbedContent";
        return _requester.ExecuteAsync<AsyncBatchEmbedContentRequest, AsyncBatchEmbedContentOperation>(HttpMethod.Post, path, request, cancellationToken);
    }

    public Task<BatchEmbedContentsResponse> BatchEmbedContentsAsync(
        string model,
        BatchEmbedContentsRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(model);
        ArgumentNullException.ThrowIfNull(request);
        var path = $"/v1/models/{Uri.EscapeDataString(model)}:batchEmbedContents";
        return _requester.ExecuteAsync<BatchEmbedContentsRequest, BatchEmbedContentsResponse>(HttpMethod.Post, path, request, cancellationToken);
    }

    public Task<BatchGenerateContentOperation> BatchGenerateContentAsync(
        string model,
        BatchGenerateContentRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(model);
        ArgumentNullException.ThrowIfNull(request);
        var path = $"/v1/models/{Uri.EscapeDataString(model)}:batchGenerateContent";
        return _requester.ExecuteAsync<BatchGenerateContentRequest, BatchGenerateContentOperation>(HttpMethod.Post, path, request, cancellationToken);
    }

    public Task<CountTokensResponse> CountTokensAsync(
        string model,
        CountTokensRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(model);
        ArgumentNullException.ThrowIfNull(request);
        var path = $"/v1/models/{Uri.EscapeDataString(model)}:countTokens";
        return _requester.ExecuteAsync<CountTokensRequest, CountTokensResponse>(HttpMethod.Post, path, request, cancellationToken);
    }

    public Task<EmbedContentResponse> EmbedContentAsync(
        string model,
        EmbedContentRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(model);
        ArgumentNullException.ThrowIfNull(request);
        var path = $"/v1/models/{Uri.EscapeDataString(model)}:embedContent";
        return _requester.ExecuteAsync<EmbedContentRequest, EmbedContentResponse>(HttpMethod.Post, path, request, cancellationToken);
    }

    public Task<GenerateContentResponse> GenerateContentAsync(
        string model,
        GenerateContentRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(model);
        ArgumentNullException.ThrowIfNull(request);
        var path = $"/v1/models/{Uri.EscapeDataString(model)}:generateContent";
        return _requester.ExecuteAsync<GenerateContentRequest, GenerateContentResponse>(HttpMethod.Post, path, request, cancellationToken);
    }

    public IAsyncEnumerable<GenerateContentResponse> StreamGenerateContentAsync(
        string model,
        GenerateContentRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(model);
        ArgumentNullException.ThrowIfNull(request);
        var path = $"/v1/models/{Uri.EscapeDataString(model)}:streamGenerateContent?alt=sse";
        return _requester.ExecuteStreamingAsync<GenerateContentRequest, GenerateContentResponse>(HttpMethod.Post, path, request, cancellationToken);
    }

    public Task<ListOperationsResponse> ListOperationsByModelAsync(
        string model,
        string? filter = null,
        int? pageSize = null,
        string? pageToken = null,
        bool? returnPartialSuccess = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(model);
        var query = new QueryStringBuilder()
            .Add("filter", filter)
            .Add("pageSize", pageSize)
            .Add("pageToken", pageToken)
            .Add("returnPartialSuccess", returnPartialSuccess)
            .ToString();
        var path = $"/v1/models/{Uri.EscapeDataString(model)}/operations{query}";
        return _requester.ExecuteAsync<ListOperationsResponse>(HttpMethod.Get, path, cancellationToken);
    }

    public Task<Operation> GetOperationByModelAndOperationAsync(
        string model,
        string operation,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(model);
        ArgumentNullException.ThrowIfNull(operation);
        var path = $"/v1/models/{Uri.EscapeDataString(model)}/operations/{Uri.EscapeDataString(operation)}";
        return _requester.ExecuteAsync<Operation>(HttpMethod.Get, path, cancellationToken);
    }

}

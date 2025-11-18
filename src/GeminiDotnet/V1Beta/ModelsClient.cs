using GeminiDotnet.V1Beta.Models;

namespace GeminiDotnet.V1Beta;

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
        const string path = "/v1beta/models";
        return _requester.ExecuteAsync<ListModelsResponse>(HttpMethod.Get, path, cancellationToken);
    }

    public Task<Model> GetModelAsync(
        string model,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(model);
        var path = $"/v1beta/models/{model}";
        return _requester.ExecuteAsync<Model>(HttpMethod.Get, path, cancellationToken);
    }

    public Task<AsyncBatchEmbedContentOperation> AsyncBatchEmbedContentAsync(
        string model,
        AsyncBatchEmbedContentRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(model);
        ArgumentNullException.ThrowIfNull(request);
        var path = $"/v1beta/models/{model}:asyncBatchEmbedContent";
        return _requester.ExecuteAsync<AsyncBatchEmbedContentRequest, AsyncBatchEmbedContentOperation>(HttpMethod.Post, path, request, cancellationToken);
    }

    public Task<BatchEmbedContentsResponse> BatchEmbedContentsAsync(
        string model,
        BatchEmbedContentsRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(model);
        ArgumentNullException.ThrowIfNull(request);
        var path = $"/v1beta/models/{model}:batchEmbedContents";
        return _requester.ExecuteAsync<BatchEmbedContentsRequest, BatchEmbedContentsResponse>(HttpMethod.Post, path, request, cancellationToken);
    }

    public Task<BatchEmbedTextResponse> BatchEmbedTextAsync(
        string model,
        BatchEmbedTextRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(model);
        ArgumentNullException.ThrowIfNull(request);
        var path = $"/v1beta/models/{model}:batchEmbedText";
        return _requester.ExecuteAsync<BatchEmbedTextRequest, BatchEmbedTextResponse>(HttpMethod.Post, path, request, cancellationToken);
    }

    public Task<BatchGenerateContentOperation> BatchGenerateContentAsync(
        string model,
        BatchGenerateContentRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(model);
        ArgumentNullException.ThrowIfNull(request);
        var path = $"/v1beta/models/{model}:batchGenerateContent";
        return _requester.ExecuteAsync<BatchGenerateContentRequest, BatchGenerateContentOperation>(HttpMethod.Post, path, request, cancellationToken);
    }

    public Task<CountMessageTokensResponse> CountMessageTokensAsync(
        string model,
        CountMessageTokensRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(model);
        ArgumentNullException.ThrowIfNull(request);
        var path = $"/v1beta/models/{model}:countMessageTokens";
        return _requester.ExecuteAsync<CountMessageTokensRequest, CountMessageTokensResponse>(HttpMethod.Post, path, request, cancellationToken);
    }

    public Task<CountTextTokensResponse> CountTextTokensAsync(
        string model,
        CountTextTokensRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(model);
        ArgumentNullException.ThrowIfNull(request);
        var path = $"/v1beta/models/{model}:countTextTokens";
        return _requester.ExecuteAsync<CountTextTokensRequest, CountTextTokensResponse>(HttpMethod.Post, path, request, cancellationToken);
    }

    public Task<CountTokensResponse> CountTokensAsync(
        string model,
        CountTokensRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(model);
        ArgumentNullException.ThrowIfNull(request);
        var path = $"/v1beta/models/{model}:countTokens";
        return _requester.ExecuteAsync<CountTokensRequest, CountTokensResponse>(HttpMethod.Post, path, request, cancellationToken);
    }

    public Task<EmbedContentResponse> EmbedContentAsync(
        string model,
        EmbedContentRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(model);
        ArgumentNullException.ThrowIfNull(request);
        var path = $"/v1beta/models/{model}:embedContent";
        return _requester.ExecuteAsync<EmbedContentRequest, EmbedContentResponse>(HttpMethod.Post, path, request, cancellationToken);
    }

    public Task<EmbedTextResponse> EmbedTextAsync(
        string model,
        EmbedTextRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(model);
        ArgumentNullException.ThrowIfNull(request);
        var path = $"/v1beta/models/{model}:embedText";
        return _requester.ExecuteAsync<EmbedTextRequest, EmbedTextResponse>(HttpMethod.Post, path, request, cancellationToken);
    }

    public Task<GenerateAnswerResponse> GenerateAnswerAsync(
        string model,
        GenerateAnswerRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(model);
        ArgumentNullException.ThrowIfNull(request);
        var path = $"/v1beta/models/{model}:generateAnswer";
        return _requester.ExecuteAsync<GenerateAnswerRequest, GenerateAnswerResponse>(HttpMethod.Post, path, request, cancellationToken);
    }

    public Task<GenerateContentResponse> GenerateContentAsync(
        string model,
        GenerateContentRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(model);
        ArgumentNullException.ThrowIfNull(request);
        var path = $"/v1beta/models/{model}:generateContent";
        return _requester.ExecuteAsync<GenerateContentRequest, GenerateContentResponse>(HttpMethod.Post, path, request, cancellationToken);
    }

    public Task<GenerateMessageResponse> GenerateMessageAsync(
        string model,
        GenerateMessageRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(model);
        ArgumentNullException.ThrowIfNull(request);
        var path = $"/v1beta/models/{model}:generateMessage";
        return _requester.ExecuteAsync<GenerateMessageRequest, GenerateMessageResponse>(HttpMethod.Post, path, request, cancellationToken);
    }

    public Task<GenerateTextResponse> GenerateTextAsync(
        string model,
        GenerateTextRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(model);
        ArgumentNullException.ThrowIfNull(request);
        var path = $"/v1beta/models/{model}:generateText";
        return _requester.ExecuteAsync<GenerateTextRequest, GenerateTextResponse>(HttpMethod.Post, path, request, cancellationToken);
    }

    public Task<PredictResponse> PredictAsync(
        string model,
        PredictRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(model);
        ArgumentNullException.ThrowIfNull(request);
        var path = $"/v1beta/models/{model}:predict";
        return _requester.ExecuteAsync<PredictRequest, PredictResponse>(HttpMethod.Post, path, request, cancellationToken);
    }

    public Task<PredictLongRunningOperation> PredictLongRunningAsync(
        string model,
        PredictLongRunningRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(model);
        ArgumentNullException.ThrowIfNull(request);
        var path = $"/v1beta/models/{model}:predictLongRunning";
        return _requester.ExecuteAsync<PredictLongRunningRequest, PredictLongRunningOperation>(HttpMethod.Post, path, request, cancellationToken);
    }

    public IAsyncEnumerable<GenerateContentResponse> StreamGenerateContentAsync(
        string model,
        GenerateContentRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(model);
        ArgumentNullException.ThrowIfNull(request);
        var path = $"/v1beta/models/{model}:streamGenerateContent?alt=sse";
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
        var path = $"/v1beta/models/{model}/operations";
        return _requester.ExecuteAsync<ListOperationsResponse>(HttpMethod.Get, path, cancellationToken);
    }

    public Task<Operation> GetOperationByModelAndOperationAsync(
        string model,
        string operation,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(model);
        ArgumentNullException.ThrowIfNull(operation);
        var path = $"/v1beta/models/{model}/operations/{operation}";
        return _requester.ExecuteAsync<Operation>(HttpMethod.Get, path, cancellationToken);
    }

}

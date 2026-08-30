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
        var path = $"/v1/tunedModels/{Uri.EscapeDataString(tunedModel)}:asyncBatchEmbedContent";
        return _requester.ExecuteAsync<AsyncBatchEmbedContentRequest, AsyncBatchEmbedContentOperation>(HttpMethod.Post, path, request, cancellationToken);
    }

    public Task<BatchGenerateContentOperation> BatchGenerateContentByTunedModelAsync(
        string tunedModel,
        BatchGenerateContentRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(tunedModel);
        ArgumentNullException.ThrowIfNull(request);
        var path = $"/v1/tunedModels/{Uri.EscapeDataString(tunedModel)}:batchGenerateContent";
        return _requester.ExecuteAsync<BatchGenerateContentRequest, BatchGenerateContentOperation>(HttpMethod.Post, path, request, cancellationToken);
    }

    public Task<GenerateContentResponse> GenerateContentByTunedModelAsync(
        string tunedModel,
        GenerateContentRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(tunedModel);
        ArgumentNullException.ThrowIfNull(request);
        var path = $"/v1/tunedModels/{Uri.EscapeDataString(tunedModel)}:generateContent";
        return _requester.ExecuteAsync<GenerateContentRequest, GenerateContentResponse>(HttpMethod.Post, path, request, cancellationToken);
    }

    public IAsyncEnumerable<GenerateContentResponse> StreamGenerateContentByTunedModelAsync(
        string tunedModel,
        GenerateContentRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(tunedModel);
        ArgumentNullException.ThrowIfNull(request);
        var path = $"/v1/tunedModels/{Uri.EscapeDataString(tunedModel)}:streamGenerateContent?alt=sse";
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
        var query = new QueryStringBuilder()
            .Add("filter", filter)
            .Add("pageSize", pageSize)
            .Add("pageToken", pageToken)
            .Add("returnPartialSuccess", returnPartialSuccess)
            .ToString();
        var path = $"/v1/tunedModels/{Uri.EscapeDataString(tunedModel)}/operations{query}";
        return _requester.ExecuteAsync<ListOperationsResponse>(HttpMethod.Get, path, cancellationToken);
    }

    public Task<Operation> GetOperationAsync(
        string tunedModel,
        string operation,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(tunedModel);
        ArgumentNullException.ThrowIfNull(operation);
        var path = $"/v1/tunedModels/{Uri.EscapeDataString(tunedModel)}/operations/{Uri.EscapeDataString(operation)}";
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
        var path = $"/v1/tunedModels/{Uri.EscapeDataString(tunedModel)}/operations/{Uri.EscapeDataString(operation)}:cancel";
        return _requester.ExecuteAsync<CancelOperationRequest, Empty>(HttpMethod.Post, path, request, cancellationToken);
    }

}

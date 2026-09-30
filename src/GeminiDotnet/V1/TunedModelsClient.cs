using System.Net.Http.Json;
using System.Net.ServerSentEvents;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace GeminiDotnet.V1;

internal sealed partial class TunedModelsClient : ITunedModelsClient
{
    private readonly IGeminiRequester _requester;
    
    internal TunedModelsClient(IGeminiRequester requester)
    {
        ArgumentNullException.ThrowIfNull(requester);
        _requester = requester;
    }

    public ITunedModelsOperationsClient Operations => field ??= new TunedModelsOperationsClient(_requester);

    public Task<AsyncBatchEmbedContentOperation> AsyncBatchEmbedContentAsync(
        string model,
        AsyncBatchEmbedContentRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(model);
        if (model.Split('/') is not ["tunedModels", { Length: > 0 }])
        {
            throw new ArgumentException($"'{model}' is not a resource name of the form 'tunedModels/{{tunedModelId}}'.", nameof(model));
        }
        ArgumentNullException.ThrowIfNull(request);
        var path = $"/v1/{WildcardPath.Escape(model)}:asyncBatchEmbedContent";
        return _requester.ExecuteAsync<AsyncBatchEmbedContentRequest, AsyncBatchEmbedContentOperation>(HttpMethod.Post, path, request, cancellationToken);
    }

    public Task<BatchGenerateContentOperation> BatchGenerateContentAsync(
        string model,
        BatchGenerateContentRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(model);
        if (model.Split('/') is not ["tunedModels", { Length: > 0 }])
        {
            throw new ArgumentException($"'{model}' is not a resource name of the form 'tunedModels/{{tunedModelId}}'.", nameof(model));
        }
        ArgumentNullException.ThrowIfNull(request);
        var path = $"/v1/{WildcardPath.Escape(model)}:batchGenerateContent";
        return _requester.ExecuteAsync<BatchGenerateContentRequest, BatchGenerateContentOperation>(HttpMethod.Post, path, request, cancellationToken);
    }

    public Task<GenerateContentResponse> GenerateContentAsync(
        string model,
        GenerateContentRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(model);
        if (model.Split('/') is not ["tunedModels", { Length: > 0 }])
        {
            throw new ArgumentException($"'{model}' is not a resource name of the form 'tunedModels/{{tunedModelId}}'.", nameof(model));
        }
        ArgumentNullException.ThrowIfNull(request);
        var path = $"/v1/{WildcardPath.Escape(model)}:generateContent";
        return _requester.ExecuteAsync<GenerateContentRequest, GenerateContentResponse>(HttpMethod.Post, path, request, cancellationToken);
    }

    public IAsyncEnumerable<GenerateContentResponse> StreamGenerateContentAsync(
        string model,
        GenerateContentRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(model);
        if (model.Split('/') is not ["tunedModels", { Length: > 0 }])
        {
            throw new ArgumentException($"'{model}' is not a resource name of the form 'tunedModels/{{tunedModelId}}'.", nameof(model));
        }
        ArgumentNullException.ThrowIfNull(request);
        var path = $"/v1/{WildcardPath.Escape(model)}:streamGenerateContent?alt=sse";
        return _requester.ExecuteStreamingAsync<GenerateContentRequest, GenerateContentResponse>(HttpMethod.Post, path, request, cancellationToken);
    }

}

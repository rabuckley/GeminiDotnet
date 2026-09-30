using System.Net.Http.Json;
using System.Net.ServerSentEvents;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using GeminiDotnet.V1.Models;

namespace GeminiDotnet.V1;

internal sealed partial class ModelsClient : IModelsClient
{
    private readonly IGeminiRequester _requester;
    
    internal ModelsClient(IGeminiRequester requester)
    {
        ArgumentNullException.ThrowIfNull(requester);
        _requester = requester;
    }

    public IModelsOperationsClient Operations => field ??= new ModelsOperationsClient(_requester);

    public Task<ListModelsResponse> ListAsync(
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

    public Task<Model> GetAsync(
        string name,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(name);
        if (name.Split('/') is not ["models", { Length: > 0 }])
        {
            throw new ArgumentException($"'{name}' is not a resource name of the form 'models/{{modelId}}'.", nameof(name));
        }
        var path = $"/v1/{WildcardPath.Escape(name)}";
        return _requester.ExecuteAsync<Model>(HttpMethod.Get, path, cancellationToken);
    }

    public Task<AsyncBatchEmbedContentOperation> AsyncBatchEmbedContentAsync(
        string model,
        AsyncBatchEmbedContentRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(model);
        if (model.Split('/') is not ["models", { Length: > 0 }])
        {
            throw new ArgumentException($"'{model}' is not a resource name of the form 'models/{{modelId}}'.", nameof(model));
        }
        ArgumentNullException.ThrowIfNull(request);
        var path = $"/v1/{WildcardPath.Escape(model)}:asyncBatchEmbedContent";
        return _requester.ExecuteAsync<AsyncBatchEmbedContentRequest, AsyncBatchEmbedContentOperation>(HttpMethod.Post, path, request, cancellationToken);
    }

    public Task<BatchEmbedContentsResponse> BatchEmbedContentsAsync(
        string model,
        BatchEmbedContentsRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(model);
        if (model.Split('/') is not ["models", { Length: > 0 }])
        {
            throw new ArgumentException($"'{model}' is not a resource name of the form 'models/{{modelId}}'.", nameof(model));
        }
        ArgumentNullException.ThrowIfNull(request);
        var path = $"/v1/{WildcardPath.Escape(model)}:batchEmbedContents";
        return _requester.ExecuteAsync<BatchEmbedContentsRequest, BatchEmbedContentsResponse>(HttpMethod.Post, path, request, cancellationToken);
    }

    public Task<BatchGenerateContentOperation> BatchGenerateContentAsync(
        string model,
        BatchGenerateContentRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(model);
        if (model.Split('/') is not ["models", { Length: > 0 }])
        {
            throw new ArgumentException($"'{model}' is not a resource name of the form 'models/{{modelId}}'.", nameof(model));
        }
        ArgumentNullException.ThrowIfNull(request);
        var path = $"/v1/{WildcardPath.Escape(model)}:batchGenerateContent";
        return _requester.ExecuteAsync<BatchGenerateContentRequest, BatchGenerateContentOperation>(HttpMethod.Post, path, request, cancellationToken);
    }

    public Task<CountTokensResponse> CountTokensAsync(
        string model,
        CountTokensRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(model);
        if (model.Split('/') is not ["models", { Length: > 0 }])
        {
            throw new ArgumentException($"'{model}' is not a resource name of the form 'models/{{modelId}}'.", nameof(model));
        }
        ArgumentNullException.ThrowIfNull(request);
        var path = $"/v1/{WildcardPath.Escape(model)}:countTokens";
        return _requester.ExecuteAsync<CountTokensRequest, CountTokensResponse>(HttpMethod.Post, path, request, cancellationToken);
    }

    public Task<EmbedContentResponse> EmbedContentAsync(
        string model,
        EmbedContentRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(model);
        if (model.Split('/') is not ["models", { Length: > 0 }])
        {
            throw new ArgumentException($"'{model}' is not a resource name of the form 'models/{{modelId}}'.", nameof(model));
        }
        ArgumentNullException.ThrowIfNull(request);
        var path = $"/v1/{WildcardPath.Escape(model)}:embedContent";
        return _requester.ExecuteAsync<EmbedContentRequest, EmbedContentResponse>(HttpMethod.Post, path, request, cancellationToken);
    }

    public Task<GenerateContentResponse> GenerateContentAsync(
        string model,
        GenerateContentRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(model);
        if (model.Split('/') is not ["models", { Length: > 0 }])
        {
            throw new ArgumentException($"'{model}' is not a resource name of the form 'models/{{modelId}}'.", nameof(model));
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
        if (model.Split('/') is not ["models", { Length: > 0 }])
        {
            throw new ArgumentException($"'{model}' is not a resource name of the form 'models/{{modelId}}'.", nameof(model));
        }
        ArgumentNullException.ThrowIfNull(request);
        var path = $"/v1/{WildcardPath.Escape(model)}:streamGenerateContent?alt=sse";
        return _requester.ExecuteStreamingAsync<GenerateContentRequest, GenerateContentResponse>(HttpMethod.Post, path, request, cancellationToken);
    }

}

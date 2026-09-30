using System.Net.Http.Json;
using System.Net.ServerSentEvents;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using GeminiDotnet.V1Beta.Models;

namespace GeminiDotnet.V1Beta;

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
        var path = $"/v1beta/models{query}";
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
        var path = $"/v1beta/{WildcardPath.Escape(name)}";
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
        var path = $"/v1beta/{WildcardPath.Escape(model)}:asyncBatchEmbedContent";
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
        var path = $"/v1beta/{WildcardPath.Escape(model)}:batchEmbedContents";
        return _requester.ExecuteAsync<BatchEmbedContentsRequest, BatchEmbedContentsResponse>(HttpMethod.Post, path, request, cancellationToken);
    }

    [Obsolete]
    public Task<BatchEmbedTextResponse> BatchEmbedTextAsync(
        string model,
        BatchEmbedTextRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(model);
        if (model.Split('/') is not ["models", { Length: > 0 }])
        {
            throw new ArgumentException($"'{model}' is not a resource name of the form 'models/{{modelId}}'.", nameof(model));
        }
        ArgumentNullException.ThrowIfNull(request);
        var path = $"/v1beta/{WildcardPath.Escape(model)}:batchEmbedText";
        return _requester.ExecuteAsync<BatchEmbedTextRequest, BatchEmbedTextResponse>(HttpMethod.Post, path, request, cancellationToken);
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
        var path = $"/v1beta/{WildcardPath.Escape(model)}:batchGenerateContent";
        return _requester.ExecuteAsync<BatchGenerateContentRequest, BatchGenerateContentOperation>(HttpMethod.Post, path, request, cancellationToken);
    }

    [Obsolete]
    public Task<CountMessageTokensResponse> CountMessageTokensAsync(
        string model,
        CountMessageTokensRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(model);
        if (model.Split('/') is not ["models", { Length: > 0 }])
        {
            throw new ArgumentException($"'{model}' is not a resource name of the form 'models/{{modelId}}'.", nameof(model));
        }
        ArgumentNullException.ThrowIfNull(request);
        var path = $"/v1beta/{WildcardPath.Escape(model)}:countMessageTokens";
        return _requester.ExecuteAsync<CountMessageTokensRequest, CountMessageTokensResponse>(HttpMethod.Post, path, request, cancellationToken);
    }

    [Obsolete]
    public Task<CountTextTokensResponse> CountTextTokensAsync(
        string model,
        CountTextTokensRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(model);
        if (model.Split('/') is not ["models", { Length: > 0 }])
        {
            throw new ArgumentException($"'{model}' is not a resource name of the form 'models/{{modelId}}'.", nameof(model));
        }
        ArgumentNullException.ThrowIfNull(request);
        var path = $"/v1beta/{WildcardPath.Escape(model)}:countTextTokens";
        return _requester.ExecuteAsync<CountTextTokensRequest, CountTextTokensResponse>(HttpMethod.Post, path, request, cancellationToken);
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
        var path = $"/v1beta/{WildcardPath.Escape(model)}:countTokens";
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
        var path = $"/v1beta/{WildcardPath.Escape(model)}:embedContent";
        return _requester.ExecuteAsync<EmbedContentRequest, EmbedContentResponse>(HttpMethod.Post, path, request, cancellationToken);
    }

    [Obsolete]
    public Task<EmbedTextResponse> EmbedTextAsync(
        string model,
        EmbedTextRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(model);
        if (model.Split('/') is not ["models", { Length: > 0 }])
        {
            throw new ArgumentException($"'{model}' is not a resource name of the form 'models/{{modelId}}'.", nameof(model));
        }
        ArgumentNullException.ThrowIfNull(request);
        var path = $"/v1beta/{WildcardPath.Escape(model)}:embedText";
        return _requester.ExecuteAsync<EmbedTextRequest, EmbedTextResponse>(HttpMethod.Post, path, request, cancellationToken);
    }

    [Obsolete]
    public Task<GenerateAnswerResponse> GenerateAnswerAsync(
        string model,
        GenerateAnswerRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(model);
        if (model.Split('/') is not ["models", { Length: > 0 }])
        {
            throw new ArgumentException($"'{model}' is not a resource name of the form 'models/{{modelId}}'.", nameof(model));
        }
        ArgumentNullException.ThrowIfNull(request);
        var path = $"/v1beta/{WildcardPath.Escape(model)}:generateAnswer";
        return _requester.ExecuteAsync<GenerateAnswerRequest, GenerateAnswerResponse>(HttpMethod.Post, path, request, cancellationToken);
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
        var path = $"/v1beta/{WildcardPath.Escape(model)}:generateContent";
        return _requester.ExecuteAsync<GenerateContentRequest, GenerateContentResponse>(HttpMethod.Post, path, request, cancellationToken);
    }

    [Obsolete]
    public Task<GenerateMessageResponse> GenerateMessageAsync(
        string model,
        GenerateMessageRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(model);
        if (model.Split('/') is not ["models", { Length: > 0 }])
        {
            throw new ArgumentException($"'{model}' is not a resource name of the form 'models/{{modelId}}'.", nameof(model));
        }
        ArgumentNullException.ThrowIfNull(request);
        var path = $"/v1beta/{WildcardPath.Escape(model)}:generateMessage";
        return _requester.ExecuteAsync<GenerateMessageRequest, GenerateMessageResponse>(HttpMethod.Post, path, request, cancellationToken);
    }

    [Obsolete]
    public Task<GenerateTextResponse> GenerateTextAsync(
        string model,
        GenerateTextRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(model);
        if (model.Split('/') is not ["models", { Length: > 0 }])
        {
            throw new ArgumentException($"'{model}' is not a resource name of the form 'models/{{modelId}}'.", nameof(model));
        }
        ArgumentNullException.ThrowIfNull(request);
        var path = $"/v1beta/{WildcardPath.Escape(model)}:generateText";
        return _requester.ExecuteAsync<GenerateTextRequest, GenerateTextResponse>(HttpMethod.Post, path, request, cancellationToken);
    }

    public Task<PredictResponse> PredictAsync(
        string model,
        PredictRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(model);
        if (model.Split('/') is not ["models", { Length: > 0 }])
        {
            throw new ArgumentException($"'{model}' is not a resource name of the form 'models/{{modelId}}'.", nameof(model));
        }
        ArgumentNullException.ThrowIfNull(request);
        var path = $"/v1beta/{WildcardPath.Escape(model)}:predict";
        return _requester.ExecuteAsync<PredictRequest, PredictResponse>(HttpMethod.Post, path, request, cancellationToken);
    }

    public Task<PredictLongRunningOperation> PredictLongRunningAsync(
        string model,
        PredictLongRunningRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(model);
        if (model.Split('/') is not ["models", { Length: > 0 }])
        {
            throw new ArgumentException($"'{model}' is not a resource name of the form 'models/{{modelId}}'.", nameof(model));
        }
        ArgumentNullException.ThrowIfNull(request);
        var path = $"/v1beta/{WildcardPath.Escape(model)}:predictLongRunning";
        return _requester.ExecuteAsync<PredictLongRunningRequest, PredictLongRunningOperation>(HttpMethod.Post, path, request, cancellationToken);
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
        var path = $"/v1beta/{WildcardPath.Escape(model)}:streamGenerateContent?alt=sse";
        return _requester.ExecuteStreamingAsync<GenerateContentRequest, GenerateContentResponse>(HttpMethod.Post, path, request, cancellationToken);
    }

}

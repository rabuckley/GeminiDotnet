using System.Net.Http.Json;
using System.Net.ServerSentEvents;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using GeminiDotnet.V1Beta.TunedModels;

namespace GeminiDotnet.V1Beta;

internal sealed partial class TunedModelsClient : ITunedModelsClient
{
    private readonly IGeminiRequester _requester;
    
    internal TunedModelsClient(IGeminiRequester requester)
    {
        ArgumentNullException.ThrowIfNull(requester);
        _requester = requester;
    }

    public ITunedModelsOperationsClient Operations => field ??= new TunedModelsOperationsClient(_requester);

    public ITunedModelsPermissionsClient Permissions => field ??= new TunedModelsPermissionsClient(_requester);

    [Obsolete]
    public Task<ListTunedModelsResponse> ListAsync(
        int? pageSize = null,
        string? pageToken = null,
        string? filter = null,
        CancellationToken cancellationToken = default)
    {
        var query = new QueryStringBuilder()
            .Add("pageSize", pageSize)
            .Add("pageToken", pageToken)
            .Add("filter", filter)
            .ToString();
        var path = $"/v1beta/tunedModels{query}";
        return _requester.ExecuteAsync<ListTunedModelsResponse>(HttpMethod.Get, path, cancellationToken);
    }

    [Obsolete]
    public Task<CreateTunedModelOperation> CreateAsync(
        TunedModel request,
        string? tunedModelId = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        var query = new QueryStringBuilder()
            .Add("tunedModelId", tunedModelId)
            .ToString();
        var path = $"/v1beta/tunedModels{query}";
        return _requester.ExecuteAsync<TunedModel, CreateTunedModelOperation>(HttpMethod.Post, path, request, cancellationToken);
    }

    [Obsolete]
    public Task<TunedModel> GetAsync(
        string name,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(name);
        if (name.Split('/') is not ["tunedModels", { Length: > 0 }])
        {
            throw new ArgumentException($"'{name}' is not a resource name of the form 'tunedModels/{{tunedModelId}}'.", nameof(name));
        }
        var path = $"/v1beta/{WildcardPath.Escape(name)}";
        return _requester.ExecuteAsync<TunedModel>(HttpMethod.Get, path, cancellationToken);
    }

    [Obsolete]
    public Task<Empty> DeleteAsync(
        string name,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(name);
        if (name.Split('/') is not ["tunedModels", { Length: > 0 }])
        {
            throw new ArgumentException($"'{name}' is not a resource name of the form 'tunedModels/{{tunedModelId}}'.", nameof(name));
        }
        var path = $"/v1beta/{WildcardPath.Escape(name)}";
        return _requester.ExecuteAsync<Empty>(HttpMethod.Delete, path, cancellationToken);
    }

    [Obsolete]
    public Task<TunedModel> PatchAsync(
        string name,
        TunedModel request,
        string? updateMask = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(name);
        if (name.Split('/') is not ["tunedModels", { Length: > 0 }])
        {
            throw new ArgumentException($"'{name}' is not a resource name of the form 'tunedModels/{{tunedModelId}}'.", nameof(name));
        }
        ArgumentNullException.ThrowIfNull(request);
        var query = new QueryStringBuilder()
            .Add("updateMask", updateMask)
            .ToString();
        var path = $"/v1beta/{WildcardPath.Escape(name)}{query}";
        return _requester.ExecuteAsync<TunedModel, TunedModel>(HttpMethod.Patch, path, request, cancellationToken);
    }

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
        var path = $"/v1beta/{WildcardPath.Escape(model)}:asyncBatchEmbedContent";
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
        var path = $"/v1beta/{WildcardPath.Escape(model)}:batchGenerateContent";
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
        var path = $"/v1beta/{WildcardPath.Escape(model)}:generateContent";
        return _requester.ExecuteAsync<GenerateContentRequest, GenerateContentResponse>(HttpMethod.Post, path, request, cancellationToken);
    }

    [Obsolete]
    public Task<GenerateTextResponse> GenerateTextAsync(
        string model,
        GenerateTextRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(model);
        if (model.Split('/') is not ["tunedModels", { Length: > 0 }])
        {
            throw new ArgumentException($"'{model}' is not a resource name of the form 'tunedModels/{{tunedModelId}}'.", nameof(model));
        }
        ArgumentNullException.ThrowIfNull(request);
        var path = $"/v1beta/{WildcardPath.Escape(model)}:generateText";
        return _requester.ExecuteAsync<GenerateTextRequest, GenerateTextResponse>(HttpMethod.Post, path, request, cancellationToken);
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
        var path = $"/v1beta/{WildcardPath.Escape(model)}:streamGenerateContent?alt=sse";
        return _requester.ExecuteStreamingAsync<GenerateContentRequest, GenerateContentResponse>(HttpMethod.Post, path, request, cancellationToken);
    }

    public Task<TransferOwnershipResponse> TransferOwnershipAsync(
        string name,
        TransferOwnershipRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(name);
        if (name.Split('/') is not ["tunedModels", { Length: > 0 }])
        {
            throw new ArgumentException($"'{name}' is not a resource name of the form 'tunedModels/{{tunedModelId}}'.", nameof(name));
        }
        ArgumentNullException.ThrowIfNull(request);
        var path = $"/v1beta/{WildcardPath.Escape(name)}:transferOwnership";
        return _requester.ExecuteAsync<TransferOwnershipRequest, TransferOwnershipResponse>(HttpMethod.Post, path, request, cancellationToken);
    }

}

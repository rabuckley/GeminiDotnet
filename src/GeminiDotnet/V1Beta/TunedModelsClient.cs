using GeminiDotnet.V1Beta.TunedModels;

namespace GeminiDotnet.V1Beta;

internal sealed class TunedModelsClient : ITunedModelsClient
{
    private readonly IGeminiRequester _requester;
    
    internal TunedModelsClient(IGeminiRequester requester)
    {
        ArgumentNullException.ThrowIfNull(requester);
        _requester = requester;
    }

    public Task<ListTunedModelsResponse> ListTunedModelsAsync(
        int? pageSize = null,
        string? pageToken = null,
        string? filter = null,
        CancellationToken cancellationToken = default)
    {
        const string path = "/v1beta/tunedModels";
        return _requester.ExecuteAsync<ListTunedModelsResponse>(HttpMethod.Get, path, cancellationToken);
    }

    public Task<CreateTunedModelOperation> CreateTunedModelAsync(
        TunedModel request,
        string? tunedModelId = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        const string path = "/v1beta/tunedModels";
        return _requester.ExecuteAsync<TunedModel, CreateTunedModelOperation>(HttpMethod.Post, path, request, cancellationToken);
    }

    public Task<TunedModel> GetTunedModelAsync(
        string tunedModel,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(tunedModel);
        var path = $"/v1beta/tunedModels/{tunedModel}";
        return _requester.ExecuteAsync<TunedModel>(HttpMethod.Get, path, cancellationToken);
    }

    public Task<Empty> DeleteTunedModelAsync(
        string tunedModel,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(tunedModel);
        var path = $"/v1beta/tunedModels/{tunedModel}";
        return _requester.ExecuteAsync<Empty>(HttpMethod.Delete, path, cancellationToken);
    }

    public Task<TunedModel> UpdateTunedModelAsync(
        string tunedModel,
        TunedModel request,
        string? updateMask = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(tunedModel);
        ArgumentNullException.ThrowIfNull(request);
        var path = $"/v1beta/tunedModels/{tunedModel}";
        return _requester.ExecuteAsync<TunedModel, TunedModel>(HttpMethod.Patch, path, request, cancellationToken);
    }

    public Task<AsyncBatchEmbedContentOperation> AsyncBatchEmbedContentByTunedModelAsync(
        string tunedModel,
        AsyncBatchEmbedContentRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(tunedModel);
        ArgumentNullException.ThrowIfNull(request);
        var path = $"/v1beta/tunedModels/{tunedModel}:asyncBatchEmbedContent";
        return _requester.ExecuteAsync<AsyncBatchEmbedContentRequest, AsyncBatchEmbedContentOperation>(HttpMethod.Post, path, request, cancellationToken);
    }

    public Task<BatchGenerateContentOperation> BatchGenerateContentByTunedModelAsync(
        string tunedModel,
        BatchGenerateContentRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(tunedModel);
        ArgumentNullException.ThrowIfNull(request);
        var path = $"/v1beta/tunedModels/{tunedModel}:batchGenerateContent";
        return _requester.ExecuteAsync<BatchGenerateContentRequest, BatchGenerateContentOperation>(HttpMethod.Post, path, request, cancellationToken);
    }

    public Task<GenerateContentResponse> GenerateContentByTunedModelAsync(
        string tunedModel,
        GenerateContentRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(tunedModel);
        ArgumentNullException.ThrowIfNull(request);
        var path = $"/v1beta/tunedModels/{tunedModel}:generateContent";
        return _requester.ExecuteAsync<GenerateContentRequest, GenerateContentResponse>(HttpMethod.Post, path, request, cancellationToken);
    }

    public Task<GenerateTextResponse> GenerateTextByTunedModelAsync(
        string tunedModel,
        GenerateTextRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(tunedModel);
        ArgumentNullException.ThrowIfNull(request);
        var path = $"/v1beta/tunedModels/{tunedModel}:generateText";
        return _requester.ExecuteAsync<GenerateTextRequest, GenerateTextResponse>(HttpMethod.Post, path, request, cancellationToken);
    }

    public IAsyncEnumerable<GenerateContentResponse> StreamGenerateContentByTunedModelAsync(
        string tunedModel,
        GenerateContentRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(tunedModel);
        ArgumentNullException.ThrowIfNull(request);
        var path = $"/v1beta/tunedModels/{tunedModel}:streamGenerateContent?alt=sse";
        return _requester.ExecuteStreamingAsync<GenerateContentRequest, GenerateContentResponse>(HttpMethod.Post, path, request, cancellationToken);
    }

    public Task<TransferOwnershipResponse> TransferOwnershipAsync(
        string tunedModel,
        TransferOwnershipRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(tunedModel);
        ArgumentNullException.ThrowIfNull(request);
        var path = $"/v1beta/tunedModels/{tunedModel}:transferOwnership";
        return _requester.ExecuteAsync<TransferOwnershipRequest, TransferOwnershipResponse>(HttpMethod.Post, path, request, cancellationToken);
    }

    public Task<ListOperationsResponse> ListOperationsAsync(
        string tunedModel,
        string? filter = null,
        int? pageSize = null,
        string? pageToken = null,
        bool? returnPartialSuccess = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(tunedModel);
        var path = $"/v1beta/tunedModels/{tunedModel}/operations";
        return _requester.ExecuteAsync<ListOperationsResponse>(HttpMethod.Get, path, cancellationToken);
    }

    public Task<Operation> GetOperationAsync(
        string tunedModel,
        string operation,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(tunedModel);
        ArgumentNullException.ThrowIfNull(operation);
        var path = $"/v1beta/tunedModels/{tunedModel}/operations/{operation}";
        return _requester.ExecuteAsync<Operation>(HttpMethod.Get, path, cancellationToken);
    }

    public Task<ListPermissionsResponse> ListPermissionsAsync(
        string tunedModel,
        int? pageSize = null,
        string? pageToken = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(tunedModel);
        var path = $"/v1beta/tunedModels/{tunedModel}/permissions";
        return _requester.ExecuteAsync<ListPermissionsResponse>(HttpMethod.Get, path, cancellationToken);
    }

    public Task<Permission> CreatePermissionAsync(
        string tunedModel,
        Permission request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(tunedModel);
        ArgumentNullException.ThrowIfNull(request);
        var path = $"/v1beta/tunedModels/{tunedModel}/permissions";
        return _requester.ExecuteAsync<Permission, Permission>(HttpMethod.Post, path, request, cancellationToken);
    }

    public Task<Permission> GetPermissionAsync(
        string tunedModel,
        string permission,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(tunedModel);
        ArgumentNullException.ThrowIfNull(permission);
        var path = $"/v1beta/tunedModels/{tunedModel}/permissions/{permission}";
        return _requester.ExecuteAsync<Permission>(HttpMethod.Get, path, cancellationToken);
    }

    public Task<Empty> DeletePermissionAsync(
        string tunedModel,
        string permission,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(tunedModel);
        ArgumentNullException.ThrowIfNull(permission);
        var path = $"/v1beta/tunedModels/{tunedModel}/permissions/{permission}";
        return _requester.ExecuteAsync<Empty>(HttpMethod.Delete, path, cancellationToken);
    }

    public Task<Permission> UpdatePermissionAsync(
        string tunedModel,
        string permission,
        Permission request,
        string updateMask,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(tunedModel);
        ArgumentNullException.ThrowIfNull(permission);
        ArgumentNullException.ThrowIfNull(request);
        var path = $"/v1beta/tunedModels/{tunedModel}/permissions/{permission}";
        return _requester.ExecuteAsync<Permission, Permission>(HttpMethod.Patch, path, request, cancellationToken);
    }

}

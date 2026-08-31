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
        var query = new QueryStringBuilder()
            .Add("pageSize", pageSize)
            .Add("pageToken", pageToken)
            .Add("filter", filter)
            .ToString();
        var path = $"/v1beta/tunedModels{query}";
        return _requester.ExecuteAsync<ListTunedModelsResponse>(HttpMethod.Get, path, cancellationToken);
    }

    public Task<CreateTunedModelOperation> CreateTunedModelAsync(
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

    public Task<TunedModel> GetTunedModelAsync(
        string tunedModel,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(tunedModel);
        var path = $"/v1beta/tunedModels/{Uri.EscapeDataString(tunedModel)}";
        return _requester.ExecuteAsync<TunedModel>(HttpMethod.Get, path, cancellationToken);
    }

    public Task<Empty> DeleteTunedModelAsync(
        string tunedModel,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(tunedModel);
        var path = $"/v1beta/tunedModels/{Uri.EscapeDataString(tunedModel)}";
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
        var query = new QueryStringBuilder()
            .Add("updateMask", updateMask)
            .ToString();
        var path = $"/v1beta/tunedModels/{Uri.EscapeDataString(tunedModel)}{query}";
        return _requester.ExecuteAsync<TunedModel, TunedModel>(HttpMethod.Patch, path, request, cancellationToken);
    }

    public Task<AsyncBatchEmbedContentOperation> AsyncBatchEmbedContentByTunedModelAsync(
        string tunedModel,
        AsyncBatchEmbedContentRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(tunedModel);
        ArgumentNullException.ThrowIfNull(request);
        var path = $"/v1beta/tunedModels/{Uri.EscapeDataString(tunedModel)}:asyncBatchEmbedContent";
        return _requester.ExecuteAsync<AsyncBatchEmbedContentRequest, AsyncBatchEmbedContentOperation>(HttpMethod.Post, path, request, cancellationToken);
    }

    public Task<BatchGenerateContentOperation> BatchGenerateContentByTunedModelAsync(
        string tunedModel,
        BatchGenerateContentRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(tunedModel);
        ArgumentNullException.ThrowIfNull(request);
        var path = $"/v1beta/tunedModels/{Uri.EscapeDataString(tunedModel)}:batchGenerateContent";
        return _requester.ExecuteAsync<BatchGenerateContentRequest, BatchGenerateContentOperation>(HttpMethod.Post, path, request, cancellationToken);
    }

    public Task<GenerateContentResponse> GenerateContentByTunedModelAsync(
        string tunedModel,
        GenerateContentRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(tunedModel);
        ArgumentNullException.ThrowIfNull(request);
        var path = $"/v1beta/tunedModels/{Uri.EscapeDataString(tunedModel)}:generateContent";
        return _requester.ExecuteAsync<GenerateContentRequest, GenerateContentResponse>(HttpMethod.Post, path, request, cancellationToken);
    }

    public Task<GenerateTextResponse> GenerateTextByTunedModelAsync(
        string tunedModel,
        GenerateTextRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(tunedModel);
        ArgumentNullException.ThrowIfNull(request);
        var path = $"/v1beta/tunedModels/{Uri.EscapeDataString(tunedModel)}:generateText";
        return _requester.ExecuteAsync<GenerateTextRequest, GenerateTextResponse>(HttpMethod.Post, path, request, cancellationToken);
    }

    public IAsyncEnumerable<GenerateContentResponse> StreamGenerateContentByTunedModelAsync(
        string tunedModel,
        GenerateContentRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(tunedModel);
        ArgumentNullException.ThrowIfNull(request);
        var path = $"/v1beta/tunedModels/{Uri.EscapeDataString(tunedModel)}:streamGenerateContent?alt=sse";
        return _requester.ExecuteStreamingAsync<GenerateContentRequest, GenerateContentResponse>(HttpMethod.Post, path, request, cancellationToken);
    }

    public Task<TransferOwnershipResponse> TransferOwnershipAsync(
        string tunedModel,
        TransferOwnershipRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(tunedModel);
        ArgumentNullException.ThrowIfNull(request);
        var path = $"/v1beta/tunedModels/{Uri.EscapeDataString(tunedModel)}:transferOwnership";
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
        var query = new QueryStringBuilder()
            .Add("filter", filter)
            .Add("pageSize", pageSize)
            .Add("pageToken", pageToken)
            .Add("returnPartialSuccess", returnPartialSuccess)
            .ToString();
        var path = $"/v1beta/tunedModels/{Uri.EscapeDataString(tunedModel)}/operations{query}";
        return _requester.ExecuteAsync<ListOperationsResponse>(HttpMethod.Get, path, cancellationToken);
    }

    public Task<Operation> GetOperationAsync(
        string tunedModel,
        string operation,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(tunedModel);
        ArgumentNullException.ThrowIfNull(operation);
        var path = $"/v1beta/tunedModels/{Uri.EscapeDataString(tunedModel)}/operations/{Uri.EscapeDataString(operation)}";
        return _requester.ExecuteAsync<Operation>(HttpMethod.Get, path, cancellationToken);
    }

    public Task<ListPermissionsResponse> ListPermissionsAsync(
        string tunedModel,
        int? pageSize = null,
        string? pageToken = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(tunedModel);
        var query = new QueryStringBuilder()
            .Add("pageSize", pageSize)
            .Add("pageToken", pageToken)
            .ToString();
        var path = $"/v1beta/tunedModels/{Uri.EscapeDataString(tunedModel)}/permissions{query}";
        return _requester.ExecuteAsync<ListPermissionsResponse>(HttpMethod.Get, path, cancellationToken);
    }

    public Task<Permission> CreatePermissionAsync(
        string tunedModel,
        Permission request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(tunedModel);
        ArgumentNullException.ThrowIfNull(request);
        var path = $"/v1beta/tunedModels/{Uri.EscapeDataString(tunedModel)}/permissions";
        return _requester.ExecuteAsync<Permission, Permission>(HttpMethod.Post, path, request, cancellationToken);
    }

    public Task<Permission> GetPermissionAsync(
        string tunedModel,
        string permission,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(tunedModel);
        ArgumentNullException.ThrowIfNull(permission);
        var path = $"/v1beta/tunedModels/{Uri.EscapeDataString(tunedModel)}/permissions/{Uri.EscapeDataString(permission)}";
        return _requester.ExecuteAsync<Permission>(HttpMethod.Get, path, cancellationToken);
    }

    public Task<Empty> DeletePermissionAsync(
        string tunedModel,
        string permission,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(tunedModel);
        ArgumentNullException.ThrowIfNull(permission);
        var path = $"/v1beta/tunedModels/{Uri.EscapeDataString(tunedModel)}/permissions/{Uri.EscapeDataString(permission)}";
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
        ArgumentException.ThrowIfNullOrEmpty(updateMask);
        var query = new QueryStringBuilder()
            .Add("updateMask", updateMask)
            .ToString();
        var path = $"/v1beta/tunedModels/{Uri.EscapeDataString(tunedModel)}/permissions/{Uri.EscapeDataString(permission)}{query}";
        return _requester.ExecuteAsync<Permission, Permission>(HttpMethod.Patch, path, request, cancellationToken);
    }

}

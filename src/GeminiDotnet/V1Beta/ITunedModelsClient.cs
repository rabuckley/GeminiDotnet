using GeminiDotnet.V1Beta.TunedModels;

namespace GeminiDotnet.V1Beta;

public interface ITunedModelsClient
{
    /// <summary>
    /// Lists created tuned models.
    /// </summary>
    /// <param name="pageSize">
    /// Optional. The maximum number of <c>TunedModels</c> to return (per page).
    /// The service may return fewer tuned models.
    /// If unspecified, at most 10 tuned models will be returned.
    /// This method returns at most 1000 models per page, even if you pass a larger
    /// page_size.
    /// </param>
    /// <param name="pageToken">
    /// Optional. A page token, received from a previous <c>ListTunedModels</c> call.
    /// Provide the <c>page_token</c> returned by one request as an argument to the next
    /// request to retrieve the next page.
    /// When paginating, all other parameters provided to <c>ListTunedModels</c>
    /// must match the call that provided the page token.
    /// </param>
    /// <param name="filter">
    /// Optional. A filter is a full text search over the tuned model's description and
    /// display name. By default, results will not include tuned models shared
    /// with everyone.
    /// Additional operators:
    /// - owner:me
    /// - writers:me
    /// - readers:me
    /// - readers:everyone
    /// Examples:
    /// "owner:me" returns all tuned models to which caller has owner role
    /// "readers:me" returns all tuned models to which caller has reader role
    /// "readers:everyone" returns all tuned models that are shared with everyone
    /// </param>
    /// <param name="cancellationToken"></param>
    Task<ListTunedModelsResponse> ListTunedModelsAsync(
        int? pageSize = null,
        string? pageToken = null,
        string? filter = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a tuned model.
    /// Check intermediate tuning progress (if any) through the
    /// [google.longrunning.Operations] service.
    /// Access status and results through the Operations service.
    /// Example:
    /// GET /v1/tunedModels/az2mb0bpw6i/operations/000-111-222
    /// </summary>
    /// <param name="request">Required. The tuned model to create.</param>
    /// <param name="tunedModelId">
    /// Optional. The unique id for the tuned model if specified.
    /// This value should be up to 40 characters, the first character must be a
    /// letter, the last could be a letter or a number. The id must match the
    /// regular expression: <c>[a-z]([a-z0-9-]{0,38}[a-z0-9])?</c>.
    /// </param>
    /// <param name="cancellationToken"></param>
    Task<CreateTunedModelOperation> CreateTunedModelAsync(
        TunedModel request,
        string? tunedModelId = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets information about a specific TunedModel.
    /// </summary>
    /// <param name="tunedModel">Resource ID segment making up resource <c>name</c>. It identifies the resource within its parent collection as described in https://google.aip.dev/122.</param>
    /// <param name="cancellationToken"></param>
    Task<TunedModel> GetTunedModelAsync(
        string tunedModel,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a tuned model.
    /// </summary>
    /// <param name="tunedModel">Resource ID segment making up resource <c>name</c>. It identifies the resource within its parent collection as described in https://google.aip.dev/122.</param>
    /// <param name="cancellationToken"></param>
    Task<Empty> DeleteTunedModelAsync(
        string tunedModel,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates a tuned model.
    /// </summary>
    /// <param name="tunedModel">Resource ID segment making up resource <c>name</c>. It identifies the resource within its parent collection as described in https://google.aip.dev/122.</param>
    /// <param name="request">Required. The tuned model to update.</param>
    /// <param name="updateMask">Optional. The list of fields to update.</param>
    /// <param name="cancellationToken"></param>
    Task<TunedModel> UpdateTunedModelAsync(
        string tunedModel,
        TunedModel request,
        string? updateMask = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Enqueues a batch of <c>EmbedContent</c> requests for batch processing.
    /// We have a <c>BatchEmbedContents</c> handler in <c>GenerativeService</c>, but it was
    /// synchronized. So we name this one to be <c>Async</c> to avoid confusion.
    /// </summary>
    /// <param name="tunedModel">Resource ID segment making up resource <c>name</c>. It identifies the resource within its parent collection as described in https://google.aip.dev/122.</param>
    /// <param name="request">The request body.</param>
    /// <param name="cancellationToken"></param>
    Task<AsyncBatchEmbedContentOperation> AsyncBatchEmbedContentByTunedModelAsync(
        string tunedModel,
        AsyncBatchEmbedContentRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Enqueues a batch of <c>GenerateContent</c> requests for batch processing.
    /// </summary>
    /// <param name="tunedModel">Resource ID segment making up resource <c>name</c>. It identifies the resource within its parent collection as described in https://google.aip.dev/122.</param>
    /// <param name="request">The request body.</param>
    /// <param name="cancellationToken"></param>
    Task<BatchGenerateContentOperation> BatchGenerateContentByTunedModelAsync(
        string tunedModel,
        BatchGenerateContentRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Generates a model response given an input <see cref="V1Beta.GenerateContentRequest"/>.
    /// Refer to the [text generation
    /// guide](https://ai.google.dev/gemini-api/docs/text-generation) for detailed
    /// usage information. Input capabilities differ between models, including
    /// tuned models. Refer to the [model
    /// guide](https://ai.google.dev/gemini-api/docs/models/gemini) and [tuning
    /// guide](https://ai.google.dev/gemini-api/docs/model-tuning) for details.
    /// </summary>
    /// <param name="tunedModel">Resource ID segment making up resource <c>name</c>. It identifies the resource within its parent collection as described in https://google.aip.dev/122.</param>
    /// <param name="request">The request body.</param>
    /// <param name="cancellationToken"></param>
    Task<GenerateContentResponse> GenerateContentByTunedModelAsync(
        string tunedModel,
        GenerateContentRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Generates a response from the model given an input message.
    /// </summary>
    /// <param name="tunedModel">Resource ID segment making up resource <c>name</c>. It identifies the resource within its parent collection as described in https://google.aip.dev/122.</param>
    /// <param name="request">The request body.</param>
    /// <param name="cancellationToken"></param>
    Task<GenerateTextResponse> GenerateTextByTunedModelAsync(
        string tunedModel,
        GenerateTextRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Generates a [streamed
    /// response](https://ai.google.dev/gemini-api/docs/text-generation?lang=python#generate-a-text-stream)
    /// from the model given an input <see cref="V1Beta.GenerateContentRequest"/>.
    /// </summary>
    /// <param name="tunedModel">Resource ID segment making up resource <c>name</c>. It identifies the resource within its parent collection as described in https://google.aip.dev/122.</param>
    /// <param name="request">The request body.</param>
    /// <param name="cancellationToken"></param>
    IAsyncEnumerable<GenerateContentResponse> StreamGenerateContentByTunedModelAsync(
        string tunedModel,
        GenerateContentRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Transfers ownership of the tuned model.
    /// This is the only way to change ownership of the tuned model.
    /// The current owner will be downgraded to writer role.
    /// </summary>
    /// <param name="tunedModel">Resource ID segment making up resource <c>name</c>. It identifies the resource within its parent collection as described in https://google.aip.dev/122.</param>
    /// <param name="request">The request body.</param>
    /// <param name="cancellationToken"></param>
    Task<TransferOwnershipResponse> TransferOwnershipAsync(
        string tunedModel,
        TransferOwnershipRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Lists operations that match the specified filter in the request. If the
    /// server doesn't support this method, it returns <c>UNIMPLEMENTED</c>.
    /// </summary>
    /// <param name="tunedModel">Resource ID segment making up resource <c>name</c>. It identifies the resource within its parent collection as described in https://google.aip.dev/122.</param>
    /// <param name="filter">The standard list filter.</param>
    /// <param name="pageSize">The standard list page size.</param>
    /// <param name="pageToken">The standard list page token.</param>
    /// <param name="returnPartialSuccess">
    /// When set to <c>true</c>, operations that are reachable are returned as normal,
    /// and those that are unreachable are returned in the
    /// ListOperationsResponse.unreachable
    /// field.
    /// This can only be <c>true</c> when reading across collections. For example, when
    /// <c>parent</c> is set to <c>"projects/example/locations/-"</c>.
    /// This field is not supported by default and will result in an
    /// <c>UNIMPLEMENTED</c> error if set unless explicitly documented otherwise in
    /// service or product specific documentation.
    /// </param>
    /// <param name="cancellationToken"></param>
    Task<ListOperationsResponse> ListOperationsAsync(
        string tunedModel,
        string? filter = null,
        int? pageSize = null,
        string? pageToken = null,
        bool? returnPartialSuccess = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the latest state of a long-running operation.  Clients can use this
    /// method to poll the operation result at intervals as recommended by the API
    /// service.
    /// </summary>
    /// <param name="tunedModel">Resource ID segment making up resource <c>name</c>. It identifies the resource within its parent collection as described in https://google.aip.dev/122.</param>
    /// <param name="operation">Resource ID segment making up resource <c>name</c>. It identifies the resource within its parent collection as described in https://google.aip.dev/122.</param>
    /// <param name="cancellationToken"></param>
    Task<Operation> GetOperationAsync(
        string tunedModel,
        string operation,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Lists permissions for the specific resource.
    /// </summary>
    /// <param name="tunedModel">Resource ID segment making up resource <c>name</c>. It identifies the resource within its parent collection as described in https://google.aip.dev/122.</param>
    /// <param name="pageSize">
    /// Optional. The maximum number of <see cref="V1Beta.Permission"/>s to return (per page).
    /// The service may return fewer permissions.
    /// If unspecified, at most 10 permissions will be returned.
    /// This method returns at most 1000 permissions per page, even if you pass
    /// larger page_size.
    /// </param>
    /// <param name="pageToken">
    /// Optional. A page token, received from a previous <c>ListPermissions</c> call.
    /// Provide the <c>page_token</c> returned by one request as an argument to the
    /// next request to retrieve the next page.
    /// When paginating, all other parameters provided to <c>ListPermissions</c>
    /// must match the call that provided the page token.
    /// </param>
    /// <param name="cancellationToken"></param>
    Task<ListPermissionsResponse> ListPermissionsAsync(
        string tunedModel,
        int? pageSize = null,
        string? pageToken = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Create a permission to a specific resource.
    /// </summary>
    /// <param name="tunedModel">Resource ID segment making up resource <c>name</c>. It identifies the resource within its parent collection as described in https://google.aip.dev/122.</param>
    /// <param name="request">Required. The permission to create.</param>
    /// <param name="cancellationToken"></param>
    Task<Permission> CreatePermissionAsync(
        string tunedModel,
        Permission request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets information about a specific Permission.
    /// </summary>
    /// <param name="tunedModel">Resource ID segment making up resource <c>name</c>. It identifies the resource within its parent collection as described in https://google.aip.dev/122.</param>
    /// <param name="permission">Resource ID segment making up resource <c>name</c>. It identifies the resource within its parent collection as described in https://google.aip.dev/122.</param>
    /// <param name="cancellationToken"></param>
    Task<Permission> GetPermissionAsync(
        string tunedModel,
        string permission,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes the permission.
    /// </summary>
    /// <param name="tunedModel">Resource ID segment making up resource <c>name</c>. It identifies the resource within its parent collection as described in https://google.aip.dev/122.</param>
    /// <param name="permission">Resource ID segment making up resource <c>name</c>. It identifies the resource within its parent collection as described in https://google.aip.dev/122.</param>
    /// <param name="cancellationToken"></param>
    Task<Empty> DeletePermissionAsync(
        string tunedModel,
        string permission,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates the permission.
    /// </summary>
    /// <param name="tunedModel">Resource ID segment making up resource <c>name</c>. It identifies the resource within its parent collection as described in https://google.aip.dev/122.</param>
    /// <param name="permission">Resource ID segment making up resource <c>name</c>. It identifies the resource within its parent collection as described in https://google.aip.dev/122.</param>
    /// <param name="request">
    /// Required. The permission to update.
    /// The permission's <c>name</c> field is used to identify the permission to update.
    /// </param>
    /// <param name="updateMask">
    /// Required. The list of fields to update. Accepted ones:
    /// - role (<c>Permission.role</c> field)
    /// </param>
    /// <param name="cancellationToken"></param>
    Task<Permission> UpdatePermissionAsync(
        string tunedModel,
        string permission,
        Permission request,
        string updateMask,
        CancellationToken cancellationToken = default);

}

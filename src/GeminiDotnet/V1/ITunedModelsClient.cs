using GeminiDotnet.V1.TunedModels;

namespace GeminiDotnet.V1;

public interface ITunedModelsClient
{
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
    /// Generates a model response given an input <see cref="V1.GenerateContentRequest"/>.
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
    /// Generates a [streamed
    /// response](https://ai.google.dev/gemini-api/docs/text-generation?lang=python#generate-a-text-stream)
    /// from the model given an input <see cref="V1.GenerateContentRequest"/>.
    /// </summary>
    /// <param name="tunedModel">Resource ID segment making up resource <c>name</c>. It identifies the resource within its parent collection as described in https://google.aip.dev/122.</param>
    /// <param name="request">The request body.</param>
    /// <param name="cancellationToken"></param>
    IAsyncEnumerable<GenerateContentResponse> StreamGenerateContentByTunedModelAsync(
        string tunedModel,
        GenerateContentRequest request,
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
    Task<ListOperationsResponse> ListOperationsByTunedModelAsync(
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
    /// Starts asynchronous cancellation on a long-running operation.  The server
    /// makes a best effort to cancel the operation, but success is not
    /// guaranteed.  If the server doesn't support this method, it returns
    /// <c>google.rpc.Code.UNIMPLEMENTED</c>.  Clients can use
    /// Operations.GetOperation or
    /// other methods to check whether the cancellation succeeded or whether the
    /// operation completed despite cancellation. On successful cancellation,
    /// the operation is not deleted; instead, it becomes an operation with
    /// an Operation.error value with a google.rpc.Status.code of <c>1</c>,
    /// corresponding to <c>Code.CANCELLED</c>.
    /// </summary>
    /// <param name="tunedModel">Resource ID segment making up resource <c>name</c>. It identifies the resource within its parent collection as described in https://google.aip.dev/122.</param>
    /// <param name="operation">Resource ID segment making up resource <c>name</c>. It identifies the resource within its parent collection as described in https://google.aip.dev/122.</param>
    /// <param name="request">The request body.</param>
    /// <param name="cancellationToken"></param>
    Task<Empty> CancelOperationAsync(
        string tunedModel,
        string operation,
        CancelOperationRequest request,
        CancellationToken cancellationToken = default);

}

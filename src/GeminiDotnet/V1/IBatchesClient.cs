
namespace GeminiDotnet.V1;

public partial interface IBatchesClient
{
    /// <summary>
    /// Lists operations that match the specified filter in the request. If the
    /// server doesn't support this method, it returns <c>UNIMPLEMENTED</c>.
    /// </summary>
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
    Task<ListOperationsResponse> ListAsync(
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
    /// <param name="name">
    /// The name of the operation resource.
    /// A resource name of the form <c>batches/{batchId}</c>.
    /// </param>
    /// <param name="cancellationToken"></param>
    Task<Operation> GetAsync(
        string name,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a long-running operation. This method indicates that the client is
    /// no longer interested in the operation result. It does not cancel the
    /// operation. If the server doesn't support this method, it returns
    /// <c>google.rpc.Code.UNIMPLEMENTED</c>.
    /// </summary>
    /// <param name="name">
    /// The name of the operation resource to be deleted.
    /// A resource name of the form <c>batches/{batchId}</c>.
    /// </param>
    /// <param name="cancellationToken"></param>
    Task<Empty> DeleteAsync(
        string name,
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
    /// <param name="name">
    /// The name of the operation resource to be cancelled.
    /// A resource name of the form <c>batches/{batchId}</c>.
    /// </param>
    /// <param name="cancellationToken"></param>
    Task<Empty> CancelAsync(
        string name,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates a batch of EmbedContent requests for batch processing.
    /// </summary>
    /// <param name="name">
    /// Output only. Identifier. Resource name of the batch. Format: <c>batches/{batch_id}</c>.
    /// A resource name of the form <c>batches/{batchId}</c>.
    /// </param>
    /// <param name="request">Required. The batch to update.</param>
    /// <param name="updateMask">Optional. The list of fields to update.</param>
    /// <param name="cancellationToken"></param>
    Task<EmbedContentBatch> UpdateEmbedContentBatchAsync(
        string name,
        EmbedContentBatch request,
        string? updateMask = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates a batch of GenerateContent requests for batch processing.
    /// </summary>
    /// <param name="name">
    /// Output only. Identifier. Resource name of the batch. Format: <c>batches/{batch_id}</c>.
    /// A resource name of the form <c>batches/{batchId}</c>.
    /// </param>
    /// <param name="request">Required. The batch to update.</param>
    /// <param name="updateMask">Optional. The list of fields to update.</param>
    /// <param name="cancellationToken"></param>
    Task<GenerateContentBatch> UpdateGenerateContentBatchAsync(
        string name,
        GenerateContentBatch request,
        string? updateMask = null,
        CancellationToken cancellationToken = default);

}

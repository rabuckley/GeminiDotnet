namespace GeminiDotnet.V1;

public interface IOperationsClient
{
    /// <summary>
    /// Lists operations that match the specified filter in the request. If the
    /// server doesn't support this method, it returns <c>UNIMPLEMENTED</c>.
    /// </summary>
    /// <param name="filter">
    /// The standard list filter.
    /// </param>
    /// <param name="pageSize">
    /// The standard list page size.
    /// </param>
    /// <param name="pageToken">
    /// The standard list page token.
    /// </param>
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
        string? filter = null,
        int? pageSize = null,
        string? pageToken = null,
        bool? returnPartialSuccess = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a long-running operation. This method indicates that the client is
    /// no longer interested in the operation result. It does not cancel the
    /// operation. If the server doesn't support this method, it returns
    /// <c>google.rpc.Code.UNIMPLEMENTED</c>.
    /// </summary>
    /// <param name="operationsId">
    /// Part of <c>name</c>. The name of the operation resource to be deleted.
    /// </param>
    /// <param name="cancellationToken"></param>
    Task<Empty> DeleteOperationByOperationsIdAsync(
        string operationsId,
        CancellationToken cancellationToken = default);

}

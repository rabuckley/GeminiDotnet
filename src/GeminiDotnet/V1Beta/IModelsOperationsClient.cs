
namespace GeminiDotnet.V1Beta;

public partial interface IModelsOperationsClient
{
    /// <summary>
    /// Lists operations that match the specified filter in the request. If the
    /// server doesn't support this method, it returns <c>UNIMPLEMENTED</c>.
    /// </summary>
    /// <param name="name">
    /// The name of the operation's parent resource.
    /// A resource name of the form <c>models/{modelId}</c>.
    /// </param>
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
        string name,
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
    /// A resource name of the form <c>models/{modelId}/operations/{operationId}</c>.
    /// </param>
    /// <param name="cancellationToken"></param>
    Task<Operation> GetAsync(
        string name,
        CancellationToken cancellationToken = default);

}

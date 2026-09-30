
namespace GeminiDotnet.V1Beta;

public partial interface IFileSearchStoresOperationsClient
{
    /// <summary>
    /// Gets the latest state of a long-running operation.  Clients can use this
    /// method to poll the operation result at intervals as recommended by the API
    /// service.
    /// </summary>
    /// <param name="name">
    /// The name of the operation resource.
    /// A resource name of the form <c>fileSearchStores/{fileSearchStoreId}/operations/{operationId}</c>.
    /// </param>
    /// <param name="cancellationToken"></param>
    Task<Operation> GetAsync(
        string name,
        CancellationToken cancellationToken = default);

}

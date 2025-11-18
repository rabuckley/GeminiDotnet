namespace GeminiDotnet.V1;

internal sealed class FileSearchStoresClient : IFileSearchStoresClient
{
    private readonly IGeminiRequester _requester;
    
    internal FileSearchStoresClient(IGeminiRequester requester)
    {
        ArgumentNullException.ThrowIfNull(requester);
        _requester = requester;
    }

    public Task<Operation> GetOperationByFileSearchStoreAndOperationAsync(
        string fileSearchStore,
        string operation,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(fileSearchStore);
        ArgumentNullException.ThrowIfNull(operation);
        var path = $"/v1/fileSearchStores/{fileSearchStore}/operations/{operation}";
        return _requester.ExecuteAsync<Operation>(HttpMethod.Get, path, cancellationToken);
    }

    public Task<Operation> GetOperationByFileSearchStoresIdAndOperationsIdAsync(
        string fileSearchStoresId,
        string operationsId,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(fileSearchStoresId);
        ArgumentNullException.ThrowIfNull(operationsId);
        var path = $"/v1/fileSearchStores/{fileSearchStoresId}/upload/operations/{operationsId}";
        return _requester.ExecuteAsync<Operation>(HttpMethod.Get, path, cancellationToken);
    }

}

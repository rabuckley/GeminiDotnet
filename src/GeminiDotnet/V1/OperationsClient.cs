namespace GeminiDotnet.V1;

internal sealed class OperationsClient : IOperationsClient
{
    private readonly IGeminiRequester _requester;
    
    internal OperationsClient(IGeminiRequester requester)
    {
        ArgumentNullException.ThrowIfNull(requester);
        _requester = requester;
    }

    public Task<ListOperationsResponse> ListOperationsAsync(
        string? filter = null,
        int? pageSize = null,
        string? pageToken = null,
        bool? returnPartialSuccess = null,
        CancellationToken cancellationToken = default)
    {
        const string path = "/v1/operations";
        return _requester.ExecuteAsync<ListOperationsResponse>(HttpMethod.Get, path, cancellationToken);
    }

    public Task<Empty> DeleteOperationByOperationsIdAsync(
        string operationsId,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(operationsId);
        var path = $"/v1/operations/{operationsId}";
        return _requester.ExecuteAsync<Empty>(HttpMethod.Delete, path, cancellationToken);
    }

}

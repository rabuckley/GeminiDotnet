namespace GeminiDotnet.V1;

internal sealed class CorporaClient : ICorporaClient
{
    private readonly IGeminiRequester _requester;
    
    internal CorporaClient(IGeminiRequester requester)
    {
        ArgumentNullException.ThrowIfNull(requester);
        _requester = requester;
    }

    public Task<Operation> GetOperationByCorpusAndOperationAsync(
        string corpus,
        string operation,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(corpus);
        ArgumentNullException.ThrowIfNull(operation);
        var path = $"/v1/corpora/{corpus}/operations/{operation}";
        return _requester.ExecuteAsync<Operation>(HttpMethod.Get, path, cancellationToken);
    }

}

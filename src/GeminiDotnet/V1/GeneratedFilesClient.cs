namespace GeminiDotnet.V1;

internal sealed class GeneratedFilesClient : IGeneratedFilesClient
{
    private readonly IGeminiRequester _requester;
    
    internal GeneratedFilesClient(IGeminiRequester requester)
    {
        ArgumentNullException.ThrowIfNull(requester);
        _requester = requester;
    }

    public Task<Operation> GetOperationByGeneratedFileAndOperationAsync(
        string generatedFile,
        string operation,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(generatedFile);
        ArgumentNullException.ThrowIfNull(operation);
        var path = $"/v1/generatedFiles/{generatedFile}/operations/{operation}";
        return _requester.ExecuteAsync<Operation>(HttpMethod.Get, path, cancellationToken);
    }

}

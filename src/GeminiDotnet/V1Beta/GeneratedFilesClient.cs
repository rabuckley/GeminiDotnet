using GeminiDotnet.V1Beta.GeneratedFiles;

namespace GeminiDotnet.V1Beta;

internal sealed class GeneratedFilesClient : IGeneratedFilesClient
{
    private readonly IGeminiRequester _requester;
    
    internal GeneratedFilesClient(IGeminiRequester requester)
    {
        ArgumentNullException.ThrowIfNull(requester);
        _requester = requester;
    }

    public Task<ListGeneratedFilesResponse> ListGeneratedFilesAsync(
        int? pageSize = null,
        string? pageToken = null,
        CancellationToken cancellationToken = default)
    {
        const string path = "/v1beta/generatedFiles";
        return _requester.ExecuteAsync<ListGeneratedFilesResponse>(HttpMethod.Get, path, cancellationToken);
    }

    public Task<GeneratedFile> GetGeneratedFileAsync(
        string generatedFile,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(generatedFile);
        var path = $"/v1beta/generatedFiles/{generatedFile}";
        return _requester.ExecuteAsync<GeneratedFile>(HttpMethod.Get, path, cancellationToken);
    }

    public Task<Operation> GetOperationByGeneratedFileAndOperationAsync(
        string generatedFile,
        string operation,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(generatedFile);
        ArgumentNullException.ThrowIfNull(operation);
        var path = $"/v1beta/generatedFiles/{generatedFile}/operations/{operation}";
        return _requester.ExecuteAsync<Operation>(HttpMethod.Get, path, cancellationToken);
    }

}

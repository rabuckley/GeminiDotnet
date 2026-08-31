using GeminiDotnet.V1.FilesRegister;

namespace GeminiDotnet.V1;

internal sealed class FilesRegisterClient : IFilesRegisterClient
{
    private readonly IGeminiRequester _requester;
    
    internal FilesRegisterClient(IGeminiRequester requester)
    {
        ArgumentNullException.ThrowIfNull(requester);
        _requester = requester;
    }

    public Task<RegisterFilesResponse> RegisterFilesAsync(
        RegisterFilesRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        const string path = "/v1/files:register";
        return _requester.ExecuteAsync<RegisterFilesRequest, RegisterFilesResponse>(HttpMethod.Post, path, request, cancellationToken);
    }

}

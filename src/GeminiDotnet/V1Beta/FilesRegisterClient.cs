using System.Net.Http.Json;
using System.Net.ServerSentEvents;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using GeminiDotnet.V1Beta.FilesRegister;

namespace GeminiDotnet.V1Beta;

internal sealed partial class FilesRegisterClient : IFilesRegisterClient
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
        const string path = "/v1beta/files:register";
        return _requester.ExecuteAsync<RegisterFilesRequest, RegisterFilesResponse>(HttpMethod.Post, path, request, cancellationToken);
    }

}

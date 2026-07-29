using System.Net.Http.Json;
using System.Net.ServerSentEvents;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using GeminiDotnet.V1.Files;
using GeminiDotnet.V1.FileSearchStores;
using GeminiDotnet.V1.FilesRegister;
using GeminiDotnet.V1.Models;
using GeminiDotnet.V1.TunedModels;
using File = GeminiDotnet.V1.Files.File;

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

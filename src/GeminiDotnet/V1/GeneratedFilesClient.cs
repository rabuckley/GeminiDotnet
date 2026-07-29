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

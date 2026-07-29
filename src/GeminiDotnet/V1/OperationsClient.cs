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

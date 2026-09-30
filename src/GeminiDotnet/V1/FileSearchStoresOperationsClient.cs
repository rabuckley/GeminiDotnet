using System.Net.Http.Json;
using System.Net.ServerSentEvents;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace GeminiDotnet.V1;

internal sealed partial class FileSearchStoresOperationsClient : IFileSearchStoresOperationsClient
{
    private readonly IGeminiRequester _requester;
    
    internal FileSearchStoresOperationsClient(IGeminiRequester requester)
    {
        ArgumentNullException.ThrowIfNull(requester);
        _requester = requester;
    }

    public Task<Operation> GetAsync(
        string name,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(name);
        if (name.Split('/') is not ["fileSearchStores", { Length: > 0 }, "operations", { Length: > 0 }])
        {
            throw new ArgumentException($"'{name}' is not a resource name of the form 'fileSearchStores/{{fileSearchStoreId}}/operations/{{operationId}}'.", nameof(name));
        }
        var path = $"/v1/{WildcardPath.Escape(name)}";
        return _requester.ExecuteAsync<Operation>(HttpMethod.Get, path, cancellationToken);
    }

}

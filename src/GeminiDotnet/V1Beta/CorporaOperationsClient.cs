using System.Net.Http.Json;
using System.Net.ServerSentEvents;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace GeminiDotnet.V1Beta;

internal sealed partial class CorporaOperationsClient : ICorporaOperationsClient
{
    private readonly IGeminiRequester _requester;
    
    internal CorporaOperationsClient(IGeminiRequester requester)
    {
        ArgumentNullException.ThrowIfNull(requester);
        _requester = requester;
    }

    public Task<Operation> GetAsync(
        string name,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(name);
        if (name.Split('/') is not ["corpora", { Length: > 0 }, "operations", { Length: > 0 }])
        {
            throw new ArgumentException($"'{name}' is not a resource name of the form 'corpora/{{corpusId}}/operations/{{operationId}}'.", nameof(name));
        }
        var path = $"/v1beta/{WildcardPath.Escape(name)}";
        return _requester.ExecuteAsync<Operation>(HttpMethod.Get, path, cancellationToken);
    }

}

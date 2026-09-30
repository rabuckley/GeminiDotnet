using System.Net.Http.Json;
using System.Net.ServerSentEvents;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace GeminiDotnet.V1;

internal sealed partial class OperationsClient : IOperationsClient
{
    private readonly IGeminiRequester _requester;
    
    internal OperationsClient(IGeminiRequester requester)
    {
        ArgumentNullException.ThrowIfNull(requester);
        _requester = requester;
    }

    public Task<ListOperationsResponse> ListAsync(
        string? filter = null,
        int? pageSize = null,
        string? pageToken = null,
        bool? returnPartialSuccess = null,
        CancellationToken cancellationToken = default)
    {
        var query = new QueryStringBuilder()
            .Add("filter", filter)
            .Add("pageSize", pageSize)
            .Add("pageToken", pageToken)
            .Add("returnPartialSuccess", returnPartialSuccess)
            .ToString();
        var path = $"/v1/operations{query}";
        return _requester.ExecuteAsync<ListOperationsResponse>(HttpMethod.Get, path, cancellationToken);
    }

    public Task<Empty> DeleteAsync(
        string name,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(name);
        if (name.Split('/', 2) is not ["operations", _])
        {
            throw new ArgumentException($"'{name}' is not a resource name of the form 'operations/{{operationId}}'.", nameof(name));
        }
        var path = $"/v1/{WildcardPath.Escape(name)}";
        return _requester.ExecuteAsync<Empty>(HttpMethod.Delete, path, cancellationToken);
    }

}

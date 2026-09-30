using System.Net.Http.Json;
using System.Net.ServerSentEvents;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace GeminiDotnet.V1Beta;

internal sealed partial class ModelsOperationsClient : IModelsOperationsClient
{
    private readonly IGeminiRequester _requester;
    
    internal ModelsOperationsClient(IGeminiRequester requester)
    {
        ArgumentNullException.ThrowIfNull(requester);
        _requester = requester;
    }

    public Task<ListOperationsResponse> ListAsync(
        string name,
        string? filter = null,
        int? pageSize = null,
        string? pageToken = null,
        bool? returnPartialSuccess = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(name);
        if (name.Split('/') is not ["models", { Length: > 0 }])
        {
            throw new ArgumentException($"'{name}' is not a resource name of the form 'models/{{modelId}}'.", nameof(name));
        }
        var query = new QueryStringBuilder()
            .Add("filter", filter)
            .Add("pageSize", pageSize)
            .Add("pageToken", pageToken)
            .Add("returnPartialSuccess", returnPartialSuccess)
            .ToString();
        var path = $"/v1beta/{WildcardPath.Escape(name)}/operations{query}";
        return _requester.ExecuteAsync<ListOperationsResponse>(HttpMethod.Get, path, cancellationToken);
    }

    public Task<Operation> GetAsync(
        string name,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(name);
        if (name.Split('/') is not ["models", { Length: > 0 }, "operations", { Length: > 0 }])
        {
            throw new ArgumentException($"'{name}' is not a resource name of the form 'models/{{modelId}}/operations/{{operationId}}'.", nameof(name));
        }
        var path = $"/v1beta/{WildcardPath.Escape(name)}";
        return _requester.ExecuteAsync<Operation>(HttpMethod.Get, path, cancellationToken);
    }

}

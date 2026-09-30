using System.Net.Http.Json;
using System.Net.ServerSentEvents;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using GeminiDotnet.V1.TunedModels;

namespace GeminiDotnet.V1;

internal sealed partial class TunedModelsOperationsClient : ITunedModelsOperationsClient
{
    private readonly IGeminiRequester _requester;
    
    internal TunedModelsOperationsClient(IGeminiRequester requester)
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
        if (name.Split('/') is not ["tunedModels", { Length: > 0 }])
        {
            throw new ArgumentException($"'{name}' is not a resource name of the form 'tunedModels/{{tunedModelId}}'.", nameof(name));
        }
        var query = new QueryStringBuilder()
            .Add("filter", filter)
            .Add("pageSize", pageSize)
            .Add("pageToken", pageToken)
            .Add("returnPartialSuccess", returnPartialSuccess)
            .ToString();
        var path = $"/v1/{WildcardPath.Escape(name)}/operations{query}";
        return _requester.ExecuteAsync<ListOperationsResponse>(HttpMethod.Get, path, cancellationToken);
    }

    public Task<Operation> GetAsync(
        string name,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(name);
        if (name.Split('/') is not ["tunedModels", { Length: > 0 }, "operations", { Length: > 0 }])
        {
            throw new ArgumentException($"'{name}' is not a resource name of the form 'tunedModels/{{tunedModelId}}/operations/{{operationId}}'.", nameof(name));
        }
        var path = $"/v1/{WildcardPath.Escape(name)}";
        return _requester.ExecuteAsync<Operation>(HttpMethod.Get, path, cancellationToken);
    }

    public Task<Empty> CancelAsync(
        string name,
        CancelOperationRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(name);
        if (name.Split('/') is not ["tunedModels", { Length: > 0 }, "operations", { Length: > 0 }])
        {
            throw new ArgumentException($"'{name}' is not a resource name of the form 'tunedModels/{{tunedModelId}}/operations/{{operationId}}'.", nameof(name));
        }
        ArgumentNullException.ThrowIfNull(request);
        var path = $"/v1/{WildcardPath.Escape(name)}:cancel";
        return _requester.ExecuteAsync<CancelOperationRequest, Empty>(HttpMethod.Post, path, request, cancellationToken);
    }

}

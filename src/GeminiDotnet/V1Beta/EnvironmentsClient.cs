using System.Net.Http.Json;
using System.Net.ServerSentEvents;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using GeminiDotnet.V1Beta.Environments;
using Environment = GeminiDotnet.V1Beta.Environments.Environment;

namespace GeminiDotnet.V1Beta;

internal sealed partial class EnvironmentsClient : IEnvironmentsClient
{
    private readonly IGeminiRequester _requester;
    
    internal EnvironmentsClient(IGeminiRequester requester)
    {
        ArgumentNullException.ThrowIfNull(requester);
        _requester = requester;
    }

    public IEnvironmentsFilesClient Files => field ??= new EnvironmentsFilesClient(_requester);

    public Task<Empty> DeleteAsync(
        string id,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(id);
        var path = $"/v1beta/environments/{Uri.EscapeDataString(id)}:delete";
        return _requester.ExecuteAsync<Empty>(HttpMethod.Delete, path, cancellationToken);
    }

    public Task<Environment> GetAsync(
        string id,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(id);
        var path = $"/v1beta/environments/{Uri.EscapeDataString(id)}:get";
        return _requester.ExecuteAsync<Environment>(HttpMethod.Get, path, cancellationToken);
    }

    public Task<Environment> CreateAsync(
        CreateEnvironmentRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        const string path = "/v1beta/environments:create";
        return _requester.ExecuteAsync<CreateEnvironmentRequest, Environment>(HttpMethod.Post, path, request, cancellationToken);
    }

    public Task<ListEnvironmentsResponse> ListAsync(
        int? pageSize = null,
        string? pageToken = null,
        CancellationToken cancellationToken = default)
    {
        var query = new QueryStringBuilder()
            .Add("pageSize", pageSize)
            .Add("pageToken", pageToken)
            .ToString();
        var path = $"/v1beta/environments:list{query}";
        return _requester.ExecuteAsync<ListEnvironmentsResponse>(HttpMethod.Get, path, cancellationToken);
    }

}

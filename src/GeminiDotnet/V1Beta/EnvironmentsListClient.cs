using GeminiDotnet.V1Beta.EnvironmentsList;

namespace GeminiDotnet.V1Beta;

internal sealed class EnvironmentsListClient : IEnvironmentsListClient
{
    private readonly IGeminiRequester _requester;
    
    internal EnvironmentsListClient(IGeminiRequester requester)
    {
        ArgumentNullException.ThrowIfNull(requester);
        _requester = requester;
    }

    public Task<ListEnvironmentsResponse> ListEnvironmentsAsync(
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

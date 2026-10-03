using System.Net.Http.Json;
using System.Net.ServerSentEvents;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using GeminiDotnet.V1Beta.Environments;

namespace GeminiDotnet.V1Beta;

internal sealed partial class EnvironmentsFilesClient : IEnvironmentsFilesClient
{
    private readonly IGeminiRequester _requester;
    
    internal EnvironmentsFilesClient(IGeminiRequester requester)
    {
        ArgumentNullException.ThrowIfNull(requester);
        _requester = requester;
    }

    public IEnvironmentsFilesMediaClient Media => field ??= new EnvironmentsFilesMediaClient(_requester);

    public Task<GetEnvironmentFilesResponse> GetAsync(
        string name,
        bool? recursive = null,
        int? pageSize = null,
        string? pageToken = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(name);
        if (name.Split('/', 4) is not ["environments", { Length: > 0 }, "files", _])
        {
            throw new ArgumentException($"'{name}' is not a resource name of the form 'environments/{{environmentId}}/files/{{fileId}}'.", nameof(name));
        }
        var query = new QueryStringBuilder()
            .Add("recursive", recursive)
            .Add("page_size", pageSize)
            .Add("page_token", pageToken)
            .ToString();
        var path = $"/v1beta/{WildcardPath.Escape(name)}{query}";
        return _requester.ExecuteAsync<GetEnvironmentFilesResponse>(HttpMethod.Get, path, cancellationToken);
    }

}

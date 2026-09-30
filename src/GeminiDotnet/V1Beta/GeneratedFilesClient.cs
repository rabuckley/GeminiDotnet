using System.Net.Http.Json;
using System.Net.ServerSentEvents;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using GeminiDotnet.V1Beta.GeneratedFiles;

namespace GeminiDotnet.V1Beta;

internal sealed partial class GeneratedFilesClient : IGeneratedFilesClient
{
    private readonly IGeminiRequester _requester;
    
    internal GeneratedFilesClient(IGeminiRequester requester)
    {
        ArgumentNullException.ThrowIfNull(requester);
        _requester = requester;
    }

    public IGeneratedFilesOperationsClient Operations => field ??= new GeneratedFilesOperationsClient(_requester);

    public Task<ListGeneratedFilesResponse> ListAsync(
        int? pageSize = null,
        string? pageToken = null,
        CancellationToken cancellationToken = default)
    {
        var query = new QueryStringBuilder()
            .Add("pageSize", pageSize)
            .Add("pageToken", pageToken)
            .ToString();
        var path = $"/v1beta/generatedFiles{query}";
        return _requester.ExecuteAsync<ListGeneratedFilesResponse>(HttpMethod.Get, path, cancellationToken);
    }

    public Task<GeneratedFile> GetAsync(
        string name,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(name);
        if (name.Split('/') is not ["generatedFiles", { Length: > 0 }])
        {
            throw new ArgumentException($"'{name}' is not a resource name of the form 'generatedFiles/{{generatedFileId}}'.", nameof(name));
        }
        var path = $"/v1beta/{WildcardPath.Escape(name)}";
        return _requester.ExecuteAsync<GeneratedFile>(HttpMethod.Get, path, cancellationToken);
    }

}

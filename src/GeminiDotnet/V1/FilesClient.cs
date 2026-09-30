using System.Net.Http.Json;
using System.Net.ServerSentEvents;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using GeminiDotnet.V1.Files;
using File = GeminiDotnet.V1.Files.File;

namespace GeminiDotnet.V1;

internal sealed partial class FilesClient : IFilesClient
{
    private readonly IGeminiRequester _requester;
    
    internal FilesClient(IGeminiRequester requester)
    {
        ArgumentNullException.ThrowIfNull(requester);
        _requester = requester;
    }

    public Task<ListFilesResponse> ListAsync(
        int? pageSize = null,
        string? pageToken = null,
        CancellationToken cancellationToken = default)
    {
        var query = new QueryStringBuilder()
            .Add("pageSize", pageSize)
            .Add("pageToken", pageToken)
            .ToString();
        var path = $"/v1/files{query}";
        return _requester.ExecuteAsync<ListFilesResponse>(HttpMethod.Get, path, cancellationToken);
    }

    public Task<File> GetAsync(
        string name,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(name);
        if (name.Split('/') is not ["files", { Length: > 0 }])
        {
            throw new ArgumentException($"'{name}' is not a resource name of the form 'files/{{fileId}}'.", nameof(name));
        }
        var path = $"/v1/{WildcardPath.Escape(name)}";
        return _requester.ExecuteAsync<File>(HttpMethod.Get, path, cancellationToken);
    }

    public Task<Empty> DeleteAsync(
        string name,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(name);
        if (name.Split('/') is not ["files", { Length: > 0 }])
        {
            throw new ArgumentException($"'{name}' is not a resource name of the form 'files/{{fileId}}'.", nameof(name));
        }
        var path = $"/v1/{WildcardPath.Escape(name)}";
        return _requester.ExecuteAsync<Empty>(HttpMethod.Delete, path, cancellationToken);
    }

    public Task<MediaDownload> DownloadAsync(
        string name,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(name);
        if (name.Split('/') is not ["files", { Length: > 0 }])
        {
            throw new ArgumentException($"'{name}' is not a resource name of the form 'files/{{fileId}}'.", nameof(name));
        }
        var path = $"/v1/{WildcardPath.Escape(name)}:download?alt=media";
        return _requester.DownloadAsync(HttpMethod.Get, path, cancellationToken);
    }

    public Task<RegisterFilesResponse> RegisterAsync(
        RegisterFilesRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        const string path = "/v1/files:register";
        return _requester.ExecuteAsync<RegisterFilesRequest, RegisterFilesResponse>(HttpMethod.Post, path, request, cancellationToken);
    }

}

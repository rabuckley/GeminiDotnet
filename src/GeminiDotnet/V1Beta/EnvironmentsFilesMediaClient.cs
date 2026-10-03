using System.Net.Http.Json;
using System.Net.ServerSentEvents;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using GeminiDotnet.V1Beta.Environments;

namespace GeminiDotnet.V1Beta;

internal sealed partial class EnvironmentsFilesMediaClient : IEnvironmentsFilesMediaClient
{
    private readonly IGeminiRequester _requester;
    
    internal EnvironmentsFilesMediaClient(IGeminiRequester requester)
    {
        ArgumentNullException.ThrowIfNull(requester);
        _requester = requester;
    }

    public Task<GetEnvironmentFilesResponse> DownloadAsync(
        string parent,
        string? path = null,
        bool? recursive = null,
        int? pageSize = null,
        string? pageToken = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(parent);
        if (parent.Split('/') is not ["environments", { Length: > 0 }])
        {
            throw new ArgumentException($"'{parent}' is not a resource name of the form 'environments/{{environmentId}}'.", nameof(parent));
        }
        var query = new QueryStringBuilder()
            .Add("path", path)
            .Add("recursive", recursive)
            .Add("page_size", pageSize)
            .Add("page_token", pageToken)
            .ToString();
        var requestPath = $"/v1beta/{WildcardPath.Escape(parent)}/files{query}";
        return _requester.ExecuteAsync<GetEnvironmentFilesResponse>(HttpMethod.Get, requestPath, cancellationToken);
    }

    public Task<MediaDownload> DownloadContentAsync(
        string parent,
        string? path = null,
        bool? recursive = null,
        int? pageSize = null,
        string? pageToken = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(parent);
        if (parent.Split('/') is not ["environments", { Length: > 0 }])
        {
            throw new ArgumentException($"'{parent}' is not a resource name of the form 'environments/{{environmentId}}'.", nameof(parent));
        }
        var query = new QueryStringBuilder()
            .Add("alt", "media")
            .Add("path", path)
            .Add("recursive", recursive)
            .Add("page_size", pageSize)
            .Add("page_token", pageToken)
            .ToString();
        var requestPath = $"/v1beta/{WildcardPath.Escape(parent)}/files{query}";
        return _requester.DownloadAsync(HttpMethod.Get, requestPath, cancellationToken);
    }

    public Task<UploadEnvironmentFileResponse> UploadAsync(
        string parent,
        string path,
        UploadEnvironmentFileRequest request,
        MediaContent media,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(parent);
        if (parent.Split('/') is not ["environments", { Length: > 0 }])
        {
            throw new ArgumentException($"'{parent}' is not a resource name of the form 'environments/{{environmentId}}'.", nameof(parent));
        }
        ArgumentNullException.ThrowIfNull(path);
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(media);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(media.Length, 2147483648L);
        var requestPath = $"/upload/v1beta/{WildcardPath.Escape(parent)}/files/{WildcardPath.Escape(path)}";
        return _requester.UploadAsync<UploadEnvironmentFileRequest, UploadEnvironmentFileResponse>(HttpMethod.Put, requestPath, request, media, cancellationToken);
    }

}

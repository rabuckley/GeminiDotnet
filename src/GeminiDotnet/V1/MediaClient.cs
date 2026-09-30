using System.Net.Http.Json;
using System.Net.ServerSentEvents;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using GeminiDotnet.V1.Media;

namespace GeminiDotnet.V1;

internal sealed partial class MediaClient : IMediaClient
{
    private readonly IGeminiRequester _requester;
    
    internal MediaClient(IGeminiRequester requester)
    {
        ArgumentNullException.ThrowIfNull(requester);
        _requester = requester;
    }

    public Task<DownloadMediaResponse> DownloadAsync(
        string name,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(name);
        if (name.Split('/', 4) is not ["fileSearchStores", { Length: > 0 }, "media", _])
        {
            throw new ArgumentException($"'{name}' is not a resource name of the form 'fileSearchStores/{{fileSearchStoreId}}/media/{{mediaId}}'.", nameof(name));
        }
        var path = $"/v1/{WildcardPath.Escape(name)}";
        return _requester.ExecuteAsync<DownloadMediaResponse>(HttpMethod.Get, path, cancellationToken);
    }

    public Task<MediaDownload> DownloadContentAsync(
        string name,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(name);
        if (name.Split('/', 4) is not ["fileSearchStores", { Length: > 0 }, "media", _])
        {
            throw new ArgumentException($"'{name}' is not a resource name of the form 'fileSearchStores/{{fileSearchStoreId}}/media/{{mediaId}}'.", nameof(name));
        }
        var path = $"/v1/{WildcardPath.Escape(name)}?alt=media";
        return _requester.DownloadAsync(HttpMethod.Get, path, cancellationToken);
    }

    public Task<UploadToFileSearchStoreOperation> UploadToFileSearchStoreAsync(
        string fileSearchStoreName,
        UploadToFileSearchStoreRequest request,
        MediaContent media,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(fileSearchStoreName);
        if (fileSearchStoreName.Split('/') is not ["fileSearchStores", { Length: > 0 }])
        {
            throw new ArgumentException($"'{fileSearchStoreName}' is not a resource name of the form 'fileSearchStores/{{fileSearchStoreId}}'.", nameof(fileSearchStoreName));
        }
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(media);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(media.Length, 104857600L);
        var path = $"/upload/v1/{WildcardPath.Escape(fileSearchStoreName)}:uploadToFileSearchStore";
        return _requester.UploadAsync<UploadToFileSearchStoreRequest, UploadToFileSearchStoreOperation>(HttpMethod.Post, path, request, media, cancellationToken);
    }

    public Task<CreateFileResponse> UploadAsync(
        CreateFileRequest request,
        MediaContent media,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(media);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(media.Length, 2147483648L);
        const string path = "/upload/v1/files";
        return _requester.UploadAsync<CreateFileRequest, CreateFileResponse>(HttpMethod.Post, path, request, media, cancellationToken);
    }

}

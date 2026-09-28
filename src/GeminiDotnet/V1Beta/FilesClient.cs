using GeminiDotnet.V1Beta.Files;
using File = GeminiDotnet.V1Beta.Files.File;

namespace GeminiDotnet.V1Beta;

internal sealed partial class FilesClient : IFilesClient
{
    private readonly IGeminiRequester _requester;
    
    internal FilesClient(IGeminiRequester requester)
    {
        ArgumentNullException.ThrowIfNull(requester);
        _requester = requester;
    }

    public Task<ListFilesResponse> ListFilesAsync(
        int? pageSize = null,
        string? pageToken = null,
        CancellationToken cancellationToken = default)
    {
        var query = new QueryStringBuilder()
            .Add("pageSize", pageSize)
            .Add("pageToken", pageToken)
            .ToString();
        var path = $"/v1beta/files{query}";
        return _requester.ExecuteAsync<ListFilesResponse>(HttpMethod.Get, path, cancellationToken);
    }

    public Task<CreateFileResponse> CreateFileAsync(
        CreateFileRequest request,
        MediaContent media,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(media);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(media.Length, 2147483648L);
        const string path = "/upload/v1beta/files";
        return _requester.UploadAsync<CreateFileRequest, CreateFileResponse>(HttpMethod.Post, path, request, media, cancellationToken);
    }

    public Task<File> GetFileAsync(
        string file,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(file);
        var path = $"/v1beta/files/{Uri.EscapeDataString(file)}";
        return _requester.ExecuteAsync<File>(HttpMethod.Get, path, cancellationToken);
    }

    public Task<Empty> DeleteFileAsync(
        string file,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(file);
        var path = $"/v1beta/files/{Uri.EscapeDataString(file)}";
        return _requester.ExecuteAsync<Empty>(HttpMethod.Delete, path, cancellationToken);
    }

    public Task<DownloadFileResponse> DownloadFileAsync(
        string file,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(file);
        var path = $"/v1beta/files/{Uri.EscapeDataString(file)}:download";
        return _requester.ExecuteAsync<DownloadFileResponse>(HttpMethod.Get, path, cancellationToken);
    }

}

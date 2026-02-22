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
        const string path = "/v1beta/files";
        return _requester.ExecuteAsync<ListFilesResponse>(HttpMethod.Get, path, cancellationToken);
    }

    public Task<CreateFileResponse> CreateFileAsync(
        CreateFileRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        const string path = "/v1beta/files";
        return _requester.ExecuteAsync<CreateFileRequest, CreateFileResponse>(HttpMethod.Post, path, request, cancellationToken);
    }

    public Task<File> GetFileAsync(
        string file,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(file);
        var path = $"/v1beta/files/{file}";
        return _requester.ExecuteAsync<File>(HttpMethod.Get, path, cancellationToken);
    }

    public Task<Empty> DeleteFileAsync(
        string file,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(file);
        var path = $"/v1beta/files/{file}";
        return _requester.ExecuteAsync<Empty>(HttpMethod.Delete, path, cancellationToken);
    }

    public Task<DownloadFileResponse> DownloadFileAsync(
        string file,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(file);
        var path = $"/v1beta/files/{file}:download";
        return _requester.ExecuteAsync<DownloadFileResponse>(HttpMethod.Get, path, cancellationToken);
    }

}

using System.Net.Http.Json;
using System.Net.ServerSentEvents;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using GeminiDotnet.V1.Files;
using GeminiDotnet.V1.FileSearchStores;
using GeminiDotnet.V1.FilesRegister;
using GeminiDotnet.V1.Models;
using GeminiDotnet.V1.TunedModels;
using File = GeminiDotnet.V1.Files.File;

namespace GeminiDotnet.V1;

internal sealed class FilesClient : IFilesClient
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
        var path = $"/v1/files{query}";
        return _requester.ExecuteAsync<ListFilesResponse>(HttpMethod.Get, path, cancellationToken);
    }

    public Task<CreateFileResponse> CreateFileAsync(
        CreateFileRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        const string path = "/v1/files";
        return _requester.ExecuteAsync<CreateFileRequest, CreateFileResponse>(HttpMethod.Post, path, request, cancellationToken);
    }

    public Task<File> GetFileAsync(
        string file,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(file);
        var path = $"/v1/files/{Uri.EscapeDataString(file)}";
        return _requester.ExecuteAsync<File>(HttpMethod.Get, path, cancellationToken);
    }

    public Task<Empty> DeleteFileAsync(
        string file,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(file);
        var path = $"/v1/files/{Uri.EscapeDataString(file)}";
        return _requester.ExecuteAsync<Empty>(HttpMethod.Delete, path, cancellationToken);
    }

    public Task<DownloadFileResponse> DownloadFileAsync(
        string file,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(file);
        var path = $"/v1/files/{Uri.EscapeDataString(file)}:download";
        return _requester.ExecuteAsync<DownloadFileResponse>(HttpMethod.Get, path, cancellationToken);
    }

}

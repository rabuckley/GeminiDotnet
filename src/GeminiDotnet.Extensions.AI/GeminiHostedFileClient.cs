using GeminiDotnet.V1Beta;
using GeminiDotnet.V1Beta.Files;
using Microsoft.Extensions.AI;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Runtime.CompilerServices;
using File = GeminiDotnet.V1Beta.Files.File;
using Type = System.Type;

#pragma warning disable MEAI001 // Type is for evaluation purposes only

namespace GeminiDotnet.Extensions.AI;

/// <summary>
/// An <see cref="IHostedFileClient"/> implementation for the Gemini AI service,
/// adapting the Gemini <see cref="IFilesClient"/> to the portable M.E.AI abstraction.
/// </summary>
[Experimental("MEAI001")]
public sealed class GeminiHostedFileClient : IHostedFileClient
{
    private readonly IGeminiClient _client;
    private readonly bool _ownsClient;
    private readonly HostedFileClientMetadata _metadata;

    private IFilesClient FilesClient => _client.V1Beta.Files;

    /// <summary>
    /// Initializes a new instance of the <see cref="GeminiHostedFileClient"/> class.
    /// The created <see cref="GeminiClient"/> is owned by this instance and will be
    /// disposed when this instance is disposed.
    /// </summary>
    /// <param name="options">The options to use for the client.</param>
    public GeminiHostedFileClient(GeminiClientOptions options) : this(new GeminiClient(options), ownsClient: true)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GeminiHostedFileClient"/> class.
    /// </summary>
    /// <param name="client">The <see cref="IGeminiClient"/> to use.</param>
    public GeminiHostedFileClient(IGeminiClient client) : this(client, ownsClient: false)
    {
    }

    private GeminiHostedFileClient(IGeminiClient client, bool ownsClient)
    {
        ArgumentNullException.ThrowIfNull(client);

        _client = client;
        _ownsClient = ownsClient;
        _metadata = new HostedFileClientMetadata(
            providerName: "Gemini",
            providerUri: client.Endpoint);
    }

    /// <inheritdoc />
    public async Task<HostedFileContent> UploadAsync(
        Stream content,
        string? mediaType = null,
        string? fileName = null,
        HostedFileClientOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(content);

        if (!content.CanSeek)
        {
            throw new ArgumentException(
                "The content stream must be seekable so that its length can be determined. " +
                "Consider buffering to a MemoryStream first.",
                nameof(content));
        }

        var uploadOptions = new UploadFileOptions
        {
            MimeType = mediaType,
            DisplayName = fileName,
        };

        var file = await FilesClient.UploadFileAsync(
            content,
            content.Length,
            uploadOptions,
            cancellationToken).ConfigureAwait(false);

        return MapFileToHostedFileContent(file);
    }

    /// <inheritdoc />
    public async Task<HostedFileDownloadStream> DownloadAsync(
        string fileId,
        HostedFileClientOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(fileId);

        var resourceId = ExtractResourceId(fileId);

        // Fetch metadata and download the content stream concurrently.
        var fileTask = FilesClient.GetFileAsync(resourceId, cancellationToken);
        var downloadTask = FilesClient.DownloadFileStreamAsync(resourceId, cancellationToken);

        FileDownloadResult? downloadResult = null;
        try
        {
            await Task.WhenAll(fileTask, downloadTask).ConfigureAwait(false);

            downloadResult = downloadTask.Result;

            // Eagerly resolve the content stream so the wrapper never hits sync-over-async.
            var contentStream = await downloadResult.GetContentStreamAsync(cancellationToken).ConfigureAwait(false);

            return new GeminiHostedFileDownloadStream(downloadResult, contentStream, fileTask.Result.DisplayName);
        }
        catch
        {
            // If the download task completed but something else failed, dispose its
            // result to release the underlying HttpResponseMessage.
            if (downloadTask.IsCompletedSuccessfully)
            {
                downloadResult ??= downloadTask.Result;
                downloadResult.Dispose();
            }

            throw;
        }
    }

    /// <inheritdoc />
    public async Task<HostedFileContent?> GetFileInfoAsync(
        string fileId,
        HostedFileClientOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(fileId);

        var resourceId = ExtractResourceId(fileId);

        try
        {
            var file = await FilesClient.GetFileAsync(resourceId, cancellationToken).ConfigureAwait(false);
            return MapFileToHostedFileContent(file);
        }
        catch (GeminiClientException ex) when (ex.Response.StatusCode is HttpStatusCode.NotFound)
        {
            return null;
        }
    }

    /// <inheritdoc />
    public async IAsyncEnumerable<HostedFileContent> ListFilesAsync(
        HostedFileClientOptions? options = null,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var limit = options?.Limit;
        var yielded = 0;
        string? pageToken = null;

        do
        {
            // Request up to 100 per page (the Gemini API maximum), or the remaining
            // count if a limit was specified and fewer items are needed.
            int? pageSize = limit is not null
                ? Math.Min(100, limit.Value - yielded)
                : 100;

            if (pageSize <= 0)
            {
                yield break;
            }

            var response = await FilesClient.ListFilesAsync(
                pageSize,
                pageToken,
                cancellationToken).ConfigureAwait(false);

            if (response.Files is { Count: > 0 } files)
            {
                foreach (var file in files)
                {
                    yield return MapFileToHostedFileContent(file);

                    yielded++;
                    if (limit is not null && yielded >= limit.Value)
                    {
                        yield break;
                    }
                }
            }

            pageToken = response.NextPageToken;
        }
        while (pageToken is not null);
    }

    /// <inheritdoc />
    public async Task<bool> DeleteAsync(
        string fileId,
        HostedFileClientOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(fileId);

        var resourceId = ExtractResourceId(fileId);

        try
        {
            await FilesClient.DeleteFileAsync(resourceId, cancellationToken).ConfigureAwait(false);
            return true;
        }
        catch (GeminiClientException ex) when (ex.Response.StatusCode is HttpStatusCode.NotFound)
        {
            return false;
        }
    }

    /// <inheritdoc />
    public object? GetService(Type serviceType, object? serviceKey = null)
    {
        ArgumentNullException.ThrowIfNull(serviceType);

        if (serviceKey is not null)
        {
            return null;
        }

        if (serviceType == typeof(HostedFileClientMetadata))
        {
            return _metadata;
        }

        if (serviceType == typeof(IGeminiClient))
        {
            return _client;
        }

        if (serviceType.IsInstanceOfType(this))
        {
            return this;
        }

        return null;
    }

    /// <inheritdoc />
    public void Dispose()
    {
        if (_ownsClient && _client is IDisposable disposable)
        {
            disposable.Dispose();
        }
    }

    // -- Mapping helpers --

    /// <summary>
    /// Extracts the resource ID segment from a Gemini file identifier.
    /// Accepts a full URI (e.g. <c>https://generativelanguage.googleapis.com/v1beta/files/abc123</c>),
    /// a resource name (<c>files/abc123</c>), or a bare ID (<c>abc123</c>).
    /// </summary>
    internal static string ExtractResourceId(string fileId)
    {
        // Full URI — take everything after the last '/'.
        if (fileId.StartsWith("https://", StringComparison.OrdinalIgnoreCase) ||
            fileId.StartsWith("http://", StringComparison.OrdinalIgnoreCase))
        {
            var lastSlash = fileId.LastIndexOf('/');
            return fileId[(lastSlash + 1)..];
        }

        // Resource name format: "files/{id}"
        const string prefix = "files/";
        if (fileId.StartsWith(prefix, StringComparison.Ordinal))
        {
            return fileId[prefix.Length..];
        }

        // Already a bare resource ID.
        return fileId;
    }

    internal static HostedFileContent MapFileToHostedFileContent(File file)
    {
        return new HostedFileContent(file.Uri ?? file.Name ?? string.Empty)
        {
            MediaType = file.MimeType,
            Name = file.DisplayName,
            SizeInBytes = file.SizeBytes,
            CreatedAt = file.CreateTime,
            RawRepresentation = file,
        };
    }
}

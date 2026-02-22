using GeminiDotnet.V1Beta.Files;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using File = GeminiDotnet.V1Beta.Files.File;

namespace GeminiDotnet.V1Beta;

internal sealed partial class FilesClient
{
    /// <inheritdoc />
    public async Task<File> UploadFileAsync(
        Stream content,
        long contentLength,
        UploadFileOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(content);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(contentLength);

        var mimeType = options?.MimeType ?? "application/octet-stream";

        // Step 1: Initiate the resumable upload to get an upload URL.
        //
        // The Gemini upload protocol is not in the OpenAPI spec. It uses a
        // separate /upload/ endpoint with custom X-Goog-Upload-* headers to
        // negotiate a resumable session.
        var uploadUrl = await InitiateResumableUploadAsync(
            mimeType, contentLength, options?.DisplayName, cancellationToken).ConfigureAwait(false);

        // Step 2: Upload the file bytes and finalize in a single request.
        return await UploadAndFinalizeAsync(
            uploadUrl, content, contentLength, mimeType, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Sends the initial POST to <c>/upload/v1beta/files</c> with upload protocol headers.
    /// Returns the upload URL from the <c>X-Goog-Upload-URL</c> response header.
    /// </summary>
    private async Task<string> InitiateResumableUploadAsync(
        string mimeType,
        long contentLength,
        string? displayName,
        CancellationToken cancellationToken)
    {
        using var initMessage = new HttpRequestMessage(HttpMethod.Post, "/upload/v1beta/files");

        initMessage.Headers.Add("X-Goog-Upload-Protocol", "resumable");
        initMessage.Headers.Add("X-Goog-Upload-Command", "start");
        initMessage.Headers.Add("X-Goog-Upload-Header-Content-Length", contentLength.ToString());
        initMessage.Headers.Add("X-Goog-Upload-Header-Content-Type", mimeType);

        // The body carries optional file metadata (display name, etc.)
        var metadataRequest = new CreateFileRequest
        {
            File = displayName is not null ? new File { DisplayName = displayName } : null,
        };

        initMessage.Content = JsonContent.Create(
            metadataRequest,
            V1BetaJsonContext.Default.CreateFileRequest);

        using var response = await _requester.SendAsync(initMessage, cancellationToken).ConfigureAwait(false);

        // The upload URL is returned in a response header, not in the body.
        if (!response.Headers.TryGetValues("X-Goog-Upload-URL", out var uploadUrls))
        {
            throw new InvalidOperationException(
                "The Gemini upload initiation response did not include an 'X-Goog-Upload-URL' header.");
        }

        return uploadUrls.First();
    }

    /// <summary>
    /// Sends the raw file bytes to the upload URL with finalize headers.
    /// The response body contains the completed <see cref="File"/> metadata.
    /// </summary>
    private async Task<File> UploadAndFinalizeAsync(
        string uploadUrl,
        Stream content,
        long contentLength,
        string mimeType,
        CancellationToken cancellationToken)
    {
        using var uploadMessage = new HttpRequestMessage(HttpMethod.Post, uploadUrl);

        uploadMessage.Headers.Add("X-Goog-Upload-Command", "upload, finalize");
        uploadMessage.Headers.Add("X-Goog-Upload-Offset", "0");

        var streamContent = new StreamContent(content);
        streamContent.Headers.ContentLength = contentLength;
        streamContent.Headers.ContentType = new MediaTypeHeaderValue(mimeType);
        uploadMessage.Content = streamContent;

        using var response = await _requester.SendAsync(uploadMessage, cancellationToken).ConfigureAwait(false);

        // The upload response wraps the file metadata in a "file" property, matching
        // the same envelope used by the CreateFile endpoint.
        var result = await response.Content
            .ReadFromJsonAsync(V1BetaJsonContext.Default.CreateFileResponse, cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return result?.File
            ?? throw new InvalidOperationException("The Gemini upload response did not contain valid file metadata.");
    }
}

using GeminiDotnet.V1Beta.Files;
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

        var request = new CreateFileRequest
        {
            File = options?.DisplayName is { } displayName ? new File { DisplayName = displayName } : null,
        };

        var media = new MediaContent(content, contentLength, options?.MimeType ?? "application/octet-stream");

        var response = await CreateFileAsync(request, media, cancellationToken).ConfigureAwait(false);

        return response.File
            ?? throw new InvalidOperationException("The Gemini upload response did not contain valid file metadata.");
    }
}

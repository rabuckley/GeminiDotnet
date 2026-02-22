using File = GeminiDotnet.V1Beta.Files.File;

namespace GeminiDotnet.V1Beta;

public partial interface IFilesClient
{
    /// <summary>
    /// Uploads a file using the Gemini resumable upload protocol and returns the resulting
    /// <see cref="File"/> metadata.
    /// </summary>
    /// <remarks>
    /// This implementation uses single-shot upload mode (the entire file is sent in one request).
    /// Chunked resumable uploads for large files are not yet supported.
    /// </remarks>
    /// <param name="content">The file content stream to upload.</param>
    /// <param name="contentLength">The total size of the file content in bytes.</param>
    /// <param name="options">Optional upload settings such as display name and MIME type.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    Task<File> UploadFileAsync(
        Stream content,
        long contentLength,
        UploadFileOptions? options = null,
        CancellationToken cancellationToken = default);
}

namespace GeminiDotnet.V1Beta;

/// <summary>
/// Options for uploading a file via the resumable upload protocol.
/// </summary>
public sealed record UploadFileOptions
{
    /// <summary>
    /// Optional human-readable display name for the file.
    /// </summary>
    public string? DisplayName { get; init; }

    /// <summary>
    /// The IANA MIME type of the file content (e.g. "text/csv", "application/pdf").
    /// Defaults to "application/octet-stream" if not specified.
    /// </summary>
    public string? MimeType { get; init; }
}

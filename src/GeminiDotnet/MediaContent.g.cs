namespace GeminiDotnet;

/// <summary>
/// The bytes an upload sends, with the length and media type the upload declares before it
/// sends them.
/// </summary>
public sealed class MediaContent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MediaContent"/> class.
    /// </summary>
    /// <param name="stream">
    /// The bytes to upload, read from the stream's current position. The caller keeps
    /// ownership of the stream and disposes it once the upload completes.
    /// </param>
    /// <param name="length">The number of bytes to read from <paramref name="stream"/>.</param>
    /// <param name="mimeType">The IANA media type of the bytes, such as <c>application/pdf</c>.</param>
    /// <exception cref="ArgumentNullException"><paramref name="stream"/> or <paramref name="mimeType"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException"><paramref name="mimeType"/> is empty or consists only of white-space characters.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="length"/> is negative.</exception>
    public MediaContent(Stream stream, long length, string mimeType)
    {
        ArgumentNullException.ThrowIfNull(stream);
        ArgumentOutOfRangeException.ThrowIfNegative(length);
        ArgumentException.ThrowIfNullOrWhiteSpace(mimeType);

        Stream = stream;
        Length = length;
        MimeType = mimeType;
    }

    /// <summary>
    /// Gets the stream the bytes are read from.
    /// </summary>
    public Stream Stream { get; }

    /// <summary>
    /// Gets the number of bytes to upload.
    /// </summary>
    public long Length { get; }

    /// <summary>
    /// Gets the IANA media type of the bytes.
    /// </summary>
    public string MimeType { get; }
}

// The ".g.cs" suffix makes the compiler treat this as generated code, where a
// nullable annotation needs the context turned on in the file itself (CS8669).
#nullable enable

namespace GeminiDotnet;

/// <summary>
/// The bytes a download returns, read as they arrive, with what the response declared
/// about them.
/// </summary>
/// <remarks>
/// A download holds its response open until it is disposed, so dispose it once the bytes
/// have been read.
/// </remarks>
public sealed class MediaDownload : IDisposable, IAsyncDisposable
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MediaDownload"/> class.
    /// </summary>
    /// <param name="stream">
    /// The response body. The download takes ownership of the stream and disposes it.
    /// </param>
    /// <param name="mimeType">
    /// The IANA media type the response declared, or <see langword="null"/> when it declared none.
    /// </param>
    /// <param name="length">
    /// The number of bytes the response declared, or <see langword="null"/> when it declared none.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="stream"/> is <see langword="null"/>.</exception>
    public MediaDownload(Stream stream, string? mimeType, long? length)
    {
        ArgumentNullException.ThrowIfNull(stream);

        Stream = stream;
        MimeType = mimeType;
        Length = length;
    }

    /// <summary>
    /// Gets the stream the bytes are read from.
    /// </summary>
    public Stream Stream { get; }

    /// <summary>
    /// Gets the IANA media type of the bytes, or <see langword="null"/> when the response
    /// declared none.
    /// </summary>
    public string? MimeType { get; }

    /// <summary>
    /// Gets the number of bytes, or <see langword="null"/> when the response declared none.
    /// </summary>
    public long? Length { get; }

    /// <inheritdoc />
    public void Dispose()
    {
        Stream.Dispose();
    }

    /// <inheritdoc />
    public ValueTask DisposeAsync()
    {
        return Stream.DisposeAsync();
    }
}

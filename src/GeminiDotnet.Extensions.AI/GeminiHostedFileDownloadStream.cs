using Microsoft.Extensions.AI;

#pragma warning disable MEAI001 // Type is for evaluation purposes only

namespace GeminiDotnet.Extensions.AI;

/// <summary>
/// A <see cref="HostedFileDownloadStream"/> that wraps a Gemini <see cref="MediaDownload"/>,
/// delegating all stream operations to its stream.
/// </summary>
internal sealed class GeminiHostedFileDownloadStream : HostedFileDownloadStream
{
    private readonly MediaDownload _download;
    private readonly string? _fileName;

    /// <param name="download">The download, owned by this stream and disposed with it.</param>
    /// <param name="fileName">The display name of the file, if known.</param>
    internal GeminiHostedFileDownloadStream(MediaDownload download, string? fileName)
    {
        _download = download;
        _fileName = fileName;
    }

    private Stream ContentStream => _download.Stream;

    /// <inheritdoc />
    public override string? MediaType => _download.MimeType;

    /// <inheritdoc />
    public override string? FileName => _fileName;

    /// <inheritdoc />
    public override bool CanRead => ContentStream.CanRead;

    /// <inheritdoc />
    public override bool CanSeek => ContentStream.CanSeek;

    /// <inheritdoc />
    // The download is not buffered, so its stream cannot seek; the response's Content-Length is
    // the only length there is.
    public override long Length => _download.Length ?? ContentStream.Length;

    /// <inheritdoc />
    public override long Position
    {
        get => ContentStream.Position;
        set => ContentStream.Position = value;
    }

    /// <inheritdoc />
    public override void Flush() => ContentStream.Flush();

    /// <inheritdoc />
    public override int Read(byte[] buffer, int offset, int count) =>
        ContentStream.Read(buffer, offset, count);

    /// <inheritdoc />
    public override int Read(Span<byte> buffer) =>
        ContentStream.Read(buffer);

    /// <inheritdoc />
    public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken) =>
        ContentStream.ReadAsync(buffer, offset, count, cancellationToken);

    /// <inheritdoc />
    public override ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken = default) =>
        ContentStream.ReadAsync(buffer, cancellationToken);

    /// <inheritdoc />
    public override Task CopyToAsync(Stream destination, int bufferSize, CancellationToken cancellationToken) =>
        ContentStream.CopyToAsync(destination, bufferSize, cancellationToken);

    /// <inheritdoc />
    public override long Seek(long offset, SeekOrigin origin) =>
        ContentStream.Seek(offset, origin);

    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _download.Dispose();
        }

        base.Dispose(disposing);
    }

    /// <inheritdoc />
    public override async ValueTask DisposeAsync()
    {
        await _download.DisposeAsync().ConfigureAwait(false);
        await base.DisposeAsync().ConfigureAwait(false);
        GC.SuppressFinalize(this);
    }
}

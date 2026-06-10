using GeminiDotnet.V1Beta.Files;
using Microsoft.Extensions.AI;

#pragma warning disable MEAI001 // Type is for evaluation purposes only

namespace GeminiDotnet.Extensions.AI;

/// <summary>
/// A <see cref="HostedFileDownloadStream"/> that wraps a Gemini <see cref="FileDownloadResult"/>,
/// delegating all stream operations to the underlying content stream.
/// </summary>
internal sealed class GeminiHostedFileDownloadStream : HostedFileDownloadStream
{
    private readonly FileDownloadResult _result;
    private readonly Stream _contentStream;
    private readonly string? _fileName;

    /// <param name="result">The download result, retained for metadata and disposal.</param>
    /// <param name="contentStream">The eagerly-resolved content stream from <paramref name="result"/>.</param>
    /// <param name="fileName">The display name of the file, if known.</param>
    internal GeminiHostedFileDownloadStream(FileDownloadResult result, Stream contentStream, string? fileName)
    {
        _result = result;
        _contentStream = contentStream;
        _fileName = fileName;
    }

    /// <inheritdoc />
    public override string? MediaType => _result.MediaType;

    /// <inheritdoc />
    public override string? FileName => _fileName;

    /// <inheritdoc />
    public override bool CanRead => _contentStream.CanRead;

    /// <inheritdoc />
    public override bool CanSeek => _contentStream.CanSeek;

    /// <inheritdoc />
    public override long Length => _contentStream.Length;

    /// <inheritdoc />
    public override long Position
    {
        get => _contentStream.Position;
        set => _contentStream.Position = value;
    }

    /// <inheritdoc />
    public override void Flush() => _contentStream.Flush();

    /// <inheritdoc />
    public override int Read(byte[] buffer, int offset, int count) =>
        _contentStream.Read(buffer, offset, count);

    /// <inheritdoc />
    public override int Read(Span<byte> buffer) =>
        _contentStream.Read(buffer);

    /// <inheritdoc />
    public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken) =>
        _contentStream.ReadAsync(buffer, offset, count, cancellationToken);

    /// <inheritdoc />
    public override ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken = default) =>
        _contentStream.ReadAsync(buffer, cancellationToken);

    /// <inheritdoc />
    public override Task CopyToAsync(Stream destination, int bufferSize, CancellationToken cancellationToken) =>
        _contentStream.CopyToAsync(destination, bufferSize, cancellationToken);

    /// <inheritdoc />
    public override long Seek(long offset, SeekOrigin origin) =>
        _contentStream.Seek(offset, origin);

    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _result.Dispose();
        }

        base.Dispose(disposing);
    }

    /// <inheritdoc />
    public override async ValueTask DisposeAsync()
    {
        await _result.DisposeAsync().ConfigureAwait(false);
        await base.DisposeAsync().ConfigureAwait(false);
        GC.SuppressFinalize(this);
    }
}

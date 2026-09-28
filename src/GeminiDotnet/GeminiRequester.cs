using GeminiDotnet.Text.Json;
using System.Globalization;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net.ServerSentEvents;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GeminiDotnet;

internal sealed class GeminiRequester : IGeminiRequester
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerContext _jsonSerializerContext;

    public GeminiRequester(
        HttpClient httpClient,
        JsonSerializerContext jsonSerializerContext)
    {
        ArgumentNullException.ThrowIfNull(httpClient);
        ArgumentNullException.ThrowIfNull(jsonSerializerContext);
        _httpClient = httpClient;
        _jsonSerializerContext = jsonSerializerContext;
    }

    public async Task<TResponse> ExecuteAsync<TResponse>(
        HttpMethod method,
        string path,
        CancellationToken cancellationToken = default)
    {
        using var message = new HttpRequestMessage(method, path);
        return await ExecuteAsync<TResponse>(message, cancellationToken).ConfigureAwait(false);
    }

    public async Task<TResponse> ExecuteAsync<TRequest, TResponse>(
        HttpMethod method,
        string path,
        TRequest request,
        CancellationToken cancellationToken = default)
    {
        var requestJsonTypeInfo = _jsonSerializerContext.GetTypeInfo<TRequest>();

        using var message = new HttpRequestMessage(method, path);
        message.Content = JsonContent.Create(request, requestJsonTypeInfo);

        return await ExecuteAsync<TResponse>(message, cancellationToken).ConfigureAwait(false);
    }

    private async Task<TResponse> ExecuteAsync<TResponse>(
        HttpRequestMessage message,
        CancellationToken cancellationToken)
    {
        var responseJsonTypeInfo = _jsonSerializerContext.GetTypeInfo<TResponse>();

        var response = await _httpClient.SendAsync(message, cancellationToken).ConfigureAwait(false);

        response = await EnsureSuccessOrThrow(response, cancellationToken).ConfigureAwait(false);

        var result = await response.Content
            .ReadFromJsonAsync(responseJsonTypeInfo, cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return result!;
    }

    public async IAsyncEnumerable<TResponse> ExecuteStreamingAsync<TResponse>(
        HttpMethod method,
        string path,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        using var message = new HttpRequestMessage(method, path);

        await foreach (var item in ExecuteStreamingAsync<TResponse>(message, cancellationToken).ConfigureAwait(false))
        {
            yield return item;
        }
    }

    public async IAsyncEnumerable<TResponse> ExecuteStreamingAsync<TRequest, TResponse>(
        HttpMethod method,
        string path,
        TRequest request,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        using var message = new HttpRequestMessage(method, path);
        var requestJsonTypeInfo = _jsonSerializerContext.GetTypeInfo<TRequest>();
        message.Content = JsonContent.Create(request, requestJsonTypeInfo);

        await foreach (var item in ExecuteStreamingAsync<TResponse>(message, cancellationToken).ConfigureAwait(false))
        {
            yield return item;
        }
    }

    private async IAsyncEnumerable<TResponse> ExecuteStreamingAsync<TResponse>(
        HttpRequestMessage message,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var response = await _httpClient
            .SendAsync(message, HttpCompletionOption.ResponseHeadersRead, cancellationToken)
            .ConfigureAwait(false);

        response = await EnsureSuccessOrThrow(response, cancellationToken).ConfigureAwait(false);

        var stream = await response.Content
            .ReadAsStreamAsync(cancellationToken)
            .ConfigureAwait(false);

        var parser = SseParser.Create(stream, ParseSseItem);

        await foreach (var item in parser.EnumerateAsync(cancellationToken).ConfigureAwait(false))
        {
            yield return item.Data;
        }

        TResponse ParseSseItem(string eventType, ReadOnlySpan<byte> data)
            => JsonSerializer.Deserialize(data, _jsonSerializerContext.GetTypeInfo<TResponse>())!;
    }

    public async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage message,
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.SendAsync(message, cancellationToken).ConfigureAwait(false);
        try
        {
            return await EnsureSuccessOrThrow(response, cancellationToken).ConfigureAwait(false);
        }
        catch
        {
            response.Dispose();
            throw;
        }
    }

    public async Task<TResponse> UploadAsync<TMetadata, TResponse>(
        HttpMethod method,
        string path,
        TMetadata metadata,
        MediaContent media,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(media);

        var uploadUrl = await StartResumableUploadAsync(method, path, metadata, media, cancellationToken)
            .ConfigureAwait(false);

        // The whole file goes in one request, which also finalizes the upload. Resuming an
        // interrupted upload from its last offset is not supported.
        using var message = new HttpRequestMessage(HttpMethod.Post, uploadUrl);
        message.Headers.Add("X-Goog-Upload-Command", "upload, finalize");
        message.Headers.Add("X-Goog-Upload-Offset", "0");

        var content = new BorrowedStreamContent(media.Stream, media.Length);
        content.Headers.ContentType = MediaTypeHeaderValue.Parse(media.MimeType);
        message.Content = content;

        return await ExecuteAsync<TResponse>(message, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Opens a resumable upload session and returns the URL its bytes are sent to, which the API
    /// returns in a header rather than in the body.
    /// </summary>
    private async Task<string> StartResumableUploadAsync<TMetadata>(
        HttpMethod method,
        string path,
        TMetadata metadata,
        MediaContent media,
        CancellationToken cancellationToken)
    {
        using var message = new HttpRequestMessage(method, path);
        message.Headers.Add("X-Goog-Upload-Protocol", "resumable");
        message.Headers.Add("X-Goog-Upload-Command", "start");
        message.Headers.Add("X-Goog-Upload-Header-Content-Length", media.Length.ToString(CultureInfo.InvariantCulture));
        message.Headers.Add("X-Goog-Upload-Header-Content-Type", media.MimeType);
        message.Content = JsonContent.Create(metadata, _jsonSerializerContext.GetTypeInfo<TMetadata>());

        using var response = await SendAsync(message, cancellationToken).ConfigureAwait(false);

        if (!response.Headers.TryGetValues("X-Goog-Upload-URL", out var uploadUrls))
        {
            throw new InvalidOperationException(
                "The Gemini upload start response did not include an 'X-Goog-Upload-URL' header.");
        }

        return uploadUrls.First();
    }

    public async Task<MediaDownload> DownloadAsync(
        HttpMethod method,
        string path,
        CancellationToken cancellationToken = default)
    {
        using var message = new HttpRequestMessage(method, path);

        var response = await _httpClient
            .SendAsync(message, HttpCompletionOption.ResponseHeadersRead, cancellationToken)
            .ConfigureAwait(false);

        try
        {
            response = await EnsureSuccessOrThrow(response, cancellationToken).ConfigureAwait(false);

            // Disposing the content stream of an unbuffered response is what releases its
            // connection, so the download owning the stream is enough.
            var stream = await response.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);

            return new MediaDownload(
                stream,
                response.Content.Headers.ContentType?.MediaType,
                response.Content.Headers.ContentLength);
        }
        catch
        {
            response.Dispose();
            throw;
        }
    }

    private async Task<HttpResponseMessage> EnsureSuccessOrThrow(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        if (response.IsSuccessStatusCode)
        {
            return response;
        }

        var errorResponseTypeInfo = _jsonSerializerContext.GetTypeInfo<ErrorResponse>();

        try
        {
            var errorResponse = await response.Content.ReadFromJsonAsync(
                errorResponseTypeInfo,
                cancellationToken).ConfigureAwait(false);

            GeminiClientException.Throw(errorResponse!.Error);
            return null!; // unreachable
        }
        catch (JsonException)
        {
        }

        // Fall back to throwing the HttpRequestException
        response.EnsureSuccessStatusCode();
        return null!; // unreachable
    }

    /// <summary>
    /// Sends a stream the caller still owns. <see cref="StreamContent"/> would dispose it along
    /// with the request message.
    /// </summary>
    private sealed class BorrowedStreamContent(Stream stream, long length) : HttpContent
    {
        protected override Task SerializeToStreamAsync(Stream target, TransportContext? context)
            => stream.CopyToAsync(target);

        protected override Task SerializeToStreamAsync(Stream target, TransportContext? context, CancellationToken cancellationToken)
            => stream.CopyToAsync(target, cancellationToken);

        protected override bool TryComputeLength(out long computedLength)
        {
            computedLength = length;
            return true;
        }
    }
}

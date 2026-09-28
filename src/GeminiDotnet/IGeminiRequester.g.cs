namespace GeminiDotnet;

internal interface IGeminiRequester
{
    Task<TResponse> ExecuteAsync<TResponse>(
        HttpMethod method,
        string path,
        CancellationToken cancellationToken = default);

    Task<TResponse> ExecuteAsync<TRequest, TResponse>(
        HttpMethod method,
        string path,
        TRequest request,
        CancellationToken cancellationToken = default);

    IAsyncEnumerable<TResponse> ExecuteStreamingAsync<TResponse>(
        HttpMethod method,
        string path,
        CancellationToken cancellationToken = default);

    IAsyncEnumerable<TResponse> ExecuteStreamingAsync<TRequest, TResponse>(
        HttpMethod method,
        string path,
        TRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Sends an arbitrary <see cref="HttpRequestMessage"/> and returns the raw response, for
    /// protocols a generated method cannot express: custom headers, a non-JSON body, or a
    /// response that is not deserialized.
    /// </summary>
    /// <remarks>
    /// The caller owns the returned <see cref="HttpResponseMessage"/> and must dispose it. The
    /// response body is buffered, as it is for <see cref="ExecuteAsync{TResponse}"/>; for a
    /// response read as it arrives, use <see cref="ExecuteStreamingAsync{TRequest, TResponse}"/>.
    /// </remarks>
    Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage message,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Uploads <paramref name="media"/> with Google's resumable upload protocol, sending
    /// <paramref name="metadata"/> alongside it, and returns the response to the request
    /// that finalizes the upload.
    /// </summary>
    /// <remarks>
    /// The upload takes two requests. The first is a <paramref name="method"/> request to
    /// <paramref name="path"/> with <paramref name="metadata"/> as its JSON body and the
    /// headers <c>X-Goog-Upload-Protocol: resumable</c>, <c>X-Goog-Upload-Command: start</c>,
    /// <c>X-Goog-Upload-Header-Content-Length</c> and <c>X-Goog-Upload-Header-Content-Type</c>,
    /// the last two carrying <see cref="MediaContent.Length"/> and
    /// <see cref="MediaContent.MimeType"/>. Its response's <c>X-Goog-Upload-URL</c> header
    /// names where to <c>POST</c> the bytes, with <c>X-Goog-Upload-Command: upload, finalize</c>
    /// and <c>X-Goog-Upload-Offset: 0</c>; that response's JSON body is the
    /// <typeparamref name="TResponse"/>.
    /// </remarks>
    /// <param name="method">
    /// The method the first request uses, which is the operation's own: the environment
    /// file upload is routed only for <c>PUT</c>.
    /// </param>
    /// <param name="path">
    /// The upload path, which is not the operation's path but that path under a prefix
    /// such as <c>/upload</c>.
    /// </param>
    /// <param name="metadata">The operation's request, which describes the upload.</param>
    /// <param name="media">The bytes to upload.</param>
    /// <param name="cancellationToken">A token to cancel the upload.</param>
    Task<TResponse> UploadAsync<TMetadata, TResponse>(
        HttpMethod method,
        string path,
        TMetadata metadata,
        MediaContent media,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Sends a <paramref name="method"/> request to <paramref name="path"/> and returns its
    /// response body as the bytes of a <see cref="MediaDownload"/>, without buffering them.
    /// </summary>
    /// <remarks>
    /// <paramref name="path"/> already carries <c>alt=media</c>, which is what makes the API
    /// answer with the bytes rather than with JSON. The returned download owns the response:
    /// disposing its <see cref="MediaDownload.Stream"/> has to release it. An unsuccessful
    /// response throws, as it does for <see cref="ExecuteAsync{TResponse}"/>.
    /// </remarks>
    /// <param name="method">The method of the operation that serves the bytes.</param>
    /// <param name="path">The operation's path, with <c>alt=media</c> in its query.</param>
    /// <param name="cancellationToken">A token to cancel the request.</param>
    Task<MediaDownload> DownloadAsync(
        HttpMethod method,
        string path,
        CancellationToken cancellationToken = default);
}

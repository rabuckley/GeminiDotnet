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
}

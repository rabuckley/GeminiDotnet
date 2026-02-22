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

    IAsyncEnumerable<TResponse> ExecuteStreamingAsync<TRequest, TResponse>(
        HttpMethod method,
        string path,
        TRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Sends an arbitrary <see cref="HttpRequestMessage"/> and returns the raw response.
    /// Use this for protocols that require custom headers or non-JSON request bodies
    /// (e.g. the resumable file upload protocol).
    /// </summary>
    /// <remarks>
    /// The caller is responsible for disposing the returned <see cref="HttpResponseMessage"/>.
    /// This method buffers the entire response body (uses <see cref="HttpCompletionOption.ResponseContentRead"/>),
    /// matching the non-streaming <see cref="ExecuteAsync{TResponse}"/> convention. For streaming
    /// responses, use <see cref="ExecuteStreamingAsync{TRequest, TResponse}"/> instead.
    /// </remarks>
    Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage message,
        CancellationToken cancellationToken = default);
}

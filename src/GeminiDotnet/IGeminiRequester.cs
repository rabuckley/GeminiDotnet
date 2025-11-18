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
}

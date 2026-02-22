using GeminiDotnet.Text.Json;
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
}

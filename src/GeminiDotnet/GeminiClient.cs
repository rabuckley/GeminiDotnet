using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Net.ServerSentEvents;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using System.Threading;
using System.Threading.Tasks;

using GeminiDotnet.ContentGeneration;
using GeminiDotnet.Embeddings;
using GeminiDotnet.Text.Json;

namespace GeminiDotnet;

public sealed class GeminiClient
{
    private const string BaseUrl = "https://generativelanguage.googleapis.com";

    private readonly HttpClient _httpClient;

    public GeminiClient(GeminiClientOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        _httpClient = new HttpClient { BaseAddress = new Uri(BaseUrl) };
        Options = options;
    }

    public GeminiClient(HttpClient httpClient, GeminiClientOptions options)
    {
        ArgumentNullException.ThrowIfNull(httpClient);
        ArgumentNullException.ThrowIfNull(options);

        _httpClient = httpClient;
        Options = options;
    }

    /// <summary>
    /// The options that this client is configured with.
    /// </summary>
    public GeminiClientOptions Options { get; }

    public Uri? Endpoint => _httpClient.BaseAddress;

    // ```
    // curl "https://generativelanguage.googleapis.com/v1beta/models/gemini-pro:generateContent?key=${GOOGLE_API_KEY}" \
    // -H 'Content-Type: application/json' \
    // -d '{ "contents":[{"parts":[{"text": "Write a cute story about cats."}]}]}'
    // ```
    public async Task<GenerateContentResponse> GenerateContentAsync(
        string model,
        GenerateContentRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(model);
        ArgumentNullException.ThrowIfNull(request);

        var requestJsonInfo = JsonContext.Default.GetTypeInfo<GenerateContentRequest>();

        var uri = $"/{Options.ApiVersion}/models/{model}:generateContent?key={Options.ApiKey}";

        var response = await _httpClient
            .PostAsJsonAsync(uri, request, requestJsonInfo, cancellationToken: cancellationToken).ConfigureAwait(false);

        response.EnsureSuccessStatusCode();

        var responseJsonInfo = JsonContext.Default.GetTypeInfo<GenerateContentResponse>();

        var responseJson = await response.Content
            .ReadFromJsonAsync(responseJsonInfo, cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return responseJson ?? throw new InvalidOperationException("Response body was null.");
    }

    // ```
    // curl "https://generativelanguage.googleapis.com/v1beta/models/gemini-pro:streamGenerateContent?alt=sse&key=${GOOGLE_API_KEY}" \
    // -H 'Content-Type: application/json' \
    // --no-buffer \
    // -d '{ "contents":[{"parts":[{"text": "Write a cute story about cats."}]}]}'
    // ```
    public async IAsyncEnumerable<StreamingTextGenerationResponse> GenerateContentStreamingAsync(
        string model,
        GenerateContentRequest request,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(model);
        ArgumentNullException.ThrowIfNull(request);

        var requestJsonInfo = JsonContext.Default.GetTypeInfo<GenerateContentRequest>();

        var uri = $"/{Options.ApiVersion}/models/{model}:streamGenerateContent?alt=sse&key={Options.ApiKey}";

        var response = await _httpClient.PostAsJsonAsync(
                uri,
                request,
                requestJsonInfo,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        response.EnsureSuccessStatusCode();

        var stream = await response.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);
        var sseParser = SseParser.Create(stream, ParseSseItem);

        await foreach (var item in sseParser.EnumerateAsync(cancellationToken))
        {
            yield return item.Data;
        }
    }

    private static StreamingTextGenerationResponse ParseSseItem(string eventType, ReadOnlySpan<byte> data)
    {
        var typeInfo = JsonContext.Default.GetTypeInfo<StreamingTextGenerationResponse>();
        var response = JsonSerializer.Deserialize(data, typeInfo);
        return response ?? throw new InvalidOperationException("SSE response body was null.");
    }

    // ```
    // curl "https://generativelanguage.googleapis.com/v1beta/models/text-embedding-004:embedContent?key=$GEMINI_API_KEY" \
    // -H 'Content-Type: application/json' \
    // -d '{
    // "model": "models/text-embedding-004",
    // "content": { "parts":[{ "text": "What is the meaning of life?" }] }
    // }'
    // ```
    public async Task<EmbeddingResponse> EmbedContentAsync(
        string model,
        EmbeddingRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(model);
        ArgumentNullException.ThrowIfNull(request);

        var requestJsonInfo = JsonContext.Default.GetTypeInfo<EmbeddingRequest>();

        var uri = $"/{Options.ApiVersion}/models/{model}:embedContent?key={Options.ApiKey}";

        var response = await _httpClient.PostAsJsonAsync(
                uri,
                request,
                requestJsonInfo,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        response.EnsureSuccessStatusCode();

        var responseJsonInfo = JsonContext.Default.GetTypeInfo<EmbeddingResponse>();

        var responseJson = await response.Content
            .ReadFromJsonAsync(responseJsonInfo, cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return responseJson ?? throw new InvalidOperationException("Response body was null.");
    }
}
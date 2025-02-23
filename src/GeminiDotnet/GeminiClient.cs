using GeminiDotnet.ContentGeneration;
using GeminiDotnet.Embeddings;
using GeminiDotnet.Text.Json;
using System.Diagnostics;
using System.Net.Http.Json;
using System.Net.ServerSentEvents;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

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

        var uri = new Uri($"/{Options.ApiVersion}/models/{model}:generateContent?key={Options.ApiKey}",
            UriKind.Relative);

        var response = await GenerateContentCore(uri, request, cancellationToken).ConfigureAwait(false);

        var responseJsonInfo = JsonContext.Default.GetTypeInfo<GenerateContentResponse>();

        var responseJson = await response.Content
            .ReadFromJsonAsync(responseJsonInfo, cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return responseJson!;
    }

    // ```
    // curl "https://generativelanguage.googleapis.com/v1beta/models/gemini-pro:streamGenerateContent?alt=sse&key=${GOOGLE_API_KEY}" \
    // -H 'Content-Type: application/json' \
    // --no-buffer \
    // -d '{ "contents":[{"parts":[{"text": "Write a cute story about cats."}]}]}'
    // ```
    public async IAsyncEnumerable<GenerateContentResponse> GenerateContentStreamingAsync(
        string model,
        GenerateContentRequest request,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(model);
        ArgumentNullException.ThrowIfNull(request);

        var uri = new Uri($"/{Options.ApiVersion}/models/{model}:streamGenerateContent?alt=sse&key={Options.ApiKey}",
            UriKind.Relative);

        var response = await GenerateContentCore(uri, request, cancellationToken).ConfigureAwait(false);

        var stream = await response.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);
        var sseParser = SseParser.Create(stream, ParseSseItem);

        await foreach (var item in sseParser.EnumerateAsync(cancellationToken).ConfigureAwait(false))
        {
            yield return item.Data;
        }

        static GenerateContentResponse ParseSseItem(string eventType, ReadOnlySpan<byte> data)
        {
            var typeInfo = JsonContext.Default.GetTypeInfo<GenerateContentResponse>();
            var response = JsonSerializer.Deserialize(data, typeInfo);
            return response!;
        }
    }

    // ```
    // curl "https://generativelanguage.googleapis.com/v1beta/models/text-embedding-004:embedContent?key=$GEMINI_API_KEY" \
    // -H 'Content-Type: application/json' \
    // -d '{
    // "model": "models/text-embedding-004",
    // "content": { "parts":[{ "text": "What is the meaning of life?" }] }
    // }'
    // ```
    public async Task<EmbedContentResponse> EmbedContentAsync(
        string model,
        EmbedContentRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(model);
        ArgumentNullException.ThrowIfNull(request);

        var uri = new Uri($"/{Options.ApiVersion}/models/{model}:embedContent?key={Options.ApiKey}", UriKind.Relative);

        var requestJsonInfo = JsonContext.Default.GetTypeInfo<EmbedContentRequest>();
        var response = await ExecuteAction(uri, request, requestJsonInfo, cancellationToken).ConfigureAwait(false);

        var responseJsonInfo = JsonContext.Default.GetTypeInfo<EmbedContentResponse>();

        var responseJson = await response.Content
            .ReadFromJsonAsync(responseJsonInfo, cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return responseJson!;
    }

    private ValueTask<HttpResponseMessage> GenerateContentCore(
        Uri uri,
        GenerateContentRequest request,
        CancellationToken cancellationToken)
    {
        var requestJsonInfo = JsonContext.Default.GetTypeInfo<GenerateContentRequest>();
        return ExecuteAction(uri, request, requestJsonInfo, cancellationToken);
    }

    private async ValueTask<HttpResponseMessage> ExecuteAction<TRequest>(
        Uri uri,
        TRequest request,
        JsonTypeInfo<TRequest> requestJsonInfo,
        CancellationToken cancellationToken)
    {
        Debug.Assert(uri is not null);
        Debug.Assert(request is not null);

        var response = await _httpClient.PostAsJsonAsync(
            uri,
            request,
            requestJsonInfo,
            cancellationToken).ConfigureAwait(false);

        if ((int)response.StatusCode is >= 400 and < 500)
        {
            var errorResponseTypeInfo = JsonContext.Default.GetTypeInfo<ErrorResponse>();

        try
        {
            var errorResponse = await response.Content.ReadFromJsonAsync(
                errorResponseTypeInfo,
                cancellationToken).ConfigureAwait(false);

            GeminiClientException.Throw(errorResponse!.Error);
            return null!; // unreachable
        }

        return response;
    }
}

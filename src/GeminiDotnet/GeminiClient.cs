using GeminiDotnet.V1;
using GeminiDotnet.V1Beta;

namespace GeminiDotnet;

public sealed class GeminiClient : IGeminiClient
{
    private readonly HttpClient _httpClient;

    public GeminiClient(GeminiClientOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        var httpClient = new HttpClient { BaseAddress = options.Endpoint };

        httpClient.DefaultRequestHeaders.Add("x-goog-api-key", options.ApiKey);

        _httpClient = httpClient;
        Options = options;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="GeminiClient"/> class.
    /// </summary>
    /// <remarks>
    /// The provided <see cref="HttpClient"/> must be pre-configured with the appropriate base address and headers.
    /// </remarks>
    /// <param name="httpClient">The preconfigured HttpClient</param>
    /// <param name="modelId">Optional default model ID to use when not specified per-request</param>
    public GeminiClient(HttpClient httpClient, string? modelId = null)
    {
        ArgumentNullException.ThrowIfNull(httpClient);
        _httpClient = httpClient;
        Options = new GeminiClientOptions { Endpoint = httpClient.BaseAddress, ApiKey = null!, ModelId = modelId };
    }

    /// <summary>
    /// The options that this client is configured with.
    /// </summary>
    public IGeminiClientOptions Options { get; }

    public Uri? Endpoint => _httpClient.BaseAddress;

    public IGeminiV1Client V1 => field ??= new GeminiV1Client(new GeminiRequester(_httpClient, V1JsonContext.Default));

    public IGeminiV1BetaClient V1Beta =>
        field ??= new GeminiV1BetaClient(new GeminiRequester(_httpClient, V1BetaJsonContext.Default));
}

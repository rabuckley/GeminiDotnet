using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Text;

namespace GeminiDotnet.Testing;

/// <summary>
/// Answers each request with the next scripted response, in order, and records what was sent, as an
/// offline stand-in for the Gemini API.
/// </summary>
/// <remarks>
/// A request that arrives after the script has run out is still recorded, and its send throws an
/// <see cref="InvalidOperationException"/> that names it. Scripting and sending may happen concurrently.
/// </remarks>
public sealed class ScriptedHttpMessageHandler : HttpMessageHandler
{
    /// <summary>
    /// Gets the base address that <see cref="CreateHttpClient"/> uses when none is given.
    /// </summary>
    public static readonly Uri DefaultEndpoint = new("https://generativelanguage.googleapis.com");

    private readonly Queue<Func<RecordedRequest, HttpResponseMessage>> _responses = new();
    private readonly List<RecordedRequest> _requests = [];
    private readonly object _gate = new();

    /// <summary>
    /// Gets a snapshot of the requests received so far, in the order they arrived.
    /// </summary>
    public IReadOnlyList<RecordedRequest> Requests
    {
        get
        {
            lock (_gate)
            {
                return [.. _requests];
            }
        }
    }

    /// <summary>
    /// Queues a response with a JSON body.
    /// </summary>
    /// <param name="json">The response body.</param>
    /// <param name="statusCode">The status code of the response.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    public ScriptedHttpMessageHandler RespondWithJson(
        [StringSyntax(StringSyntaxAttribute.Json)] string json,
        HttpStatusCode statusCode = HttpStatusCode.OK)
    {
        return RespondWith(() => new HttpResponseMessage(statusCode)
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json"),
        });
    }

    /// <summary>
    /// Queues a <c>text/event-stream</c> response that carries one <c>data:</c> event per payload, the way the
    /// API streams with <c>alt=sse</c>.
    /// </summary>
    /// <param name="dataPayloads">The event payloads, which may span several lines.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="dataPayloads"/> is <see langword="null"/>.</exception>
    public ScriptedHttpMessageHandler RespondWithServerSentEvents(params IEnumerable<string> dataPayloads)
    {
        ArgumentNullException.ThrowIfNull(dataPayloads);

        var stream = new StringBuilder();
        foreach (var payload in dataPayloads)
        {
            // A client joins consecutive data lines with a line feed, so this round-trips a multi-line payload.
            foreach (var line in payload.ReplaceLineEndings("\n").Split('\n'))
            {
                stream.Append("data: ").Append(line).Append("\r\n");
            }

            stream.Append("\r\n");
        }

        var body = stream.ToString();
        return RespondWith(() => new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(body, Encoding.UTF8, "text/event-stream"),
        });
    }

    /// <summary>
    /// Queues a response that is built when its request arrives.
    /// </summary>
    /// <param name="createResponse">The function that builds the response.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="createResponse"/> is <see langword="null"/>.</exception>
    public ScriptedHttpMessageHandler RespondWith(Func<HttpResponseMessage> createResponse)
    {
        ArgumentNullException.ThrowIfNull(createResponse);

        return RespondWith(_ => createResponse());
    }

    /// <summary>
    /// Queues a response that is built from the request it answers.
    /// </summary>
    /// <param name="createResponse">The function that builds the response for the request.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="createResponse"/> is <see langword="null"/>.</exception>
    /// <remarks>
    /// The response is still the next one in the script; the request only shapes it. Use this when concurrent
    /// requests may arrive in either order.
    /// </remarks>
    public ScriptedHttpMessageHandler RespondWith(Func<RecordedRequest, HttpResponseMessage> createResponse)
    {
        ArgumentNullException.ThrowIfNull(createResponse);

        lock (_gate)
        {
            _responses.Enqueue(createResponse);
        }

        return this;
    }

    /// <summary>
    /// Creates an <see cref="HttpClient"/> that sends through this handler, configured with the base address
    /// and API key header that a Gemini client expects of a preconfigured <see cref="HttpClient"/>.
    /// </summary>
    /// <param name="endpoint">The base address, or <see langword="null"/> to use <see cref="DefaultEndpoint"/>.</param>
    /// <param name="apiKey">The value of the <c>x-goog-api-key</c> header.</param>
    /// <returns>An <see cref="HttpClient"/> that sends through this handler.</returns>
    /// <remarks>
    /// Disposing the returned client does not dispose this handler, and <see cref="Requests"/> stays readable.
    /// </remarks>
    public HttpClient CreateHttpClient(Uri? endpoint = null, string apiKey = "test-api-key")
    {
        var httpClient = new HttpClient(this, disposeHandler: false) { BaseAddress = endpoint ?? DefaultEndpoint };
        httpClient.DefaultRequestHeaders.Add("x-goog-api-key", apiKey);
        return httpClient;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // The body is read now because the sender disposes the request, and its content, once it has sent.
        var recorded = await RecordedRequest.CaptureAsync(request, cancellationToken).ConfigureAwait(false);

        Func<RecordedRequest, HttpResponseMessage>? createResponse;
        int requestNumber;
        lock (_gate)
        {
            _requests.Add(recorded);
            requestNumber = _requests.Count;
            _responses.TryDequeue(out createResponse);
        }

        if (createResponse is null)
        {
            throw new InvalidOperationException(
                $"No scripted response is left for request {requestNumber}: {recorded.Method} {recorded.RequestUri}");
        }

        var response = createResponse(recorded);
        response.RequestMessage = request;
        return response;
    }
}

using System.Text;
using System.Text.Json.Nodes;

namespace GeminiDotnet.Testing;

/// <summary>
/// Represents a request as <see cref="ScriptedHttpMessageHandler"/> received it.
/// </summary>
/// <remarks>
/// Every member stays readable after the sender has disposed the original <see cref="HttpRequestMessage"/>.
/// </remarks>
public sealed class RecordedRequest
{
    private readonly byte[]? _body;

    private RecordedRequest(
        HttpMethod method,
        Uri? requestUri,
        IReadOnlyDictionary<string, string> headers,
        string? contentType,
        byte[]? body)
    {
        Method = method;
        RequestUri = requestUri;
        Headers = headers;
        ContentType = contentType;
        _body = body;
    }

    /// <summary>
    /// Gets the HTTP method of the request.
    /// </summary>
    public HttpMethod Method { get; }

    /// <summary>
    /// Gets the absolute URI of the request, with the client's base address applied.
    /// </summary>
    public Uri? RequestUri { get; }

    /// <summary>
    /// Gets the request and content headers, keyed case-insensitively, with repeated values joined by <c>", "</c>.
    /// </summary>
    public IReadOnlyDictionary<string, string> Headers { get; }

    /// <summary>
    /// Gets the media type of the body without its parameters, or <see langword="null"/> when there is no body.
    /// </summary>
    /// <remarks>
    /// The full value, including parameters such as <c>charset</c>, is in <see cref="Headers"/>.
    /// </remarks>
    public string? ContentType { get; }

    /// <summary>
    /// Returns a copy of the body exactly as it was sent.
    /// </summary>
    /// <returns>The bytes of the body.</returns>
    /// <exception cref="InvalidOperationException">The request has no body.</exception>
    public byte[] ReadBodyAsBytes()
    {
        return RequireBody().ToArray();
    }

    /// <summary>
    /// Decodes the body as UTF-8.
    /// </summary>
    /// <returns>The text of the body.</returns>
    /// <exception cref="InvalidOperationException">The request has no body.</exception>
    public string ReadBodyAsString()
    {
        return Encoding.UTF8.GetString(RequireBody());
    }

    /// <summary>
    /// Parses the body as JSON.
    /// </summary>
    /// <returns>The root node of the body.</returns>
    /// <exception cref="InvalidOperationException">The request has no body.</exception>
    /// <exception cref="System.Text.Json.JsonException">The body is not valid JSON.</exception>
    public JsonNode? ReadBodyAsJson()
    {
        return JsonNode.Parse(RequireBody());
    }

    private byte[] RequireBody()
    {
        return _body ?? throw new InvalidOperationException($"The {Method} request to {RequestUri} has no body.");
    }

    internal static async Task<RecordedRequest> CaptureAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        foreach (var (name, values) in request.Headers)
        {
            headers[name] = string.Join(", ", values);
        }

        byte[]? body = null;
        if (request.Content is { } content)
        {
            foreach (var (name, values) in content.Headers)
            {
                headers[name] = string.Join(", ", values);
            }

            body = await content.ReadAsByteArrayAsync(cancellationToken).ConfigureAwait(false);
        }

        return new RecordedRequest(
            request.Method,
            request.RequestUri,
            headers,
            request.Content?.Headers.ContentType?.MediaType,
            body);
    }
}

using System.Text.Json;
using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.CachedContents;

/// <summary>
/// A transport that can stream HTTP requests and responses.
/// Next ID: 6
/// </summary>
public sealed record StreamableHttpTransport
{
    /// <summary>
    /// Optional: Fields for authentication headers, timeouts, etc., if needed.
    /// </summary>
    [JsonPropertyName("headers")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public JsonElement Headers { get; init; }

    /// <summary>
    /// Timeout for SSE read operations.
    /// </summary>
    [JsonPropertyName("sseReadTimeout")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? SseReadTimeout { get; init; }

    /// <summary>
    /// Whether to close the client session when the transport closes.
    /// </summary>
    [JsonPropertyName("terminateOnClose")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool? TerminateOnClose { get; init; }

    /// <summary>
    /// HTTP timeout for regular operations.
    /// </summary>
    [JsonPropertyName("timeout")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? Timeout { get; init; }

    /// <summary>
    /// The full URL for the MCPServer endpoint.
    /// Example: "https://api.example.com/mcp"
    /// </summary>
    [JsonPropertyName("url")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? Url { get; init; }
}


using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.CachedContents;

/// <summary>
/// A MCPServer is a server that can be called by the model to perform actions.
/// It is a server that implements the MCP protocol.
/// Next ID: 5
/// </summary>
public sealed record McpServer
{
    /// <summary>
    /// The name of the MCPServer.
    /// </summary>
    [JsonPropertyName("name")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? Name { get; init; }

    /// <summary>
    /// A transport that can stream HTTP requests and responses.
    /// </summary>
    [JsonPropertyName("streamableHttpTransport")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public StreamableHttpTransport? StreamableHttpTransport { get; init; }
}


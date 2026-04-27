using System.Text.Json;
using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta;

/// <summary>
/// A predicted server-side <see cref="V1Beta.ToolCall"/> returned from the model. This message
/// contains information about a tool that the model wants to invoke.
/// The client is NOT expected to execute this <see cref="V1Beta.ToolCall"/>. Instead, the
/// client should pass this <see cref="V1Beta.ToolCall"/> back to the API in a subsequent turn
/// within a <see cref="V1Beta.Content"/> message, along with the corresponding <see cref="V1Beta.ToolResponse"/>.
/// </summary>
public sealed record ToolCall
{
    /// <summary>
    /// Optional. The tool call arguments.
    /// Example: {"arg1" : "value1", "arg2" : "value2" , ...}
    /// </summary>
    [JsonPropertyName("args")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public JsonElement Arguments { get; init; }

    /// <summary>
    /// Optional. Unique identifier of the tool call.
    /// The server returns the tool response with the matching <see cref="Id"/>.
    /// </summary>
    [JsonPropertyName("id")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? Id { get; init; }

    /// <summary>
    /// Required. The type of tool that was called.
    /// </summary>
    [JsonPropertyName("toolType")]
    public required ToolType ToolType { get; init; }
}


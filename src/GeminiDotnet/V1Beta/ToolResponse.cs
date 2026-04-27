using System.Text.Json;
using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta;

/// <summary>
/// The output from a server-side <see cref="V1Beta.ToolCall"/> execution. This message contains
/// the results of a tool invocation that was initiated by a <see cref="V1Beta.ToolCall"/>
/// from the model. The client should pass this <see cref="V1Beta.ToolResponse"/> back to the API
/// in a subsequent turn within a <see cref="V1Beta.Content"/> message, along with the corresponding
/// <see cref="V1Beta.ToolCall"/>.
/// </summary>
public sealed record ToolResponse
{
    /// <summary>
    /// Optional. The identifier of the tool call this response is for.
    /// </summary>
    [JsonPropertyName("id")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? Id { get; init; }

    /// <summary>
    /// Optional. The tool response.
    /// </summary>
    [JsonPropertyName("response")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public JsonElement Response { get; init; }

    /// <summary>
    /// Required. The type of tool that was called, matching the <c>tool_type</c> in the
    /// corresponding <see cref="V1Beta.ToolCall"/>.
    /// </summary>
    [JsonPropertyName("toolType")]
    public required ToolType ToolType { get; init; }
}

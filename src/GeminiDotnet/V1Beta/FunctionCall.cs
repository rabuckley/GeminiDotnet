using System.Text.Json;
using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta;

/// <summary>
/// A predicted <see cref="V1Beta.FunctionCall"/> returned from the model that contains
/// a string representing the <c>FunctionDeclaration.name</c> with the
/// arguments and their values.
/// </summary>
public sealed record FunctionCall
{
    /// <summary>
    /// Optional. The function parameters and values in JSON object format.
    /// </summary>
    [JsonPropertyName("args")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public JsonElement Arguments { get; init; }

    /// <summary>
    /// Optional. Unique identifier of the function call. If populated, the client to
    /// execute the <c>function_call</c> and return the response with the matching <see cref="Id"/>.
    /// </summary>
    [JsonPropertyName("id")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? Id { get; init; }

    /// <summary>
    /// Required. The name of the function to call.
    /// Must be a-z, A-Z, 0-9, or contain underscores and dashes, with a maximum
    /// length of 64.
    /// </summary>
    [JsonPropertyName("name")]
    public required string Name { get; init; }
}


using System.Text.Json;
using System.Text.Json.Serialization;

namespace GeminiDotnet.V1;

/// <summary>
/// The result output from a <see cref="V1.FunctionCall"/> that contains a string
/// representing the <c>FunctionDeclaration.name</c> and a structured JSON
/// object containing any output from the function is used as context to
/// the model. This should contain the result of a<see cref="V1.FunctionCall"/> made
/// based on model prediction.
/// </summary>
public sealed record FunctionResponse
{
    /// <summary>
    /// Optional. The identifier of the function call this response is for. Populated by the
    /// client to match the corresponding function call <see cref="Id"/>.
    /// </summary>
    [JsonPropertyName("id")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? Id { get; init; }

    /// <summary>
    /// Required. The name of the function to call.
    /// Must be a-z, A-Z, 0-9, or contain underscores and dashes, with a maximum
    /// length of 128.
    /// </summary>
    [JsonPropertyName("name")]
    public required string Name { get; init; }

    /// <summary>
    /// Required. The function response in JSON object format.
    /// Callers can use any keys of their choice that fit the function's syntax
    /// to return the function output, e.g. "output", "result", etc.
    /// In particular, if the function call failed to execute, the response can
    /// have an "error" key to return error details to the model.
    /// Multimedia can be included by using a subobject containing a single "$ref"
    /// key whose value is the <c>inline_data.display_name</c> of a
    /// <c>FunctionResponsePart</c> holding the multimedia.
    /// See https://ai.google.dev/gemini-api/docs/function-calling#multimodal.
    /// </summary>
    [JsonPropertyName("response")]
    public required JsonElement Response { get; init; }
}


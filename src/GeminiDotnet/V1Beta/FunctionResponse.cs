using System.Text.Json;
using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta;

/// <summary>
/// The result output from a <see cref="V1Beta.FunctionCall"/> that contains a string
/// representing the <c>FunctionDeclaration.name</c> and a structured JSON
/// object containing any output from the function is used as context to
/// the model. This should contain the result of a<see cref="V1Beta.FunctionCall"/> made
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
    /// length of 64.
    /// </summary>
    [JsonPropertyName("name")]
    public required string Name { get; init; }

    /// <summary>
    /// Optional. Ordered <c>Parts</c> that constitute a function response. Parts may have
    /// different IANA MIME types.
    /// </summary>
    [JsonPropertyName("parts")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IReadOnlyList<FunctionResponsePart>? Parts { get; init; }

    /// <summary>
    /// Required. The function response in JSON object format.
    /// Callers can use any keys of their choice that fit the function's syntax
    /// to return the function output, e.g. "output", "result", etc.
    /// In particular, if the function call failed to execute, the response can
    /// have an "error" key to return error details to the model.
    /// </summary>
    [JsonPropertyName("response")]
    public required JsonElement Response { get; init; }

    /// <summary>
    /// Optional. Specifies how the response should be scheduled in the conversation.
    /// Only applicable to NON_BLOCKING function calls, is ignored otherwise.
    /// Defaults to WHEN_IDLE.
    /// </summary>
    [JsonPropertyName("scheduling")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public FunctionResponseScheduling? Scheduling { get; init; }

    /// <summary>
    /// Optional. Signals that function call continues, and more responses will be
    /// returned, turning the function call into a generator.
    /// Is only applicable to NON_BLOCKING function calls, is ignored otherwise.
    /// If set to false, future responses will not be considered.
    /// It is allowed to return empty <see cref="Response"/> with <c>will_continue=False</c> to
    /// signal that the function call is finished. This may still trigger the model
    /// generation. To avoid triggering the generation and finish the function
    /// call, additionally set <see cref="Scheduling"/> to <c>SILENT</c>.
    /// </summary>
    [JsonPropertyName("willContinue")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool? WillContinue { get; init; }
}


using System.Text.Json;
using System.Text.Json.Serialization;

namespace GeminiDotnet.ContentGeneration.FunctionCalling;

/// <summary>
/// The result output from a <see cref="FunctionCall"/> that contains a string representing the <see cref="FunctionDeclaration.Name"/> name and a structured JSON object containing any output from the function is used as context to the model. This should contain the result of a <see cref="FunctionCall"/> made based on model prediction.
/// </summary>
public sealed record FunctionResponse
{
    /// <summary>
    /// The id of the function call this response is for. Populated by the client to match the corresponding function call id.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// The name of the function to call. Must be a-z, A-Z, 0-9, or contain underscores and dashes, with a maximum
    /// length of 63.
    /// </summary>
    [JsonPropertyName("name")]
    public required string Name { get; init; }

    /// <summary>
    /// The function response in JSON object format.
    /// </summary>
    [JsonPropertyName("response")]
    public required IDictionary<string, JsonElement> Response { get; init; }

    /// <summary>
    /// Optional. Signals that function call continues, and more responses will be returned, turning the function call
    /// into a generator. Is only applicable to NON_BLOCKING function calls, is ignored otherwise. If set to
    /// <see langword="false"/>, future responses will not be considered. It is allowed to return empty response
    /// with <see cref="WillContinue"/> = <see langword="false"/> to signal that the function call is finished. This may
    /// still trigger the model generation. To avoid triggering the generation and finish the function call,
    /// additionally set scheduling to SILENT.
    /// </summary>
    [JsonPropertyName("willContinue")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? WillContinue { get; init; }

    /// <summary>
    /// Optional. Specifies how the response should be scheduled in the conversation. Only applicable to
    /// <see cref="FunctionBehavior.NonBlocking"/> function calls, is ignored otherwise. Defaults to <see cref="FunctionResponseScheduling.WhenIdle"/>
    /// </summary>
    [JsonPropertyName("scheduling")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public FunctionResponseScheduling? Scheduling { get; init; }
}

using System.Text.Json;
using System.Text.Json.Serialization;

namespace GeminiDotnet.ContentGeneration.FunctionCalling;

/// <summary>
/// The result output from a <see cref="FunctionCall"/> that contains a string representing the
/// <see cref="FunctionDeclaration.Name"/> name and a structured JSON object containing any output from the function is
/// used as context to the model. This should contain the result of a <see cref="FunctionCall"/> made based on model
/// prediction.
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
    public required JsonElement Response { get; init; }
}

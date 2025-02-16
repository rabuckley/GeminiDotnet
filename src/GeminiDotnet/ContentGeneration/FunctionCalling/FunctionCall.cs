using System.Text.Json.Serialization;

namespace GeminiDotnet.ContentGeneration.FunctionCalling;

/// <summary>
/// A predicted <see cref="FunctionCall"/>
/// FunctionCall returned from the model that contains a string representing the FunctionDeclaration.name
/// with the arguments and their values.
/// </summary>
public sealed record FunctionCall
{
    /// <summary>
    /// The unique id of the function call. If populated, the client to execute the functionCall and return the
    /// response with the matching id.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// The name of the function to call.
    /// <remarks>
    /// Must be a-z, A-Z, 0-9, or contain underscores and dashes, with a maximum length of 63.
    /// </remarks>
    /// </summary>
    [JsonPropertyName("name")]
    public required string Name { get; init; }

    /// <summary>
    /// The function parameters and values in JSON object format.
    /// </summary>
    [JsonPropertyName("args")]
    public IDictionary<string, object?>? Arguments { get; init; }
}

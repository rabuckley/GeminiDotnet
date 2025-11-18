using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.Models;

/// <summary>
/// An input/output example used to instruct the Model.
/// It demonstrates how the model should respond or format its response.
/// </summary>
public sealed record Example
{
    /// <summary>
    /// Required. An example of an input <see cref="V1Beta.Models.Message"/> from the user.
    /// </summary>
    [JsonPropertyName("input")]
    public required Message Input { get; init; }

    /// <summary>
    /// Required. An example of what the model should output given the input.
    /// </summary>
    [JsonPropertyName("output")]
    public required Message Output { get; init; }
}


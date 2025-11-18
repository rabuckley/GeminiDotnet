using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.Models;

/// <summary>
/// The response from the model.
/// This includes candidate messages and
/// conversation history in the form of chronologically-ordered messages.
/// </summary>
public sealed record GenerateMessageResponse
{
    /// <summary>
    /// Candidate response messages from the model.
    /// </summary>
    [JsonPropertyName("candidates")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IReadOnlyList<Message>? Candidates { get; init; }

    /// <summary>
    /// A set of content filtering metadata for the prompt and response
    /// text.
    /// This indicates which <c>SafetyCategory</c>(s) blocked a
    /// candidate from this response, the lowest <c>HarmProbability</c>
    /// that triggered a block, and the HarmThreshold setting for that category.
    /// </summary>
    [JsonPropertyName("filters")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IReadOnlyList<ContentFilter>? Filters { get; init; }

    /// <summary>
    /// The conversation history used by the model.
    /// </summary>
    [JsonPropertyName("messages")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IReadOnlyList<Message>? Messages { get; init; }
}


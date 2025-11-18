using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.Models;

/// <summary>
/// All of the structured input text passed to the model as a prompt.
/// A <see cref="V1Beta.Models.MessagePrompt"/> contains a structured set of fields that provide context
/// for the conversation, examples of user input/model output message pairs that
/// prime the model to respond in different ways, and the conversation history
/// or list of messages representing the alternating turns of the conversation
/// between the user and the model.
/// </summary>
public sealed record MessagePrompt
{
    /// <summary>
    /// Optional. Text that should be provided to the model first to ground the response.
    /// If not empty, this <see cref="Context"/> will be given to the model first before the
    /// <see cref="Examples"/> and <see cref="Messages"/>. When using a <see cref="Context"/> be sure to provide it
    /// with every request to maintain continuity.
    /// This field can be a description of your prompt to the model to help provide
    /// context and guide the responses. Examples: "Translate the phrase from
    /// English to French." or "Given a statement, classify the sentiment as happy,
    /// sad or neutral."
    /// Anything included in this field will take precedence over message history
    /// if the total input size exceeds the model's <c>input_token_limit</c> and the
    /// input request is truncated.
    /// </summary>
    [JsonPropertyName("context")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? Context { get; init; }

    /// <summary>
    /// Optional. Examples of what the model should generate.
    /// This includes both user input and the response that the model should
    /// emulate.
    /// These <see cref="Examples"/> are treated identically to conversation messages except
    /// that they take precedence over the history in <see cref="Messages"/>:
    /// If the total input size exceeds the model's <c>input_token_limit</c> the input
    /// will be truncated. Items will be dropped from <see cref="Messages"/> before <see cref="Examples"/>.
    /// </summary>
    [JsonPropertyName("examples")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IReadOnlyList<Example>? Examples { get; init; }

    /// <summary>
    /// Required. A snapshot of the recent conversation history sorted chronologically.
    /// Turns alternate between two authors.
    /// If the total input size exceeds the model's <c>input_token_limit</c> the input
    /// will be truncated: The oldest items will be dropped from <see cref="Messages"/>.
    /// </summary>
    [JsonPropertyName("messages")]
    public required IReadOnlyList<Message> Messages { get; init; }
}


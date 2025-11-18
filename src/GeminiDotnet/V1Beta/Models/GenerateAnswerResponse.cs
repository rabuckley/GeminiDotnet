using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.Models;

/// <summary>
/// Response from the model for a grounded answer.
/// </summary>
public sealed record GenerateAnswerResponse
{
    /// <summary>
    /// Candidate answer from the model.
    /// Note: The model *always* attempts to provide a grounded answer, even when
    /// the answer is unlikely to be answerable from the given passages.
    /// In that case, a low-quality or ungrounded answer may be provided, along
    /// with a low <c>answerable_probability</c>.
    /// </summary>
    [JsonPropertyName("answer")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Candidate? Answer { get; init; }

    /// <summary>
    /// Output only. The model's estimate of the probability that its answer is correct and
    /// grounded in the input passages.
    /// A low <c>answerable_probability</c> indicates that the answer might not be
    /// grounded in the sources.
    /// When <c>answerable_probability</c> is low, you may want to:
    /// * Display a message to the effect of "We couldn’t answer that question" to
    /// the user.
    /// * Fall back to a general-purpose LLM that answers the question from world
    /// knowledge. The threshold and nature of such fallbacks will depend on
    /// individual use cases. <c>0.5</c> is a good starting threshold.
    /// </summary>
    [JsonPropertyName("answerableProbability")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public float? AnswerableProbability { get; init; }

    /// <summary>
    /// Output only. Feedback related to the input data used to answer the question, as opposed
    /// to the model-generated response to the question.
    /// The input data can be one or more of the following:
    /// - Question specified by the last entry in <c>GenerateAnswerRequest.content</c>
    /// - Conversation history specified by the other entries in
    /// <c>GenerateAnswerRequest.content</c>
    /// - Grounding sources (<c>GenerateAnswerRequest.semantic_retriever</c> or
    /// <c>GenerateAnswerRequest.inline_passages</c>)
    /// </summary>
    [JsonPropertyName("inputFeedback")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public InputFeedback? InputFeedback { get; init; }
}


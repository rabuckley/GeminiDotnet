using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.Models;

/// <summary>
/// Request to generate a grounded answer from the <see cref="V1Beta.Models.Model"/>.
/// </summary>
public sealed record GenerateAnswerRequest
{
    /// <summary>
    /// Required. Style in which answers should be returned.
    /// </summary>
    [JsonPropertyName("answerStyle")]
    public required GenerateAnswerRequestAnswerStyle AnswerStyle { get; init; }

    /// <summary>
    /// Required. The content of the current conversation with the <see cref="V1Beta.Models.Model"/>. For single-turn
    /// queries, this is a single question to answer. For multi-turn queries, this
    /// is a repeated field that contains conversation history and the last
    /// <see cref="V1Beta.Content"/> in the list containing the question.
    /// Note: <c>GenerateAnswer</c> only supports queries in English.
    /// </summary>
    [JsonPropertyName("contents")]
    public required IReadOnlyList<Content> Contents { get; init; }

    /// <summary>
    /// Passages provided inline with the request.
    /// </summary>
    [JsonPropertyName("inlinePassages")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public GroundingPassages? InlinePassages { get; init; }

    /// <summary>
    /// Optional. A list of unique <see cref="V1Beta.Models.SafetySetting"/> instances for blocking unsafe content.
    /// This will be enforced on the <see cref="Contents"/> and
    /// <c>GenerateAnswerResponse.candidate</c>. There should not be more than one
    /// setting for each <c>SafetyCategory</c> type. The API will block any contents and
    /// responses that fail to meet the thresholds set by these settings. This list
    /// overrides the default settings for each <c>SafetyCategory</c> specified in the
    /// safety_settings. If there is no <see cref="V1Beta.Models.SafetySetting"/> for a given
    /// <c>SafetyCategory</c> provided in the list, the API will use the default safety
    /// setting for that category. Harm categories HARM_CATEGORY_HATE_SPEECH,
    /// HARM_CATEGORY_SEXUALLY_EXPLICIT, HARM_CATEGORY_DANGEROUS_CONTENT,
    /// HARM_CATEGORY_HARASSMENT are supported.
    /// Refer to the
    /// [guide](https://ai.google.dev/gemini-api/docs/safety-settings)
    /// for detailed information on available safety settings. Also refer to the
    /// [Safety guidance](https://ai.google.dev/gemini-api/docs/safety-guidance) to
    /// learn how to incorporate safety considerations in your AI applications.
    /// </summary>
    [JsonPropertyName("safetySettings")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IReadOnlyList<SafetySetting>? SafetySettings { get; init; }

    /// <summary>
    /// Content retrieved from resources created via the Semantic Retriever
    /// API.
    /// </summary>
    [JsonPropertyName("semanticRetriever")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public SemanticRetrieverConfiguration? SemanticRetriever { get; init; }

    /// <summary>
    /// Optional. Controls the randomness of the output.
    /// Values can range from [0.0,1.0], inclusive. A value closer to 1.0 will
    /// produce responses that are more varied and creative, while a value closer
    /// to 0.0 will typically result in more straightforward responses from the
    /// model. A low temperature (~0.2) is usually recommended for
    /// Attributed-Question-Answering use cases.
    /// </summary>
    [JsonPropertyName("temperature")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public float? Temperature { get; init; }
}


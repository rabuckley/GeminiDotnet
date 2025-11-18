using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.Models;

/// <summary>
/// Counts the number of tokens in the <c>prompt</c> sent to a model.
/// Models may tokenize text differently, so each model may return a different
/// <c>token_count</c>.
/// </summary>
public sealed record CountTokensRequest
{
    /// <summary>
    /// Optional. The input given to the model as a prompt. This field is ignored when
    /// <c>generate_content_request</c> is set.
    /// </summary>
    [JsonPropertyName("contents")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IReadOnlyList<Content>? Contents { get; init; }

    /// <summary>
    /// Optional. The overall input given to the <see cref="V1Beta.Models.Model"/>. This includes the prompt as well as
    /// other model steering information like [system
    /// instructions](https://ai.google.dev/gemini-api/docs/system-instructions),
    /// and/or function declarations for [function
    /// calling](https://ai.google.dev/gemini-api/docs/function-calling).
    /// <see cref="V1Beta.Models.Model"/>s/<see cref="V1Beta.Content"/>s and <c>generate_content_request</c>s are mutually
    /// exclusive. You can either send <see cref="V1Beta.Models.Model"/> + <see cref="V1Beta.Content"/>s or a
    /// <c>generate_content_request</c>, but never both.
    /// </summary>
    [JsonPropertyName("generateContentRequest")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public GenerateContentRequest? GenerateContentRequest { get; init; }
}


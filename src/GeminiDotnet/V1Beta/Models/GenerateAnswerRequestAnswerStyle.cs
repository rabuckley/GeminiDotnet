using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.Models;

/// <summary>
/// Required. Style in which answers should be returned.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter<GenerateAnswerRequestAnswerStyle>))]
public enum GenerateAnswerRequestAnswerStyle
{
    /// <summary>
    /// Unspecified answer style.
    /// </summary>
    [JsonStringEnumMemberName("ANSWER_STYLE_UNSPECIFIED")]
    Unspecified,

    /// <summary>
    /// Succinct but abstract style.
    /// </summary>
    [JsonStringEnumMemberName("ABSTRACTIVE")]
    Abstractive,

    /// <summary>
    /// Very brief and extractive style.
    /// </summary>
    [JsonStringEnumMemberName("EXTRACTIVE")]
    Extractive,

    /// <summary>
    /// Verbose style including extra details. The response may be formatted as a
    /// sentence, paragraph, multiple paragraphs, or bullet points, etc.
    /// </summary>
    [JsonStringEnumMemberName("VERBOSE")]
    Verbose,
}


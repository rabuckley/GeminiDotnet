using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.Models;

/// <summary>
/// The base unit of structured text.
/// A <see cref="V1Beta.Models.Message"/> includes an <see cref="Author"/> and the <see cref="Content"/> of
/// the <see cref="V1Beta.Models.Message"/>.
/// The <see cref="Author"/> is used to tag messages when they are fed to the
/// model as text.
/// </summary>
public sealed record Message
{
    /// <summary>
    /// Optional. The author of this Message.
    /// This serves as a key for tagging
    /// the content of this Message when it is fed to the model as text.
    /// The author can be any alphanumeric string.
    /// </summary>
    [JsonPropertyName("author")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? Author { get; init; }

    /// <summary>
    /// Output only. Citation information for model-generated <see cref="Content"/> in this <see cref="V1Beta.Models.Message"/>.
    /// If this <see cref="V1Beta.Models.Message"/> was generated as output from the model, this field may be
    /// populated with attribution information for any text included in the
    /// <see cref="Content"/>. This field is used only on output.
    /// </summary>
    [JsonPropertyName("citationMetadata")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public CitationMetadata? CitationMetadata { get; init; }

    /// <summary>
    /// Required. The text content of the structured <see cref="V1Beta.Models.Message"/>.
    /// </summary>
    [JsonPropertyName("content")]
    public required string Content { get; init; }
}


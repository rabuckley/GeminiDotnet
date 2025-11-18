using System.Text.Json.Serialization;

namespace GeminiDotnet.V1.Models;

/// <summary>
/// The base structured datatype containing multi-part content of a message.
/// A <see cref="V1.Models.Content"/> includes a <see cref="Role"/> field designating the producer of the <see cref="V1.Models.Content"/>
/// and a <see cref="Parts"/> field containing multi-part data that contains the content of
/// the message turn.
/// </summary>
public sealed record Content
{
    /// <summary>
    /// Ordered <c>Parts</c> that constitute a single message. Parts may have different
    /// MIME types.
    /// </summary>
    [JsonPropertyName("parts")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IReadOnlyList<Part>? Parts { get; init; }

    /// <summary>
    /// Optional. The producer of the content. Must be either 'user' or 'model'.
    /// Useful to set for multi-turn conversations, otherwise can be left blank
    /// or unset.
    /// </summary>
    [JsonPropertyName("role")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? Role { get; init; }
}


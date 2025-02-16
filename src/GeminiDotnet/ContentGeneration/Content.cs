using System.Text.Json.Serialization;

namespace GeminiDotnet.ContentGeneration;

/// <summary>
/// The base structured datatype containing multi-part content of a message.
/// A Content includes a <see cref="Role"/> field designating the producer of the Content and a <see cref="Parts"/> field containing multi-part data that contains the content of the message turn.
/// </summary>
public sealed record Content
{
    /// <summary>
    /// Ordered <see cref="Part"/>s that constitute a single message. Parts may have different MIME types.
    /// </summary>
    [JsonPropertyName("parts")]
    public required IReadOnlyCollection<Part> Parts { get; init; }

    /// <summary>
    ///  The producer of the content. Must be either 'user' or 'model'.
    /// Useful to set for multi-turn conversations, otherwise can be left blank or unset.
    /// </summary>
    [JsonPropertyName("role")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Role { get; init; }
}

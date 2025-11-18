using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.Models;

/// <summary>
/// A proto encapsulate various type of media.
/// </summary>
public sealed record Media
{
    /// <summary>
    /// Video as the only one for now.  This is mimicking Vertex proto.
    /// </summary>
    [JsonPropertyName("video")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Video? Video { get; init; }
}


using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta;

/// <summary>
/// User provided string values assigned to a single metadata key.
/// </summary>
[global::System.Diagnostics.CodeAnalysis.Experimental("GEMINI001", UrlFormat = "https://github.com/rabuckley/GeminiDotnet#v1beta-stability")]
public sealed record StringList
{
    /// <summary>
    /// The string values of the metadata to store.
    /// </summary>
    [JsonPropertyName("values")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IReadOnlyList<string>? Values { get; init; }
}


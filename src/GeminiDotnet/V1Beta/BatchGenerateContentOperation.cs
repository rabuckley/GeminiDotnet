using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta;

/// <summary>
/// This resource represents a long-running operation where metadata and response fields are strongly typed.
/// </summary>
[global::System.Diagnostics.CodeAnalysis.Experimental("GEMINI001", UrlFormat = "https://github.com/rabuckley/GeminiDotnet#v1beta-stability")]
public sealed record BatchGenerateContentOperation : BaseOperation
{
    /// <summary>
    /// A resource representing a batch of <c>GenerateContent</c> requests.
    /// </summary>
    [JsonPropertyName("metadata")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public GenerateContentBatch? Metadata { get; init; }

    /// <summary>
    /// Response for a <c>BatchGenerateContent</c> operation.
    /// </summary>
    [JsonPropertyName("response")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public BatchGenerateContentResponse? Response { get; init; }
}


using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta;

/// <summary>
/// Metadata describes the input audio content.
/// Set this in <c>Part.audio_metadata</c> or <c>FunctionResponsePart.audio_metadata</c>
/// when <c>inline_data</c> or <c>file_data</c> contains audio to configure static audio
/// preprocessing.
/// </summary>
[global::System.Diagnostics.CodeAnalysis.Experimental("GEMINI001", UrlFormat = "https://github.com/rabuckley/GeminiDotnet#v1beta-stability")]
public sealed record AudioMetadata
{
    /// <summary>
    /// Optional. The end offset of the audio, relative to the start of the
    /// audio stream.
    /// </summary>
    [JsonPropertyName("endOffset")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? EndOffset { get; init; }

    /// <summary>
    /// Optional. The start offset of the audio, relative to the start of the
    /// audio stream.
    /// </summary>
    [JsonPropertyName("startOffset")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? StartOffset { get; init; }
}


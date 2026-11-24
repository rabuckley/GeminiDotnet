using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta;

/// <summary>
/// A datatype containing media that is part of a <see cref="V1Beta.FunctionResponse"/> message.
/// A <see cref="V1Beta.FunctionResponsePart"/> consists of data which has an associated datatype. A
/// <see cref="V1Beta.FunctionResponsePart"/> can only contain one of the accepted types in
/// <c>FunctionResponsePart.data</c>.
/// Media can be passed as raw bytes (<c>inline_data</c>) or by URI (<c>file_data</c>) with
/// an IANA MIME type identifying the type and subtype of the media.
/// </summary>
[global::System.Diagnostics.CodeAnalysis.Experimental("GEMINI001", UrlFormat = "https://github.com/rabuckley/GeminiDotnet#v1beta-stability")]
public sealed record FunctionResponsePart
{
    /// <summary>
    /// Optional. Audio metadata. The metadata should only be specified while the audio
    /// data is presented in inline_data or file_data.
    /// </summary>
    [JsonPropertyName("audioMetadata")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public AudioMetadata? AudioMetadata { get; init; }

    /// <summary>
    /// Optional. Document metadata. The metadata should only be specified while the
    /// document data is presented in inline_data or file_data.
    /// </summary>
    [JsonPropertyName("documentMetadata")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DocumentMetadata? DocumentMetadata { get; init; }

    /// <summary>
    /// URI based data.
    /// </summary>
    [JsonPropertyName("fileData")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public FunctionResponseFileData? FileData { get; init; }

    /// <summary>
    /// Inline media bytes.
    /// </summary>
    [JsonPropertyName("inlineData")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public FunctionResponseBlob? InlineData { get; init; }

    /// <summary>
    /// Optional. How the model processes this part's media for understanding.
    /// Only meaningful for video parts (<c>inline_data</c> or <c>file_data</c> with video
    /// mime). Non-video parts ignore this field.
    /// </summary>
    [JsonPropertyName("mediaProcessing")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public FunctionResponsePartMediaProcessing? MediaProcessing { get; init; }

    /// <summary>
    /// Optional. Media resolution for the input media.
    /// </summary>
    [JsonPropertyName("mediaResolution")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public MediaResolution? MediaResolution { get; init; }

    /// <summary>
    /// Optional. Spreadsheet metadata. The metadata should only be specified while the
    /// spreadsheet data is presented in inline_data or file_data.
    /// </summary>
    [JsonPropertyName("spreadsheetMetadata")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public SpreadsheetMetadata? SpreadsheetMetadata { get; init; }

    /// <summary>
    /// Optional. Video metadata. The metadata should only be specified while the video
    /// data is presented in inline_data or file_data.
    /// </summary>
    [JsonPropertyName("videoMetadata")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public VideoMetadata? VideoMetadata { get; init; }
}


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
public sealed record FunctionResponsePart
{
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
}


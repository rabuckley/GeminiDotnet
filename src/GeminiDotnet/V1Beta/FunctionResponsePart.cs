using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta;

/// <summary>
/// A datatype containing media that is part of a <see cref="V1Beta.FunctionResponse"/> message.
/// A <see cref="V1Beta.FunctionResponsePart"/> consists of data which has an associated datatype. A
/// <see cref="V1Beta.FunctionResponsePart"/> can only contain one of the accepted types in
/// <c>FunctionResponsePart.data</c>.
/// A <see cref="V1Beta.FunctionResponsePart"/> must have a fixed IANA MIME type identifying the
/// type and subtype of the media if the <c>inline_data</c> field is filled with raw
/// bytes.
/// </summary>
public sealed record FunctionResponsePart
{
    /// <summary>
    /// Inline media bytes.
    /// </summary>
    [JsonPropertyName("inlineData")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public FunctionResponseBlob? InlineData { get; init; }
}


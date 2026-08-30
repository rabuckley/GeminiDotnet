using System.Text.Json.Serialization;

namespace GeminiDotnet.V1;

/// <summary>
/// Optional. How the model processes this part's media for understanding.
/// Only meaningful for video parts (<c>inline_data</c> or <c>file_data</c> with video
/// mime). Non-video parts ignore this field.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter<PartMediaProcessing>))]
public enum PartMediaProcessing
{
    /// <summary>
    /// Defaults to model-specific processing.
    /// </summary>
    [JsonStringEnumMemberName("MEDIA_PROCESSING_UNSPECIFIED")]
    Unspecified,

    /// <summary>
    /// Fixed-rate frame extraction. All frames placed in context.
    /// </summary>
    [JsonStringEnumMemberName("STATIC")]
    Static,

    /// <summary>
    /// Model-driven dynamic navigation. Recommended for most use cases.
    /// </summary>
    [JsonStringEnumMemberName("AGENTIC")]
    Agentic,
}


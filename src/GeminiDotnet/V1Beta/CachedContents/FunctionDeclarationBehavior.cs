using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.CachedContents;

/// <summary>
/// Optional. Specifies the function Behavior.
/// Currently only supported by the BidiGenerateContent method.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter<FunctionDeclarationBehavior>))]
public enum FunctionDeclarationBehavior
{
    /// <summary>
    /// This value is unused.
    /// </summary>
    [JsonStringEnumMemberName("UNSPECIFIED")]
    Unspecified,

    /// <summary>
    /// If set, the system will wait to receive the function response before
    /// continuing the conversation.
    /// </summary>
    [JsonStringEnumMemberName("BLOCKING")]
    Blocking,

    /// <summary>
    /// If set, the system will not wait to receive the function response.
    /// Instead, it will attempt to handle function responses as they become
    /// available while maintaining the conversation between the user and the
    /// model.
    /// </summary>
    [JsonStringEnumMemberName("NON_BLOCKING")]
    NonBlocking,
}


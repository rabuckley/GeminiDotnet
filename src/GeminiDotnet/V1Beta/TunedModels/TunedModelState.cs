using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.TunedModels;

/// <summary>
/// Output only. The state of the tuned model.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter<TunedModelState>))]
public enum TunedModelState
{
    /// <summary>
    /// The default value. This value is unused.
    /// </summary>
    [JsonStringEnumMemberName("STATE_UNSPECIFIED")]
    Unspecified,

    /// <summary>
    /// The model is being created.
    /// </summary>
    [JsonStringEnumMemberName("CREATING")]
    Creating,

    /// <summary>
    /// The model is ready to be used.
    /// </summary>
    [JsonStringEnumMemberName("ACTIVE")]
    Active,

    /// <summary>
    /// The model failed to be created.
    /// </summary>
    [JsonStringEnumMemberName("FAILED")]
    Failed,
}


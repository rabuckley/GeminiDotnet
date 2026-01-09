using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta;

[JsonConverter(typeof(JsonStringEnumConverter<ModelStage>))]
public enum ModelStage
{
    /// <summary>
    /// Unspecified model stage.
    /// </summary>
    [JsonStringEnumMemberName("MODEL_STAGE_UNSPECIFIED")]
    Unspecified,

    /// <summary>
    /// The underlying model is subject to lots of tunings.
    /// </summary>
    [JsonStringEnumMemberName("UNSTABLE_EXPERIMENTAL")]
    UnstableExperimental,

    /// <summary>
    /// Models in this stage are for experimental purposes only.
    /// </summary>
    [JsonStringEnumMemberName("EXPERIMENTAL")]
    Experimental,

    /// <summary>
    /// Models in this stage are more mature than experimental models.
    /// </summary>
    [JsonStringEnumMemberName("PREVIEW")]
    Preview,

    /// <summary>
    /// Models in this stage are considered stable and ready for production use.
    /// </summary>
    [JsonStringEnumMemberName("STABLE")]
    Stable,

    /// <summary>
    /// If the model is on this stage, it means that this model is on the path to
    /// deprecation in near future. Only existing customers can use this model.
    /// </summary>
    [JsonStringEnumMemberName("LEGACY")]
    Legacy,

    /// <summary>
    /// Models in this stage are deprecated. These models cannot be used.
    /// </summary>
    [JsonStringEnumMemberName("DEPRECATED")]
    Deprecated,

    /// <summary>
    /// Models in this stage are retired. These models cannot be used.
    /// </summary>
    [JsonStringEnumMemberName("RETIRED")]
    Retired,
}


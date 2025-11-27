using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.Models;

/// <summary>
/// Required. Operator applied to the given key-value pair to trigger the condition.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter<ConditionOperation>))]
public enum ConditionOperation
{
    /// <summary>
    /// The default value. This value is unused.
    /// </summary>
    [JsonStringEnumMemberName("OPERATOR_UNSPECIFIED")]
    OperatorUnspecified,

    /// <summary>
    /// Supported by numeric.
    /// </summary>
    [JsonStringEnumMemberName("LESS")]
    Less,

    /// <summary>
    /// Supported by numeric.
    /// </summary>
    [JsonStringEnumMemberName("LESS_EQUAL")]
    LessEqual,

    /// <summary>
    /// Supported by numeric & string.
    /// </summary>
    [JsonStringEnumMemberName("EQUAL")]
    Equal,

    /// <summary>
    /// Supported by numeric.
    /// </summary>
    [JsonStringEnumMemberName("GREATER_EQUAL")]
    GreaterEqual,

    /// <summary>
    /// Supported by numeric.
    /// </summary>
    [JsonStringEnumMemberName("GREATER")]
    Greater,

    /// <summary>
    /// Supported by numeric & string.
    /// </summary>
    [JsonStringEnumMemberName("NOT_EQUAL")]
    NotEqual,

    /// <summary>
    /// Supported by string only when `CustomMetadata` value type for the given
    /// key has a `string_list_value`.
    /// </summary>
    [JsonStringEnumMemberName("INCLUDES")]
    Includes,

    /// <summary>
    /// Supported by string only when `CustomMetadata` value type for the given
    /// key has a `string_list_value`.
    /// </summary>
    [JsonStringEnumMemberName("EXCLUDES")]
    Excludes,
}


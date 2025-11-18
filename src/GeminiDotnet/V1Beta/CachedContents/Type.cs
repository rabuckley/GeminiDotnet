using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.CachedContents;

[JsonConverter(typeof(JsonStringEnumConverter<Type>))]
public enum Type
{
    /// <summary>
    /// Not specified, should not be used.
    /// </summary>
    [JsonStringEnumMemberName("TYPE_UNSPECIFIED")]
    Unspecified,

    /// <summary>
    /// String type.
    /// </summary>
    [JsonStringEnumMemberName("STRING")]
    String,

    /// <summary>
    /// Number type.
    /// </summary>
    [JsonStringEnumMemberName("NUMBER")]
    Number,

    /// <summary>
    /// Integer type.
    /// </summary>
    [JsonStringEnumMemberName("INTEGER")]
    Integer,

    /// <summary>
    /// Boolean type.
    /// </summary>
    [JsonStringEnumMemberName("BOOLEAN")]
    Boolean,

    /// <summary>
    /// Array type.
    /// </summary>
    [JsonStringEnumMemberName("ARRAY")]
    Array,

    /// <summary>
    /// Object type.
    /// </summary>
    [JsonStringEnumMemberName("OBJECT")]
    Object,

    /// <summary>
    /// Null type.
    /// </summary>
    [JsonStringEnumMemberName("NULL")]
    Null,
}


using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta;

/// <summary>
/// Optional. Immutable. The type of the grantee.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter<PermissionGranteeType>))]
public enum PermissionGranteeType
{
    /// <summary>
    /// The default value. This value is unused.
    /// </summary>
    [JsonStringEnumMemberName("GRANTEE_TYPE_UNSPECIFIED")]
    Unspecified,

    /// <summary>
    /// Represents a user. When set, you must provide email_address for the user.
    /// </summary>
    [JsonStringEnumMemberName("USER")]
    User,

    /// <summary>
    /// Represents a group. When set, you must provide email_address for the
    /// group.
    /// </summary>
    [JsonStringEnumMemberName("GROUP")]
    Group,

    /// <summary>
    /// Represents access to everyone. No extra information is required.
    /// </summary>
    [JsonStringEnumMemberName("EVERYONE")]
    Everyone,
}


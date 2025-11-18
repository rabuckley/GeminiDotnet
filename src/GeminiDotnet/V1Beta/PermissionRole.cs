using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta;

/// <summary>
/// Required. The role granted by this permission.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter<PermissionRole>))]
public enum PermissionRole
{
    /// <summary>
    /// The default value. This value is unused.
    /// </summary>
    [JsonStringEnumMemberName("ROLE_UNSPECIFIED")]
    Unspecified,

    /// <summary>
    /// Owner can use, update, share and delete the resource.
    /// </summary>
    [JsonStringEnumMemberName("OWNER")]
    Owner,

    /// <summary>
    /// Writer can use, update and share the resource.
    /// </summary>
    [JsonStringEnumMemberName("WRITER")]
    Writer,

    /// <summary>
    /// Reader can use the resource.
    /// </summary>
    [JsonStringEnumMemberName("READER")]
    Reader,
}


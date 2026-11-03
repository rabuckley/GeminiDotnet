using System.Text.Json;
using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta;

/// <summary>
/// Optional. Immutable. The type of the grantee.
/// </summary>
/// <remarks>
/// Unknown API values are preserved.
/// </remarks>
[JsonConverter(typeof(PermissionGranteeType.Converter))]
public readonly struct PermissionGranteeType : IEquatable<PermissionGranteeType>
{
    private readonly string? _value;

    /// <summary>
    /// Initializes a new instance of the <see cref="PermissionGranteeType"/> structure.
    /// </summary>
    /// <param name="value">
    /// The wire value, including values not declared by this type.
    /// </param>
    /// <exception cref="global::System.ArgumentNullException">
    /// <paramref name="value"/> is <see langword="null"/>.
    /// </exception>
    public PermissionGranteeType(string value)
    {
        global::System.ArgumentNullException.ThrowIfNull(value);
        _value = value;
    }

    /// <summary>
    /// The default value. This value is unused.
    /// </summary>
    public static PermissionGranteeType Unspecified { get; } = new("GRANTEE_TYPE_UNSPECIFIED");

    /// <summary>
    /// Represents a user. When set, you must provide email_address for the user.
    /// </summary>
    public static PermissionGranteeType User { get; } = new("USER");

    /// <summary>
    /// Represents a group. When set, you must provide email_address for the
    /// group.
    /// </summary>
    public static PermissionGranteeType Group { get; } = new("GROUP");

    /// <summary>
    /// Represents access to everyone. No extra information is required.
    /// </summary>
    public static PermissionGranteeType Everyone { get; } = new("EVERYONE");

    /// <summary>
    /// Gets the declared values in schema order.
    /// </summary>
    public static IReadOnlyList<PermissionGranteeType> KnownValues { get; } =
    [
        Unspecified,
        User,
        Group,
        Everyone,
    ];

    /// <summary>
    /// Gets the wire value.
    /// </summary>
    /// <value>
    /// The default instance has the value of <see cref="Unspecified"/>.
    /// </value>
    public string Value => _value ?? "GRANTEE_TYPE_UNSPECIFIED";

    /// <summary>
    /// Compares wire values using ordinal equality.
    /// </summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    public static bool operator ==(PermissionGranteeType left, PermissionGranteeType right) => left.Equals(right);

    /// <summary>
    /// Compares wire values using ordinal inequality.
    /// </summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    public static bool operator !=(PermissionGranteeType left, PermissionGranteeType right) => !left.Equals(right);

    /// <inheritdoc/>
    public bool Equals(PermissionGranteeType other) => string.Equals(Value, other.Value, global::System.StringComparison.Ordinal);

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is PermissionGranteeType other && Equals(other);

    /// <inheritdoc/>
    public override int GetHashCode() => global::System.StringComparer.Ordinal.GetHashCode(Value);

    /// <summary>
    /// Returns <see cref="Value"/>.
    /// </summary>
    public override string ToString() => Value;

    [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
    public sealed class Converter : JsonConverter<PermissionGranteeType>
    {
        /// <inheritdoc/>
        public override PermissionGranteeType Read(ref Utf8JsonReader reader, global::System.Type typeToConvert, JsonSerializerOptions options)
            => new(reader.GetString() ?? throw new JsonException("Expected a string for PermissionGranteeType."));

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, PermissionGranteeType value, JsonSerializerOptions options)
        {
            global::System.ArgumentNullException.ThrowIfNull(writer);
            writer.WriteStringValue(value.Value);
        }
    }
}


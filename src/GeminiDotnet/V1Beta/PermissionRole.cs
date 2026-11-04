using System.Text.Json;
using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta;

/// <summary>
/// Required. The role granted by this permission.
/// </summary>
/// <remarks>
/// Unknown API values are preserved.
/// </remarks>
[JsonConverter(typeof(PermissionRole.Converter))]
public readonly struct PermissionRole : IEquatable<PermissionRole>
{
    private readonly string? _value;

    /// <summary>
    /// Initializes a new instance of the <see cref="PermissionRole"/> structure.
    /// </summary>
    /// <param name="value">
    /// The wire value, including values not declared by this type.
    /// </param>
    /// <exception cref="global::System.ArgumentNullException">
    /// <paramref name="value"/> is <see langword="null"/>.
    /// </exception>
    public PermissionRole(string value)
    {
        global::System.ArgumentNullException.ThrowIfNull(value);
        _value = value;
    }

    /// <summary>
    /// The default value. This value is unused.
    /// </summary>
    public static PermissionRole Unspecified { get; } = new("ROLE_UNSPECIFIED");

    /// <summary>
    /// Owner can use, update, share and delete the resource.
    /// </summary>
    public static PermissionRole Owner { get; } = new("OWNER");

    /// <summary>
    /// Writer can use, update and share the resource.
    /// </summary>
    public static PermissionRole Writer { get; } = new("WRITER");

    /// <summary>
    /// Reader can use the resource.
    /// </summary>
    public static PermissionRole Reader { get; } = new("READER");

    /// <summary>
    /// Gets the declared values in schema order.
    /// </summary>
    public static IReadOnlyList<PermissionRole> KnownValues { get; } =
    [
        Unspecified,
        Owner,
        Writer,
        Reader,
    ];

    /// <summary>
    /// Gets the wire value.
    /// </summary>
    /// <value>
    /// The default instance has the value of <see cref="Unspecified"/>.
    /// </value>
    public string Value => _value ?? "ROLE_UNSPECIFIED";

    /// <summary>
    /// Compares wire values using ordinal equality.
    /// </summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    public static bool operator ==(PermissionRole left, PermissionRole right) => left.Equals(right);

    /// <summary>
    /// Compares wire values using ordinal inequality.
    /// </summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    public static bool operator !=(PermissionRole left, PermissionRole right) => !left.Equals(right);

    /// <inheritdoc/>
    public bool Equals(PermissionRole other) => string.Equals(Value, other.Value, global::System.StringComparison.Ordinal);

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is PermissionRole other && Equals(other);

    /// <inheritdoc/>
    public override int GetHashCode() => global::System.StringComparer.Ordinal.GetHashCode(Value);

    /// <summary>
    /// Returns <see cref="Value"/>.
    /// </summary>
    public override string ToString() => Value;

    [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
    public sealed class Converter : JsonConverter<PermissionRole>
    {
        /// <inheritdoc/>
        public override PermissionRole Read(ref Utf8JsonReader reader, global::System.Type typeToConvert, JsonSerializerOptions options)
            => new(reader.GetString() ?? throw new JsonException("Expected a string for PermissionRole."));

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, PermissionRole value, JsonSerializerOptions options)
        {
            global::System.ArgumentNullException.ThrowIfNull(writer);
            writer.WriteStringValue(value.Value);
        }
    }
}


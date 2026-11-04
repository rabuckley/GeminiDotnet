using System.Text.Json;
using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.Environments;

/// <remarks>
/// Unknown API values are preserved.
/// </remarks>
[JsonConverter(typeof(SourceType.Converter))]
public readonly struct SourceType : IEquatable<SourceType>
{
    private readonly string? _value;

    /// <summary>
    /// Initializes a new instance of the <see cref="SourceType"/> structure.
    /// </summary>
    /// <param name="value">
    /// The wire value, including values not declared by this type.
    /// </param>
    /// <exception cref="global::System.ArgumentNullException">
    /// <paramref name="value"/> is <see langword="null"/>.
    /// </exception>
    public SourceType(string value)
    {
        global::System.ArgumentNullException.ThrowIfNull(value);
        _value = value;
    }

    public static SourceType Unspecified { get; } = new("TYPE_UNSPECIFIED");

    /// <summary>
    /// A Cloud Storage bucket.
    /// </summary>
    public static SourceType Gcs { get; } = new("GCS");

    /// <summary>
    /// Inline content.
    /// </summary>
    public static SourceType Inline { get; } = new("INLINE");

    /// <summary>
    /// A generic repository. The protocol prefix in the source URL
    /// identifies the provider (e.g., github://, gcs://).
    /// </summary>
    public static SourceType Repository { get; } = new("REPOSITORY");

    /// <summary>
    /// Gets the declared values in schema order.
    /// </summary>
    public static IReadOnlyList<SourceType> KnownValues { get; } =
    [
        Unspecified,
        Gcs,
        Inline,
        Repository,
    ];

    /// <summary>
    /// Gets the wire value.
    /// </summary>
    /// <value>
    /// The default instance has the value of <see cref="Unspecified"/>.
    /// </value>
    public string Value => _value ?? "TYPE_UNSPECIFIED";

    /// <summary>
    /// Compares wire values using ordinal equality.
    /// </summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    public static bool operator ==(SourceType left, SourceType right) => left.Equals(right);

    /// <summary>
    /// Compares wire values using ordinal inequality.
    /// </summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    public static bool operator !=(SourceType left, SourceType right) => !left.Equals(right);

    /// <inheritdoc/>
    public bool Equals(SourceType other) => string.Equals(Value, other.Value, global::System.StringComparison.Ordinal);

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is SourceType other && Equals(other);

    /// <inheritdoc/>
    public override int GetHashCode() => global::System.StringComparer.Ordinal.GetHashCode(Value);

    /// <summary>
    /// Returns <see cref="Value"/>.
    /// </summary>
    public override string ToString() => Value;

    [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
    public sealed class Converter : JsonConverter<SourceType>
    {
        /// <inheritdoc/>
        public override SourceType Read(ref Utf8JsonReader reader, global::System.Type typeToConvert, JsonSerializerOptions options)
            => new(reader.GetString() ?? throw new JsonException("Expected a string for SourceType."));

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, SourceType value, JsonSerializerOptions options)
        {
            global::System.ArgumentNullException.ThrowIfNull(writer);
            writer.WriteStringValue(value.Value);
        }
    }
}


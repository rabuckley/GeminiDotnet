using System.Text.Json;
using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.Media;

/// <summary>
/// Describes what the field reference contains.
/// </summary>
/// <remarks>
/// Unknown API values are preserved.
/// </remarks>
[JsonConverter(typeof(CompositeMediaReferenceType.Converter))]
public readonly struct CompositeMediaReferenceType : IEquatable<CompositeMediaReferenceType>
{
    private readonly string? _value;

    /// <summary>
    /// Initializes a new instance of the <see cref="CompositeMediaReferenceType"/> structure.
    /// </summary>
    /// <param name="value">
    /// The wire value, including values not declared by this type.
    /// </param>
    /// <exception cref="global::System.ArgumentNullException">
    /// <paramref name="value"/> is <see langword="null"/>.
    /// </exception>
    public CompositeMediaReferenceType(string value)
    {
        global::System.ArgumentNullException.ThrowIfNull(value);
        _value = value;
    }

    /// <summary>
    /// Reference contains a GFS path or a local path.
    /// </summary>
    public static CompositeMediaReferenceType Path { get; } = new("PATH");

    /// <summary>
    /// Reference points to a blobstore object. This could be either
    /// a v1 blob_ref or a v2 blobstore2_info. Clients should check
    /// blobstore2_info first, since v1 is being deprecated.
    /// </summary>
    public static CompositeMediaReferenceType BlobRef { get; } = new("BLOB_REF");

    /// <summary>
    /// Data is included into this proto buffer
    /// </summary>
    public static CompositeMediaReferenceType Inline { get; } = new("INLINE");

    /// <summary>
    /// Reference points to a bigstore object
    /// </summary>
    public static CompositeMediaReferenceType BigstoreRef { get; } = new("BIGSTORE_REF");

    /// <summary>
    /// Indicates the data is stored in cosmo_binary_reference.
    /// </summary>
    public static CompositeMediaReferenceType CosmoBinaryReference { get; } = new("COSMO_BINARY_REFERENCE");

    /// <summary>
    /// Gets the declared values in schema order.
    /// </summary>
    public static IReadOnlyList<CompositeMediaReferenceType> KnownValues { get; } =
    [
        Path,
        BlobRef,
        Inline,
        BigstoreRef,
        CosmoBinaryReference,
    ];

    /// <summary>
    /// Gets the wire value.
    /// </summary>
    /// <value>
    /// The default instance has the value of <see cref="Path"/>.
    /// </value>
    public string Value => _value ?? "PATH";

    /// <summary>
    /// Compares wire values using ordinal equality.
    /// </summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    public static bool operator ==(CompositeMediaReferenceType left, CompositeMediaReferenceType right) => left.Equals(right);

    /// <summary>
    /// Compares wire values using ordinal inequality.
    /// </summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    public static bool operator !=(CompositeMediaReferenceType left, CompositeMediaReferenceType right) => !left.Equals(right);

    /// <inheritdoc/>
    public bool Equals(CompositeMediaReferenceType other) => string.Equals(Value, other.Value, global::System.StringComparison.Ordinal);

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is CompositeMediaReferenceType other && Equals(other);

    /// <inheritdoc/>
    public override int GetHashCode() => global::System.StringComparer.Ordinal.GetHashCode(Value);

    /// <summary>
    /// Returns <see cref="Value"/>.
    /// </summary>
    public override string ToString() => Value;

    [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
    public sealed class Converter : JsonConverter<CompositeMediaReferenceType>
    {
        /// <inheritdoc/>
        public override CompositeMediaReferenceType Read(ref Utf8JsonReader reader, global::System.Type typeToConvert, JsonSerializerOptions options)
            => new(reader.GetString() ?? throw new JsonException("Expected a string for CompositeMediaReferenceType."));

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, CompositeMediaReferenceType value, JsonSerializerOptions options)
        {
            global::System.ArgumentNullException.ThrowIfNull(writer);
            writer.WriteStringValue(value.Value);
        }
    }
}


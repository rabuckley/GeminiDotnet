using System.Text.Json;
using System.Text.Json.Serialization;

namespace GeminiDotnet.V1;

/// <summary>
/// Optional. The MIME type of the image output.
/// </summary>
/// <remarks>
/// Unknown API values are preserved.
/// </remarks>
[JsonConverter(typeof(ImageResponseFormatMimeType.Converter))]
public readonly struct ImageResponseFormatMimeType : IEquatable<ImageResponseFormatMimeType>
{
    private readonly string? _value;

    /// <summary>
    /// Initializes a new instance of the <see cref="ImageResponseFormatMimeType"/> structure.
    /// </summary>
    /// <param name="value">
    /// The wire value, including values not declared by this type.
    /// </param>
    /// <exception cref="global::System.ArgumentNullException">
    /// <paramref name="value"/> is <see langword="null"/>.
    /// </exception>
    public ImageResponseFormatMimeType(string value)
    {
        global::System.ArgumentNullException.ThrowIfNull(value);
        _value = value;
    }

    /// <summary>
    /// Default value. This value is unused.
    /// </summary>
    public static ImageResponseFormatMimeType Unspecified { get; } = new("MIME_TYPE_UNSPECIFIED");

    /// <summary>
    /// JPEG image format.
    /// </summary>
    public static ImageResponseFormatMimeType ImageJpeg { get; } = new("IMAGE_JPEG");

    /// <summary>
    /// Gets the declared values in schema order.
    /// </summary>
    public static IReadOnlyList<ImageResponseFormatMimeType> KnownValues { get; } =
    [
        Unspecified,
        ImageJpeg,
    ];

    /// <summary>
    /// Gets the wire value.
    /// </summary>
    /// <value>
    /// The default instance has the value of <see cref="Unspecified"/>.
    /// </value>
    public string Value => _value ?? "MIME_TYPE_UNSPECIFIED";

    /// <summary>
    /// Compares wire values using ordinal equality.
    /// </summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    public static bool operator ==(ImageResponseFormatMimeType left, ImageResponseFormatMimeType right) => left.Equals(right);

    /// <summary>
    /// Compares wire values using ordinal inequality.
    /// </summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    public static bool operator !=(ImageResponseFormatMimeType left, ImageResponseFormatMimeType right) => !left.Equals(right);

    /// <inheritdoc/>
    public bool Equals(ImageResponseFormatMimeType other) => string.Equals(Value, other.Value, global::System.StringComparison.Ordinal);

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is ImageResponseFormatMimeType other && Equals(other);

    /// <inheritdoc/>
    public override int GetHashCode() => global::System.StringComparer.Ordinal.GetHashCode(Value);

    /// <summary>
    /// Returns <see cref="Value"/>.
    /// </summary>
    public override string ToString() => Value;

    [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
    public sealed class Converter : JsonConverter<ImageResponseFormatMimeType>
    {
        /// <inheritdoc/>
        public override ImageResponseFormatMimeType Read(ref Utf8JsonReader reader, global::System.Type typeToConvert, JsonSerializerOptions options)
            => new(reader.GetString() ?? throw new JsonException("Expected a string for ImageResponseFormatMimeType."));

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, ImageResponseFormatMimeType value, JsonSerializerOptions options)
        {
            global::System.ArgumentNullException.ThrowIfNull(writer);
            writer.WriteStringValue(value.Value);
        }
    }
}


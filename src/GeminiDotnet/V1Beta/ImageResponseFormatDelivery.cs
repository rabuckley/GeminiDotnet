using System.Text.Json;
using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta;

/// <summary>
/// Optional. The delivery mode for the image output.
/// </summary>
/// <remarks>
/// Unknown API values are preserved.
/// </remarks>
[JsonConverter(typeof(ImageResponseFormatDelivery.Converter))]
public readonly struct ImageResponseFormatDelivery : IEquatable<ImageResponseFormatDelivery>
{
    private readonly string? _value;

    /// <summary>
    /// Initializes a new instance of the <see cref="ImageResponseFormatDelivery"/> structure.
    /// </summary>
    /// <param name="value">
    /// The wire value, including values not declared by this type.
    /// </param>
    /// <exception cref="global::System.ArgumentNullException">
    /// <paramref name="value"/> is <see langword="null"/>.
    /// </exception>
    public ImageResponseFormatDelivery(string value)
    {
        global::System.ArgumentNullException.ThrowIfNull(value);
        _value = value;
    }

    /// <summary>
    /// Default value. This value is unused.
    /// </summary>
    public static ImageResponseFormatDelivery Unspecified { get; } = new("DELIVERY_UNSPECIFIED");

    /// <summary>
    /// Image data is returned inline in the response.
    /// </summary>
    public static ImageResponseFormatDelivery Inline { get; } = new("INLINE");

    /// <summary>
    /// Image data is returned as a URI.
    /// </summary>
    public static ImageResponseFormatDelivery Uri { get; } = new("URI");

    /// <summary>
    /// Gets the declared values in schema order.
    /// </summary>
    public static IReadOnlyList<ImageResponseFormatDelivery> KnownValues { get; } =
    [
        Unspecified,
        Inline,
        Uri,
    ];

    /// <summary>
    /// Gets the wire value.
    /// </summary>
    /// <value>
    /// The default instance has the value of <see cref="Unspecified"/>.
    /// </value>
    public string Value => _value ?? "DELIVERY_UNSPECIFIED";

    /// <summary>
    /// Compares wire values using ordinal equality.
    /// </summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    public static bool operator ==(ImageResponseFormatDelivery left, ImageResponseFormatDelivery right) => left.Equals(right);

    /// <summary>
    /// Compares wire values using ordinal inequality.
    /// </summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    public static bool operator !=(ImageResponseFormatDelivery left, ImageResponseFormatDelivery right) => !left.Equals(right);

    /// <inheritdoc/>
    public bool Equals(ImageResponseFormatDelivery other) => string.Equals(Value, other.Value, global::System.StringComparison.Ordinal);

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is ImageResponseFormatDelivery other && Equals(other);

    /// <inheritdoc/>
    public override int GetHashCode() => global::System.StringComparer.Ordinal.GetHashCode(Value);

    /// <summary>
    /// Returns <see cref="Value"/>.
    /// </summary>
    public override string ToString() => Value;

    [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
    public sealed class Converter : JsonConverter<ImageResponseFormatDelivery>
    {
        /// <inheritdoc/>
        public override ImageResponseFormatDelivery Read(ref Utf8JsonReader reader, global::System.Type typeToConvert, JsonSerializerOptions options)
            => new(reader.GetString() ?? throw new JsonException("Expected a string for ImageResponseFormatDelivery."));

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, ImageResponseFormatDelivery value, JsonSerializerOptions options)
        {
            global::System.ArgumentNullException.ThrowIfNull(writer);
            writer.WriteStringValue(value.Value);
        }
    }
}


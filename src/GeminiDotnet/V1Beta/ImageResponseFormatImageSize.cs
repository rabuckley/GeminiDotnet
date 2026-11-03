using System.Text.Json;
using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta;

/// <summary>
/// Optional. The size of the image output.
/// </summary>
/// <remarks>
/// Unknown API values are preserved.
/// </remarks>
[JsonConverter(typeof(ImageResponseFormatImageSize.Converter))]
public readonly struct ImageResponseFormatImageSize : IEquatable<ImageResponseFormatImageSize>
{
    private readonly string? _value;

    /// <summary>
    /// Initializes a new instance of the <see cref="ImageResponseFormatImageSize"/> structure.
    /// </summary>
    /// <param name="value">
    /// The wire value, including values not declared by this type.
    /// </param>
    /// <exception cref="global::System.ArgumentNullException">
    /// <paramref name="value"/> is <see langword="null"/>.
    /// </exception>
    public ImageResponseFormatImageSize(string value)
    {
        global::System.ArgumentNullException.ThrowIfNull(value);
        _value = value;
    }

    /// <summary>
    /// Default value. This value is unused.
    /// </summary>
    public static ImageResponseFormatImageSize Unspecified { get; } = new("IMAGE_SIZE_UNSPECIFIED");

    /// <summary>
    /// 512px image size.
    /// </summary>
    public static ImageResponseFormatImageSize FiveTwelve { get; } = new("IMAGE_SIZE_FIVE_TWELVE");

    /// <summary>
    /// 1K image size.
    /// </summary>
    public static ImageResponseFormatImageSize OneK { get; } = new("IMAGE_SIZE_ONE_K");

    /// <summary>
    /// 2K image size.
    /// </summary>
    public static ImageResponseFormatImageSize TwoK { get; } = new("IMAGE_SIZE_TWO_K");

    /// <summary>
    /// 4K image size.
    /// </summary>
    public static ImageResponseFormatImageSize FourK { get; } = new("IMAGE_SIZE_FOUR_K");

    /// <summary>
    /// Gets the declared values in schema order.
    /// </summary>
    public static IReadOnlyList<ImageResponseFormatImageSize> KnownValues { get; } =
    [
        Unspecified,
        FiveTwelve,
        OneK,
        TwoK,
        FourK,
    ];

    /// <summary>
    /// Gets the wire value.
    /// </summary>
    /// <value>
    /// The default instance has the value of <see cref="Unspecified"/>.
    /// </value>
    public string Value => _value ?? "IMAGE_SIZE_UNSPECIFIED";

    /// <summary>
    /// Compares wire values using ordinal equality.
    /// </summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    public static bool operator ==(ImageResponseFormatImageSize left, ImageResponseFormatImageSize right) => left.Equals(right);

    /// <summary>
    /// Compares wire values using ordinal inequality.
    /// </summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    public static bool operator !=(ImageResponseFormatImageSize left, ImageResponseFormatImageSize right) => !left.Equals(right);

    /// <inheritdoc/>
    public bool Equals(ImageResponseFormatImageSize other) => string.Equals(Value, other.Value, global::System.StringComparison.Ordinal);

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is ImageResponseFormatImageSize other && Equals(other);

    /// <inheritdoc/>
    public override int GetHashCode() => global::System.StringComparer.Ordinal.GetHashCode(Value);

    /// <summary>
    /// Returns <see cref="Value"/>.
    /// </summary>
    public override string ToString() => Value;

    [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
    public sealed class Converter : JsonConverter<ImageResponseFormatImageSize>
    {
        /// <inheritdoc/>
        public override ImageResponseFormatImageSize Read(ref Utf8JsonReader reader, global::System.Type typeToConvert, JsonSerializerOptions options)
            => new(reader.GetString() ?? throw new JsonException("Expected a string for ImageResponseFormatImageSize."));

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, ImageResponseFormatImageSize value, JsonSerializerOptions options)
        {
            global::System.ArgumentNullException.ThrowIfNull(writer);
            writer.WriteStringValue(value.Value);
        }
    }
}


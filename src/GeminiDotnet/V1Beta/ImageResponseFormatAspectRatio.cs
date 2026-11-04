using System.Text.Json;
using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta;

/// <summary>
/// Optional. The aspect ratio for the image output.
/// </summary>
/// <remarks>
/// Unknown API values are preserved.
/// </remarks>
[JsonConverter(typeof(ImageResponseFormatAspectRatio.Converter))]
public readonly struct ImageResponseFormatAspectRatio : IEquatable<ImageResponseFormatAspectRatio>
{
    private readonly string? _value;

    /// <summary>
    /// Initializes a new instance of the <see cref="ImageResponseFormatAspectRatio"/> structure.
    /// </summary>
    /// <param name="value">
    /// The wire value, including values not declared by this type.
    /// </param>
    /// <exception cref="global::System.ArgumentNullException">
    /// <paramref name="value"/> is <see langword="null"/>.
    /// </exception>
    public ImageResponseFormatAspectRatio(string value)
    {
        global::System.ArgumentNullException.ThrowIfNull(value);
        _value = value;
    }

    /// <summary>
    /// Default value. This value is unused.
    /// </summary>
    public static ImageResponseFormatAspectRatio Unspecified { get; } = new("ASPECT_RATIO_UNSPECIFIED");

    /// <summary>
    /// 1:1 aspect ratio.
    /// </summary>
    public static ImageResponseFormatAspectRatio OneByOne { get; } = new("ASPECT_RATIO_ONE_BY_ONE");

    /// <summary>
    /// 2:3 aspect ratio.
    /// </summary>
    public static ImageResponseFormatAspectRatio TwoByThree { get; } = new("ASPECT_RATIO_TWO_BY_THREE");

    /// <summary>
    /// 3:2 aspect ratio.
    /// </summary>
    public static ImageResponseFormatAspectRatio ThreeByTwo { get; } = new("ASPECT_RATIO_THREE_BY_TWO");

    /// <summary>
    /// 3:4 aspect ratio.
    /// </summary>
    public static ImageResponseFormatAspectRatio ThreeByFour { get; } = new("ASPECT_RATIO_THREE_BY_FOUR");

    /// <summary>
    /// 4:3 aspect ratio.
    /// </summary>
    public static ImageResponseFormatAspectRatio FourByThree { get; } = new("ASPECT_RATIO_FOUR_BY_THREE");

    /// <summary>
    /// 4:5 aspect ratio.
    /// </summary>
    public static ImageResponseFormatAspectRatio FourByFive { get; } = new("ASPECT_RATIO_FOUR_BY_FIVE");

    /// <summary>
    /// 5:4 aspect ratio.
    /// </summary>
    public static ImageResponseFormatAspectRatio FiveByFour { get; } = new("ASPECT_RATIO_FIVE_BY_FOUR");

    /// <summary>
    /// 9:16 aspect ratio.
    /// </summary>
    public static ImageResponseFormatAspectRatio NineBySixteen { get; } = new("ASPECT_RATIO_NINE_BY_SIXTEEN");

    /// <summary>
    /// 16:9 aspect ratio.
    /// </summary>
    public static ImageResponseFormatAspectRatio SixteenByNine { get; } = new("ASPECT_RATIO_SIXTEEN_BY_NINE");

    /// <summary>
    /// 21:9 aspect ratio.
    /// </summary>
    public static ImageResponseFormatAspectRatio TwentyOneByNine { get; } = new("ASPECT_RATIO_TWENTY_ONE_BY_NINE");

    /// <summary>
    /// 1:8 aspect ratio.
    /// </summary>
    public static ImageResponseFormatAspectRatio OneByEight { get; } = new("ASPECT_RATIO_ONE_BY_EIGHT");

    /// <summary>
    /// 8:1 aspect ratio.
    /// </summary>
    public static ImageResponseFormatAspectRatio EightByOne { get; } = new("ASPECT_RATIO_EIGHT_BY_ONE");

    /// <summary>
    /// 1:4 aspect ratio.
    /// </summary>
    public static ImageResponseFormatAspectRatio OneByFour { get; } = new("ASPECT_RATIO_ONE_BY_FOUR");

    /// <summary>
    /// 4:1 aspect ratio.
    /// </summary>
    public static ImageResponseFormatAspectRatio FourByOne { get; } = new("ASPECT_RATIO_FOUR_BY_ONE");

    /// <summary>
    /// Gets the declared values in schema order.
    /// </summary>
    public static IReadOnlyList<ImageResponseFormatAspectRatio> KnownValues { get; } =
    [
        Unspecified,
        OneByOne,
        TwoByThree,
        ThreeByTwo,
        ThreeByFour,
        FourByThree,
        FourByFive,
        FiveByFour,
        NineBySixteen,
        SixteenByNine,
        TwentyOneByNine,
        OneByEight,
        EightByOne,
        OneByFour,
        FourByOne,
    ];

    /// <summary>
    /// Gets the wire value.
    /// </summary>
    /// <value>
    /// The default instance has the value of <see cref="Unspecified"/>.
    /// </value>
    public string Value => _value ?? "ASPECT_RATIO_UNSPECIFIED";

    /// <summary>
    /// Compares wire values using ordinal equality.
    /// </summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    public static bool operator ==(ImageResponseFormatAspectRatio left, ImageResponseFormatAspectRatio right) => left.Equals(right);

    /// <summary>
    /// Compares wire values using ordinal inequality.
    /// </summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    public static bool operator !=(ImageResponseFormatAspectRatio left, ImageResponseFormatAspectRatio right) => !left.Equals(right);

    /// <inheritdoc/>
    public bool Equals(ImageResponseFormatAspectRatio other) => string.Equals(Value, other.Value, global::System.StringComparison.Ordinal);

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is ImageResponseFormatAspectRatio other && Equals(other);

    /// <inheritdoc/>
    public override int GetHashCode() => global::System.StringComparer.Ordinal.GetHashCode(Value);

    /// <summary>
    /// Returns <see cref="Value"/>.
    /// </summary>
    public override string ToString() => Value;

    [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
    public sealed class Converter : JsonConverter<ImageResponseFormatAspectRatio>
    {
        /// <inheritdoc/>
        public override ImageResponseFormatAspectRatio Read(ref Utf8JsonReader reader, global::System.Type typeToConvert, JsonSerializerOptions options)
            => new(reader.GetString() ?? throw new JsonException("Expected a string for ImageResponseFormatAspectRatio."));

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, ImageResponseFormatAspectRatio value, JsonSerializerOptions options)
        {
            global::System.ArgumentNullException.ThrowIfNull(writer);
            writer.WriteStringValue(value.Value);
        }
    }
}


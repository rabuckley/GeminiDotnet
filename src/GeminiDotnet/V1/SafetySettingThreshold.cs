using System.Text.Json;
using System.Text.Json.Serialization;

namespace GeminiDotnet.V1;

/// <summary>
/// Required. Controls the probability threshold at which harm is blocked.
/// </summary>
/// <remarks>
/// Unknown API values are preserved.
/// </remarks>
[JsonConverter(typeof(SafetySettingThreshold.Converter))]
public readonly struct SafetySettingThreshold : IEquatable<SafetySettingThreshold>
{
    private readonly string? _value;

    /// <summary>
    /// Initializes a new instance of the <see cref="SafetySettingThreshold"/> structure.
    /// </summary>
    /// <param name="value">
    /// The wire value, including values not declared by this type.
    /// </param>
    /// <exception cref="global::System.ArgumentNullException">
    /// <paramref name="value"/> is <see langword="null"/>.
    /// </exception>
    public SafetySettingThreshold(string value)
    {
        global::System.ArgumentNullException.ThrowIfNull(value);
        _value = value;
    }

    /// <summary>
    /// Threshold is unspecified.
    /// </summary>
    public static SafetySettingThreshold HarmBlockThresholdUnspecified { get; } = new("HARM_BLOCK_THRESHOLD_UNSPECIFIED");

    /// <summary>
    /// Content with NEGLIGIBLE will be allowed.
    /// </summary>
    public static SafetySettingThreshold BlockLowAndAbove { get; } = new("BLOCK_LOW_AND_ABOVE");

    /// <summary>
    /// Content with NEGLIGIBLE and LOW will be allowed.
    /// </summary>
    public static SafetySettingThreshold BlockMediumAndAbove { get; } = new("BLOCK_MEDIUM_AND_ABOVE");

    /// <summary>
    /// Content with NEGLIGIBLE, LOW, and MEDIUM will be allowed.
    /// </summary>
    public static SafetySettingThreshold BlockOnlyHigh { get; } = new("BLOCK_ONLY_HIGH");

    /// <summary>
    /// All content will be allowed.
    /// </summary>
    public static SafetySettingThreshold BlockNone { get; } = new("BLOCK_NONE");

    /// <summary>
    /// Turn off the safety filter.
    /// </summary>
    public static SafetySettingThreshold Off { get; } = new("OFF");

    /// <summary>
    /// Gets the declared values in schema order.
    /// </summary>
    public static IReadOnlyList<SafetySettingThreshold> KnownValues { get; } =
    [
        HarmBlockThresholdUnspecified,
        BlockLowAndAbove,
        BlockMediumAndAbove,
        BlockOnlyHigh,
        BlockNone,
        Off,
    ];

    /// <summary>
    /// Gets the wire value.
    /// </summary>
    /// <value>
    /// The default instance has the value of <see cref="HarmBlockThresholdUnspecified"/>.
    /// </value>
    public string Value => _value ?? "HARM_BLOCK_THRESHOLD_UNSPECIFIED";

    /// <summary>
    /// Compares wire values using ordinal equality.
    /// </summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    public static bool operator ==(SafetySettingThreshold left, SafetySettingThreshold right) => left.Equals(right);

    /// <summary>
    /// Compares wire values using ordinal inequality.
    /// </summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    public static bool operator !=(SafetySettingThreshold left, SafetySettingThreshold right) => !left.Equals(right);

    /// <inheritdoc/>
    public bool Equals(SafetySettingThreshold other) => string.Equals(Value, other.Value, global::System.StringComparison.Ordinal);

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is SafetySettingThreshold other && Equals(other);

    /// <inheritdoc/>
    public override int GetHashCode() => global::System.StringComparer.Ordinal.GetHashCode(Value);

    /// <summary>
    /// Returns <see cref="Value"/>.
    /// </summary>
    public override string ToString() => Value;

    [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
    public sealed class Converter : JsonConverter<SafetySettingThreshold>
    {
        /// <inheritdoc/>
        public override SafetySettingThreshold Read(ref Utf8JsonReader reader, global::System.Type typeToConvert, JsonSerializerOptions options)
            => new(reader.GetString() ?? throw new JsonException("Expected a string for SafetySettingThreshold."));

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, SafetySettingThreshold value, JsonSerializerOptions options)
        {
            global::System.ArgumentNullException.ThrowIfNull(writer);
            writer.WriteStringValue(value.Value);
        }
    }
}


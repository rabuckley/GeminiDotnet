using System.Text.Json;
using System.Text.Json.Serialization;

namespace GeminiDotnet.V1;

/// <summary>
/// Required. The probability of harm for this content.
/// </summary>
/// <remarks>
/// Unknown API values are preserved.
/// </remarks>
[JsonConverter(typeof(SafetyRatingProbability.Converter))]
public readonly struct SafetyRatingProbability : IEquatable<SafetyRatingProbability>
{
    private readonly string? _value;

    /// <summary>
    /// Initializes a new instance of the <see cref="SafetyRatingProbability"/> structure.
    /// </summary>
    /// <param name="value">
    /// The wire value, including values not declared by this type.
    /// </param>
    /// <exception cref="global::System.ArgumentNullException">
    /// <paramref name="value"/> is <see langword="null"/>.
    /// </exception>
    public SafetyRatingProbability(string value)
    {
        global::System.ArgumentNullException.ThrowIfNull(value);
        _value = value;
    }

    /// <summary>
    /// Probability is unspecified.
    /// </summary>
    public static SafetyRatingProbability HarmProbabilityUnspecified { get; } = new("HARM_PROBABILITY_UNSPECIFIED");

    /// <summary>
    /// Content has a negligible chance of being unsafe.
    /// </summary>
    public static SafetyRatingProbability Negligible { get; } = new("NEGLIGIBLE");

    /// <summary>
    /// Content has a low chance of being unsafe.
    /// </summary>
    public static SafetyRatingProbability Low { get; } = new("LOW");

    /// <summary>
    /// Content has a medium chance of being unsafe.
    /// </summary>
    public static SafetyRatingProbability Medium { get; } = new("MEDIUM");

    /// <summary>
    /// Content has a high chance of being unsafe.
    /// </summary>
    public static SafetyRatingProbability High { get; } = new("HIGH");

    /// <summary>
    /// Gets the declared values in schema order.
    /// </summary>
    public static IReadOnlyList<SafetyRatingProbability> KnownValues { get; } =
    [
        HarmProbabilityUnspecified,
        Negligible,
        Low,
        Medium,
        High,
    ];

    /// <summary>
    /// Gets the wire value.
    /// </summary>
    /// <value>
    /// The default instance has the value of <see cref="HarmProbabilityUnspecified"/>.
    /// </value>
    public string Value => _value ?? "HARM_PROBABILITY_UNSPECIFIED";

    /// <summary>
    /// Compares wire values using ordinal equality.
    /// </summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    public static bool operator ==(SafetyRatingProbability left, SafetyRatingProbability right) => left.Equals(right);

    /// <summary>
    /// Compares wire values using ordinal inequality.
    /// </summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    public static bool operator !=(SafetyRatingProbability left, SafetyRatingProbability right) => !left.Equals(right);

    /// <inheritdoc/>
    public bool Equals(SafetyRatingProbability other) => string.Equals(Value, other.Value, global::System.StringComparison.Ordinal);

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is SafetyRatingProbability other && Equals(other);

    /// <inheritdoc/>
    public override int GetHashCode() => global::System.StringComparer.Ordinal.GetHashCode(Value);

    /// <summary>
    /// Returns <see cref="Value"/>.
    /// </summary>
    public override string ToString() => Value;

    [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
    public sealed class Converter : JsonConverter<SafetyRatingProbability>
    {
        /// <inheritdoc/>
        public override SafetyRatingProbability Read(ref Utf8JsonReader reader, global::System.Type typeToConvert, JsonSerializerOptions options)
            => new(reader.GetString() ?? throw new JsonException("Expected a string for SafetyRatingProbability."));

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, SafetyRatingProbability value, JsonSerializerOptions options)
        {
            global::System.ArgumentNullException.ThrowIfNull(writer);
            writer.WriteStringValue(value.Value);
        }
    }
}


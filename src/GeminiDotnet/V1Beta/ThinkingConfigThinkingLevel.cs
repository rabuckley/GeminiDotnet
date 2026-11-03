using System.Text.Json;
using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta;

/// <summary>
/// Optional. Controls the maximum depth of the model's internal reasoning process before
/// it produces a response. The default value is model-dependent. Refer to the
/// [Thinking levels
/// guide](https://aistudio.google.com/docs/thinking#thinking-levels) for
/// more details. Recommended for Gemini 3 or later models. Use with earlier
/// models results in an error.
/// </summary>
/// <remarks>
/// Unknown API values are preserved.
/// </remarks>
[JsonConverter(typeof(ThinkingConfigThinkingLevel.Converter))]
public readonly struct ThinkingConfigThinkingLevel : IEquatable<ThinkingConfigThinkingLevel>
{
    private readonly string? _value;

    /// <summary>
    /// Initializes a new instance of the <see cref="ThinkingConfigThinkingLevel"/> structure.
    /// </summary>
    /// <param name="value">
    /// The wire value, including values not declared by this type.
    /// </param>
    /// <exception cref="global::System.ArgumentNullException">
    /// <paramref name="value"/> is <see langword="null"/>.
    /// </exception>
    public ThinkingConfigThinkingLevel(string value)
    {
        global::System.ArgumentNullException.ThrowIfNull(value);
        _value = value;
    }

    /// <summary>
    /// Default value.
    /// </summary>
    public static ThinkingConfigThinkingLevel Unspecified { get; } = new("THINKING_LEVEL_UNSPECIFIED");

    /// <summary>
    /// Little to no thinking.
    /// </summary>
    public static ThinkingConfigThinkingLevel Minimal { get; } = new("MINIMAL");

    /// <summary>
    /// Low thinking level.
    /// </summary>
    public static ThinkingConfigThinkingLevel Low { get; } = new("LOW");

    /// <summary>
    /// Medium thinking level.
    /// </summary>
    public static ThinkingConfigThinkingLevel Medium { get; } = new("MEDIUM");

    /// <summary>
    /// High thinking level.
    /// </summary>
    public static ThinkingConfigThinkingLevel High { get; } = new("HIGH");

    /// <summary>
    /// Gets the declared values in schema order.
    /// </summary>
    public static IReadOnlyList<ThinkingConfigThinkingLevel> KnownValues { get; } =
    [
        Unspecified,
        Minimal,
        Low,
        Medium,
        High,
    ];

    /// <summary>
    /// Gets the wire value.
    /// </summary>
    /// <value>
    /// The default instance has the value of <see cref="Unspecified"/>.
    /// </value>
    public string Value => _value ?? "THINKING_LEVEL_UNSPECIFIED";

    /// <summary>
    /// Compares wire values using ordinal equality.
    /// </summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    public static bool operator ==(ThinkingConfigThinkingLevel left, ThinkingConfigThinkingLevel right) => left.Equals(right);

    /// <summary>
    /// Compares wire values using ordinal inequality.
    /// </summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    public static bool operator !=(ThinkingConfigThinkingLevel left, ThinkingConfigThinkingLevel right) => !left.Equals(right);

    /// <inheritdoc/>
    public bool Equals(ThinkingConfigThinkingLevel other) => string.Equals(Value, other.Value, global::System.StringComparison.Ordinal);

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is ThinkingConfigThinkingLevel other && Equals(other);

    /// <inheritdoc/>
    public override int GetHashCode() => global::System.StringComparer.Ordinal.GetHashCode(Value);

    /// <summary>
    /// Returns <see cref="Value"/>.
    /// </summary>
    public override string ToString() => Value;

    [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
    public sealed class Converter : JsonConverter<ThinkingConfigThinkingLevel>
    {
        /// <inheritdoc/>
        public override ThinkingConfigThinkingLevel Read(ref Utf8JsonReader reader, global::System.Type typeToConvert, JsonSerializerOptions options)
            => new(reader.GetString() ?? throw new JsonException("Expected a string for ThinkingConfigThinkingLevel."));

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, ThinkingConfigThinkingLevel value, JsonSerializerOptions options)
        {
            global::System.ArgumentNullException.ThrowIfNull(writer);
            writer.WriteStringValue(value.Value);
        }
    }
}


using System.Text.Json;
using System.Text.Json.Serialization;

namespace GeminiDotnet.V1;

/// <summary>
/// Optional. How the model processes this part's media for understanding.
/// Only meaningful for video parts (<c>inline_data</c> or <c>file_data</c> with video
/// mime). Non-video parts ignore this field.
/// </summary>
/// <remarks>
/// Unknown API values are preserved.
/// </remarks>
[JsonConverter(typeof(PartMediaProcessing.Converter))]
public readonly struct PartMediaProcessing : IEquatable<PartMediaProcessing>
{
    private readonly string? _value;

    /// <summary>
    /// Initializes a new instance of the <see cref="PartMediaProcessing"/> structure.
    /// </summary>
    /// <param name="value">
    /// The wire value, including values not declared by this type.
    /// </param>
    /// <exception cref="global::System.ArgumentNullException">
    /// <paramref name="value"/> is <see langword="null"/>.
    /// </exception>
    public PartMediaProcessing(string value)
    {
        global::System.ArgumentNullException.ThrowIfNull(value);
        _value = value;
    }

    /// <summary>
    /// Defaults to model-specific processing.
    /// </summary>
    public static PartMediaProcessing Unspecified { get; } = new("MEDIA_PROCESSING_UNSPECIFIED");

    /// <summary>
    /// Fixed-rate frame extraction. All frames placed in context.
    /// </summary>
    public static PartMediaProcessing Static { get; } = new("STATIC");

    /// <summary>
    /// Model-driven dynamic navigation. Recommended for most use cases.
    /// </summary>
    public static PartMediaProcessing Agentic { get; } = new("AGENTIC");

    /// <summary>
    /// Gets the declared values in schema order.
    /// </summary>
    public static IReadOnlyList<PartMediaProcessing> KnownValues { get; } =
    [
        Unspecified,
        Static,
        Agentic,
    ];

    /// <summary>
    /// Gets the wire value.
    /// </summary>
    /// <value>
    /// The default instance has the value of <see cref="Unspecified"/>.
    /// </value>
    public string Value => _value ?? "MEDIA_PROCESSING_UNSPECIFIED";

    /// <summary>
    /// Compares wire values using ordinal equality.
    /// </summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    public static bool operator ==(PartMediaProcessing left, PartMediaProcessing right) => left.Equals(right);

    /// <summary>
    /// Compares wire values using ordinal inequality.
    /// </summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    public static bool operator !=(PartMediaProcessing left, PartMediaProcessing right) => !left.Equals(right);

    /// <inheritdoc/>
    public bool Equals(PartMediaProcessing other) => string.Equals(Value, other.Value, global::System.StringComparison.Ordinal);

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is PartMediaProcessing other && Equals(other);

    /// <inheritdoc/>
    public override int GetHashCode() => global::System.StringComparer.Ordinal.GetHashCode(Value);

    /// <summary>
    /// Returns <see cref="Value"/>.
    /// </summary>
    public override string ToString() => Value;

    [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
    public sealed class Converter : JsonConverter<PartMediaProcessing>
    {
        /// <inheritdoc/>
        public override PartMediaProcessing Read(ref Utf8JsonReader reader, global::System.Type typeToConvert, JsonSerializerOptions options)
            => new(reader.GetString() ?? throw new JsonException("Expected a string for PartMediaProcessing."));

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, PartMediaProcessing value, JsonSerializerOptions options)
        {
            global::System.ArgumentNullException.ThrowIfNull(writer);
            writer.WriteStringValue(value.Value);
        }
    }
}


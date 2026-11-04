using System.Text.Json;
using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta;

/// <summary>
/// Optional. How the model processes this part's media for understanding.
/// Only meaningful for video parts (<c>inline_data</c> or <c>file_data</c> with video
/// mime). Non-video parts ignore this field.
/// </summary>
/// <remarks>
/// Unknown API values are preserved.
/// </remarks>
[JsonConverter(typeof(FunctionResponsePartMediaProcessing.Converter))]
public readonly struct FunctionResponsePartMediaProcessing : IEquatable<FunctionResponsePartMediaProcessing>
{
    private readonly string? _value;

    /// <summary>
    /// Initializes a new instance of the <see cref="FunctionResponsePartMediaProcessing"/> structure.
    /// </summary>
    /// <param name="value">
    /// The wire value, including values not declared by this type.
    /// </param>
    /// <exception cref="global::System.ArgumentNullException">
    /// <paramref name="value"/> is <see langword="null"/>.
    /// </exception>
    public FunctionResponsePartMediaProcessing(string value)
    {
        global::System.ArgumentNullException.ThrowIfNull(value);
        _value = value;
    }

    /// <summary>
    /// Defaults to model-specific processing.
    /// </summary>
    public static FunctionResponsePartMediaProcessing Unspecified { get; } = new("MEDIA_PROCESSING_UNSPECIFIED");

    /// <summary>
    /// Fixed-rate frame extraction. All frames placed in context.
    /// </summary>
    public static FunctionResponsePartMediaProcessing Static { get; } = new("STATIC");

    /// <summary>
    /// Model-driven dynamic navigation. Recommended for most use cases.
    /// </summary>
    public static FunctionResponsePartMediaProcessing Agentic { get; } = new("AGENTIC");

    /// <summary>
    /// Gets the declared values in schema order.
    /// </summary>
    public static IReadOnlyList<FunctionResponsePartMediaProcessing> KnownValues { get; } =
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
    public static bool operator ==(FunctionResponsePartMediaProcessing left, FunctionResponsePartMediaProcessing right) => left.Equals(right);

    /// <summary>
    /// Compares wire values using ordinal inequality.
    /// </summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    public static bool operator !=(FunctionResponsePartMediaProcessing left, FunctionResponsePartMediaProcessing right) => !left.Equals(right);

    /// <inheritdoc/>
    public bool Equals(FunctionResponsePartMediaProcessing other) => string.Equals(Value, other.Value, global::System.StringComparison.Ordinal);

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is FunctionResponsePartMediaProcessing other && Equals(other);

    /// <inheritdoc/>
    public override int GetHashCode() => global::System.StringComparer.Ordinal.GetHashCode(Value);

    /// <summary>
    /// Returns <see cref="Value"/>.
    /// </summary>
    public override string ToString() => Value;

    [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
    public sealed class Converter : JsonConverter<FunctionResponsePartMediaProcessing>
    {
        /// <inheritdoc/>
        public override FunctionResponsePartMediaProcessing Read(ref Utf8JsonReader reader, global::System.Type typeToConvert, JsonSerializerOptions options)
            => new(reader.GetString() ?? throw new JsonException("Expected a string for FunctionResponsePartMediaProcessing."));

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, FunctionResponsePartMediaProcessing value, JsonSerializerOptions options)
        {
            global::System.ArgumentNullException.ThrowIfNull(writer);
            writer.WriteStringValue(value.Value);
        }
    }
}


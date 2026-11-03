using System.Text.Json;
using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta;

/// <summary>
/// Optional. Specifies how the response should be scheduled in the conversation.
/// Only applicable to NON_BLOCKING function calls, is ignored otherwise.
/// Defaults to WHEN_IDLE.
/// </summary>
/// <remarks>
/// Unknown API values are preserved.
/// </remarks>
[JsonConverter(typeof(FunctionResponseScheduling.Converter))]
public readonly struct FunctionResponseScheduling : IEquatable<FunctionResponseScheduling>
{
    private readonly string? _value;

    /// <summary>
    /// Initializes a new instance of the <see cref="FunctionResponseScheduling"/> structure.
    /// </summary>
    /// <param name="value">
    /// The wire value, including values not declared by this type.
    /// </param>
    /// <exception cref="global::System.ArgumentNullException">
    /// <paramref name="value"/> is <see langword="null"/>.
    /// </exception>
    public FunctionResponseScheduling(string value)
    {
        global::System.ArgumentNullException.ThrowIfNull(value);
        _value = value;
    }

    /// <summary>
    /// This value is unused.
    /// </summary>
    public static FunctionResponseScheduling Unspecified { get; } = new("SCHEDULING_UNSPECIFIED");

    /// <summary>
    /// Only add the result to the conversation context, do not interrupt or
    /// trigger generation.
    /// </summary>
    public static FunctionResponseScheduling Silent { get; } = new("SILENT");

    /// <summary>
    /// Add the result to the conversation context, and prompt to generate output
    /// without interrupting ongoing generation.
    /// </summary>
    public static FunctionResponseScheduling WhenIdle { get; } = new("WHEN_IDLE");

    /// <summary>
    /// Add the result to the conversation context, interrupt ongoing generation
    /// and prompt to generate output.
    /// </summary>
    public static FunctionResponseScheduling Interrupt { get; } = new("INTERRUPT");

    /// <summary>
    /// Gets the declared values in schema order.
    /// </summary>
    public static IReadOnlyList<FunctionResponseScheduling> KnownValues { get; } =
    [
        Unspecified,
        Silent,
        WhenIdle,
        Interrupt,
    ];

    /// <summary>
    /// Gets the wire value.
    /// </summary>
    /// <value>
    /// The default instance has the value of <see cref="Unspecified"/>.
    /// </value>
    public string Value => _value ?? "SCHEDULING_UNSPECIFIED";

    /// <summary>
    /// Compares wire values using ordinal equality.
    /// </summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    public static bool operator ==(FunctionResponseScheduling left, FunctionResponseScheduling right) => left.Equals(right);

    /// <summary>
    /// Compares wire values using ordinal inequality.
    /// </summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    public static bool operator !=(FunctionResponseScheduling left, FunctionResponseScheduling right) => !left.Equals(right);

    /// <inheritdoc/>
    public bool Equals(FunctionResponseScheduling other) => string.Equals(Value, other.Value, global::System.StringComparison.Ordinal);

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is FunctionResponseScheduling other && Equals(other);

    /// <inheritdoc/>
    public override int GetHashCode() => global::System.StringComparer.Ordinal.GetHashCode(Value);

    /// <summary>
    /// Returns <see cref="Value"/>.
    /// </summary>
    public override string ToString() => Value;

    [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
    public sealed class Converter : JsonConverter<FunctionResponseScheduling>
    {
        /// <inheritdoc/>
        public override FunctionResponseScheduling Read(ref Utf8JsonReader reader, global::System.Type typeToConvert, JsonSerializerOptions options)
            => new(reader.GetString() ?? throw new JsonException("Expected a string for FunctionResponseScheduling."));

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, FunctionResponseScheduling value, JsonSerializerOptions options)
        {
            global::System.ArgumentNullException.ThrowIfNull(writer);
            writer.WriteStringValue(value.Value);
        }
    }
}


using System.Text.Json;
using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.TunedModels;

/// <summary>
/// Output only. The state of the tuned model.
/// </summary>
/// <remarks>
/// Unknown API values are preserved.
/// </remarks>
[JsonConverter(typeof(TunedModelState.Converter))]
public readonly struct TunedModelState : IEquatable<TunedModelState>
{
    private readonly string? _value;

    /// <summary>
    /// Initializes a new instance of the <see cref="TunedModelState"/> structure.
    /// </summary>
    /// <param name="value">
    /// The wire value, including values not declared by this type.
    /// </param>
    /// <exception cref="global::System.ArgumentNullException">
    /// <paramref name="value"/> is <see langword="null"/>.
    /// </exception>
    public TunedModelState(string value)
    {
        global::System.ArgumentNullException.ThrowIfNull(value);
        _value = value;
    }

    /// <summary>
    /// The default value. This value is unused.
    /// </summary>
    public static TunedModelState Unspecified { get; } = new("STATE_UNSPECIFIED");

    /// <summary>
    /// The model is being created.
    /// </summary>
    public static TunedModelState Creating { get; } = new("CREATING");

    /// <summary>
    /// The model is ready to be used.
    /// </summary>
    public static TunedModelState Active { get; } = new("ACTIVE");

    /// <summary>
    /// The model failed to be created.
    /// </summary>
    public static TunedModelState Failed { get; } = new("FAILED");

    /// <summary>
    /// Gets the declared values in schema order.
    /// </summary>
    public static IReadOnlyList<TunedModelState> KnownValues { get; } =
    [
        Unspecified,
        Creating,
        Active,
        Failed,
    ];

    /// <summary>
    /// Gets the wire value.
    /// </summary>
    /// <value>
    /// The default instance has the value of <see cref="Unspecified"/>.
    /// </value>
    public string Value => _value ?? "STATE_UNSPECIFIED";

    /// <summary>
    /// Compares wire values using ordinal equality.
    /// </summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    public static bool operator ==(TunedModelState left, TunedModelState right) => left.Equals(right);

    /// <summary>
    /// Compares wire values using ordinal inequality.
    /// </summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    public static bool operator !=(TunedModelState left, TunedModelState right) => !left.Equals(right);

    /// <inheritdoc/>
    public bool Equals(TunedModelState other) => string.Equals(Value, other.Value, global::System.StringComparison.Ordinal);

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is TunedModelState other && Equals(other);

    /// <inheritdoc/>
    public override int GetHashCode() => global::System.StringComparer.Ordinal.GetHashCode(Value);

    /// <summary>
    /// Returns <see cref="Value"/>.
    /// </summary>
    public override string ToString() => Value;

    [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
    public sealed class Converter : JsonConverter<TunedModelState>
    {
        /// <inheritdoc/>
        public override TunedModelState Read(ref Utf8JsonReader reader, global::System.Type typeToConvert, JsonSerializerOptions options)
            => new(reader.GetString() ?? throw new JsonException("Expected a string for TunedModelState."));

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, TunedModelState value, JsonSerializerOptions options)
        {
            global::System.ArgumentNullException.ThrowIfNull(writer);
            writer.WriteStringValue(value.Value);
        }
    }
}


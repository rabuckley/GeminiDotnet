using System.Text.Json;
using System.Text.Json.Serialization;

namespace GeminiDotnet.V1;

/// <summary>
/// Optional. Specifies the mode in which function calling should execute. If
/// unspecified, the default value will be set to AUTO.
/// </summary>
/// <remarks>
/// Unknown API values are preserved.
/// </remarks>
[JsonConverter(typeof(FunctionCallingConfigMode.Converter))]
public readonly struct FunctionCallingConfigMode : IEquatable<FunctionCallingConfigMode>
{
    private readonly string? _value;

    /// <summary>
    /// Initializes a new instance of the <see cref="FunctionCallingConfigMode"/> structure.
    /// </summary>
    /// <param name="value">
    /// The wire value, including values not declared by this type.
    /// </param>
    /// <exception cref="global::System.ArgumentNullException">
    /// <paramref name="value"/> is <see langword="null"/>.
    /// </exception>
    public FunctionCallingConfigMode(string value)
    {
        global::System.ArgumentNullException.ThrowIfNull(value);
        _value = value;
    }

    /// <summary>
    /// Unspecified function calling mode. This value should not be used.
    /// </summary>
    public static FunctionCallingConfigMode Unspecified { get; } = new("MODE_UNSPECIFIED");

    /// <summary>
    /// Default model behavior, model decides to predict either a function call
    /// or a natural language response.
    /// </summary>
    public static FunctionCallingConfigMode Auto { get; } = new("AUTO");

    /// <summary>
    /// Model is constrained to always predicting a function call only.
    /// If "allowed_function_names" are set, the predicted function call will be
    /// limited to any one of "allowed_function_names", else the predicted
    /// function call will be any one of the provided "function_declarations".
    /// </summary>
    public static FunctionCallingConfigMode Any { get; } = new("ANY");

    /// <summary>
    /// Model will not predict any function call. Model behavior is same as when
    /// not passing any function declarations.
    /// </summary>
    public static FunctionCallingConfigMode None { get; } = new("NONE");

    /// <summary>
    /// Model decides to predict either a function call
    /// or a natural language response, but will validate function calls with
    /// constrained decoding.
    /// If "allowed_function_names" are set, the predicted function call will be
    /// limited to any one of "allowed_function_names", else the predicted
    /// function call will be any one of the provided "function_declarations".
    /// </summary>
    public static FunctionCallingConfigMode Validated { get; } = new("VALIDATED");

    /// <summary>
    /// Gets the declared values in schema order.
    /// </summary>
    public static IReadOnlyList<FunctionCallingConfigMode> KnownValues { get; } =
    [
        Unspecified,
        Auto,
        Any,
        None,
        Validated,
    ];

    /// <summary>
    /// Gets the wire value.
    /// </summary>
    /// <value>
    /// The default instance has the value of <see cref="Unspecified"/>.
    /// </value>
    public string Value => _value ?? "MODE_UNSPECIFIED";

    /// <summary>
    /// Compares wire values using ordinal equality.
    /// </summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    public static bool operator ==(FunctionCallingConfigMode left, FunctionCallingConfigMode right) => left.Equals(right);

    /// <summary>
    /// Compares wire values using ordinal inequality.
    /// </summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    public static bool operator !=(FunctionCallingConfigMode left, FunctionCallingConfigMode right) => !left.Equals(right);

    /// <inheritdoc/>
    public bool Equals(FunctionCallingConfigMode other) => string.Equals(Value, other.Value, global::System.StringComparison.Ordinal);

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is FunctionCallingConfigMode other && Equals(other);

    /// <inheritdoc/>
    public override int GetHashCode() => global::System.StringComparer.Ordinal.GetHashCode(Value);

    /// <summary>
    /// Returns <see cref="Value"/>.
    /// </summary>
    public override string ToString() => Value;

    [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
    public sealed class Converter : JsonConverter<FunctionCallingConfigMode>
    {
        /// <inheritdoc/>
        public override FunctionCallingConfigMode Read(ref Utf8JsonReader reader, global::System.Type typeToConvert, JsonSerializerOptions options)
            => new(reader.GetString() ?? throw new JsonException("Expected a string for FunctionCallingConfigMode."));

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, FunctionCallingConfigMode value, JsonSerializerOptions options)
        {
            global::System.ArgumentNullException.ThrowIfNull(writer);
            writer.WriteStringValue(value.Value);
        }
    }
}


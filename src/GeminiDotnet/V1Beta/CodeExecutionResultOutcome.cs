using System.Text.Json;
using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta;

/// <summary>
/// Required. Outcome of the code execution.
/// </summary>
/// <remarks>
/// Unknown API values are preserved.
/// </remarks>
[JsonConverter(typeof(CodeExecutionResultOutcome.Converter))]
public readonly struct CodeExecutionResultOutcome : IEquatable<CodeExecutionResultOutcome>
{
    private readonly string? _value;

    /// <summary>
    /// Initializes a new instance of the <see cref="CodeExecutionResultOutcome"/> structure.
    /// </summary>
    /// <param name="value">
    /// The wire value, including values not declared by this type.
    /// </param>
    /// <exception cref="global::System.ArgumentNullException">
    /// <paramref name="value"/> is <see langword="null"/>.
    /// </exception>
    public CodeExecutionResultOutcome(string value)
    {
        global::System.ArgumentNullException.ThrowIfNull(value);
        _value = value;
    }

    /// <summary>
    /// Unspecified status. This value should not be used.
    /// </summary>
    public static CodeExecutionResultOutcome Unspecified { get; } = new("OUTCOME_UNSPECIFIED");

    /// <summary>
    /// Code execution completed successfully. <c>output</c> contains the stdout, if
    /// any.
    /// </summary>
    public static CodeExecutionResultOutcome Ok { get; } = new("OUTCOME_OK");

    /// <summary>
    /// Code execution failed. <c>output</c> contains the stderr and stdout, if any.
    /// </summary>
    public static CodeExecutionResultOutcome Failed { get; } = new("OUTCOME_FAILED");

    /// <summary>
    /// Code execution ran for too long, and was cancelled. There may or may not
    /// be a partial <c>output</c> present.
    /// </summary>
    public static CodeExecutionResultOutcome DeadlineExceeded { get; } = new("OUTCOME_DEADLINE_EXCEEDED");

    /// <summary>
    /// Gets the declared values in schema order.
    /// </summary>
    public static IReadOnlyList<CodeExecutionResultOutcome> KnownValues { get; } =
    [
        Unspecified,
        Ok,
        Failed,
        DeadlineExceeded,
    ];

    /// <summary>
    /// Gets the wire value.
    /// </summary>
    /// <value>
    /// The default instance has the value of <see cref="Unspecified"/>.
    /// </value>
    public string Value => _value ?? "OUTCOME_UNSPECIFIED";

    /// <summary>
    /// Compares wire values using ordinal equality.
    /// </summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    public static bool operator ==(CodeExecutionResultOutcome left, CodeExecutionResultOutcome right) => left.Equals(right);

    /// <summary>
    /// Compares wire values using ordinal inequality.
    /// </summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    public static bool operator !=(CodeExecutionResultOutcome left, CodeExecutionResultOutcome right) => !left.Equals(right);

    /// <inheritdoc/>
    public bool Equals(CodeExecutionResultOutcome other) => string.Equals(Value, other.Value, global::System.StringComparison.Ordinal);

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is CodeExecutionResultOutcome other && Equals(other);

    /// <inheritdoc/>
    public override int GetHashCode() => global::System.StringComparer.Ordinal.GetHashCode(Value);

    /// <summary>
    /// Returns <see cref="Value"/>.
    /// </summary>
    public override string ToString() => Value;

    [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
    public sealed class Converter : JsonConverter<CodeExecutionResultOutcome>
    {
        /// <inheritdoc/>
        public override CodeExecutionResultOutcome Read(ref Utf8JsonReader reader, global::System.Type typeToConvert, JsonSerializerOptions options)
            => new(reader.GetString() ?? throw new JsonException("Expected a string for CodeExecutionResultOutcome."));

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, CodeExecutionResultOutcome value, JsonSerializerOptions options)
        {
            global::System.ArgumentNullException.ThrowIfNull(writer);
            writer.WriteStringValue(value.Value);
        }
    }
}


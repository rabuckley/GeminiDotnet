using System.Text.Json;
using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.Models;

/// <summary>
/// Required. Operator applied to the given key-value pair to trigger the condition.
/// </summary>
/// <remarks>
/// Unknown API values are preserved.
/// </remarks>
[JsonConverter(typeof(ConditionOperation.Converter))]
public readonly struct ConditionOperation : IEquatable<ConditionOperation>
{
    private readonly string? _value;

    /// <summary>
    /// Initializes a new instance of the <see cref="ConditionOperation"/> structure.
    /// </summary>
    /// <param name="value">
    /// The wire value, including values not declared by this type.
    /// </param>
    /// <exception cref="global::System.ArgumentNullException">
    /// <paramref name="value"/> is <see langword="null"/>.
    /// </exception>
    public ConditionOperation(string value)
    {
        global::System.ArgumentNullException.ThrowIfNull(value);
        _value = value;
    }

    /// <summary>
    /// The default value. This value is unused.
    /// </summary>
    public static ConditionOperation OperatorUnspecified { get; } = new("OPERATOR_UNSPECIFIED");

    /// <summary>
    /// Supported by numeric.
    /// </summary>
    public static ConditionOperation Less { get; } = new("LESS");

    /// <summary>
    /// Supported by numeric.
    /// </summary>
    public static ConditionOperation LessEqual { get; } = new("LESS_EQUAL");

    /// <summary>
    /// Supported by numeric &amp; string.
    /// </summary>
    public static ConditionOperation Equal { get; } = new("EQUAL");

    /// <summary>
    /// Supported by numeric.
    /// </summary>
    public static ConditionOperation GreaterEqual { get; } = new("GREATER_EQUAL");

    /// <summary>
    /// Supported by numeric.
    /// </summary>
    public static ConditionOperation Greater { get; } = new("GREATER");

    /// <summary>
    /// Supported by numeric &amp; string.
    /// </summary>
    public static ConditionOperation NotEqual { get; } = new("NOT_EQUAL");

    /// <summary>
    /// Supported by string only when <c>CustomMetadata</c> value type for the given
    /// key has a <c>string_list_value</c>.
    /// </summary>
    public static ConditionOperation Includes { get; } = new("INCLUDES");

    /// <summary>
    /// Supported by string only when <c>CustomMetadata</c> value type for the given
    /// key has a <c>string_list_value</c>.
    /// </summary>
    public static ConditionOperation Excludes { get; } = new("EXCLUDES");

    /// <summary>
    /// Gets the declared values in schema order.
    /// </summary>
    public static IReadOnlyList<ConditionOperation> KnownValues { get; } =
    [
        OperatorUnspecified,
        Less,
        LessEqual,
        Equal,
        GreaterEqual,
        Greater,
        NotEqual,
        Includes,
        Excludes,
    ];

    /// <summary>
    /// Gets the wire value.
    /// </summary>
    /// <value>
    /// The default instance has the value of <see cref="OperatorUnspecified"/>.
    /// </value>
    public string Value => _value ?? "OPERATOR_UNSPECIFIED";

    /// <summary>
    /// Compares wire values using ordinal equality.
    /// </summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    public static bool operator ==(ConditionOperation left, ConditionOperation right) => left.Equals(right);

    /// <summary>
    /// Compares wire values using ordinal inequality.
    /// </summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    public static bool operator !=(ConditionOperation left, ConditionOperation right) => !left.Equals(right);

    /// <inheritdoc/>
    public bool Equals(ConditionOperation other) => string.Equals(Value, other.Value, global::System.StringComparison.Ordinal);

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is ConditionOperation other && Equals(other);

    /// <inheritdoc/>
    public override int GetHashCode() => global::System.StringComparer.Ordinal.GetHashCode(Value);

    /// <summary>
    /// Returns <see cref="Value"/>.
    /// </summary>
    public override string ToString() => Value;

    [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
    public sealed class Converter : JsonConverter<ConditionOperation>
    {
        /// <inheritdoc/>
        public override ConditionOperation Read(ref Utf8JsonReader reader, global::System.Type typeToConvert, JsonSerializerOptions options)
            => new(reader.GetString() ?? throw new JsonException("Expected a string for ConditionOperation."));

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, ConditionOperation value, JsonSerializerOptions options)
        {
            global::System.ArgumentNullException.ThrowIfNull(writer);
            writer.WriteStringValue(value.Value);
        }
    }
}


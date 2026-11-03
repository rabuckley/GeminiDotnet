using System.Text.Json;
using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta;

/// <remarks>
/// Unknown API values are preserved.
/// </remarks>
[JsonConverter(typeof(BatchState.Converter))]
public readonly struct BatchState : IEquatable<BatchState>
{
    private readonly string? _value;

    /// <summary>
    /// Initializes a new instance of the <see cref="BatchState"/> structure.
    /// </summary>
    /// <param name="value">
    /// The wire value, including values not declared by this type.
    /// </param>
    /// <exception cref="global::System.ArgumentNullException">
    /// <paramref name="value"/> is <see langword="null"/>.
    /// </exception>
    public BatchState(string value)
    {
        global::System.ArgumentNullException.ThrowIfNull(value);
        _value = value;
    }

    /// <summary>
    /// The batch state is unspecified.
    /// </summary>
    public static BatchState Unspecified { get; } = new("BATCH_STATE_UNSPECIFIED");

    /// <summary>
    /// The service is preparing to run the batch.
    /// </summary>
    public static BatchState Pending { get; } = new("BATCH_STATE_PENDING");

    /// <summary>
    /// The batch is in progress.
    /// </summary>
    public static BatchState Running { get; } = new("BATCH_STATE_RUNNING");

    /// <summary>
    /// The batch completed successfully.
    /// </summary>
    public static BatchState Succeeded { get; } = new("BATCH_STATE_SUCCEEDED");

    /// <summary>
    /// The batch failed.
    /// </summary>
    public static BatchState Failed { get; } = new("BATCH_STATE_FAILED");

    /// <summary>
    /// The batch has been cancelled.
    /// </summary>
    public static BatchState Cancelled { get; } = new("BATCH_STATE_CANCELLED");

    /// <summary>
    /// The batch has expired.
    /// </summary>
    public static BatchState Expired { get; } = new("BATCH_STATE_EXPIRED");

    /// <summary>
    /// Gets the declared values in schema order.
    /// </summary>
    public static IReadOnlyList<BatchState> KnownValues { get; } =
    [
        Unspecified,
        Pending,
        Running,
        Succeeded,
        Failed,
        Cancelled,
        Expired,
    ];

    /// <summary>
    /// Gets the wire value.
    /// </summary>
    /// <value>
    /// The default instance has the value of <see cref="Unspecified"/>.
    /// </value>
    public string Value => _value ?? "BATCH_STATE_UNSPECIFIED";

    /// <summary>
    /// Compares wire values using ordinal equality.
    /// </summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    public static bool operator ==(BatchState left, BatchState right) => left.Equals(right);

    /// <summary>
    /// Compares wire values using ordinal inequality.
    /// </summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    public static bool operator !=(BatchState left, BatchState right) => !left.Equals(right);

    /// <inheritdoc/>
    public bool Equals(BatchState other) => string.Equals(Value, other.Value, global::System.StringComparison.Ordinal);

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is BatchState other && Equals(other);

    /// <inheritdoc/>
    public override int GetHashCode() => global::System.StringComparer.Ordinal.GetHashCode(Value);

    /// <summary>
    /// Returns <see cref="Value"/>.
    /// </summary>
    public override string ToString() => Value;

    [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
    public sealed class Converter : JsonConverter<BatchState>
    {
        /// <inheritdoc/>
        public override BatchState Read(ref Utf8JsonReader reader, global::System.Type typeToConvert, JsonSerializerOptions options)
            => new(reader.GetString() ?? throw new JsonException("Expected a string for BatchState."));

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, BatchState value, JsonSerializerOptions options)
        {
            global::System.ArgumentNullException.ThrowIfNull(writer);
            writer.WriteStringValue(value.Value);
        }
    }
}


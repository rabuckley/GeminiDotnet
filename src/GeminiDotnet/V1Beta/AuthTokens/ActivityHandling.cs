using System.Text.Json;
using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.AuthTokens;

/// <remarks>
/// Unknown API values are preserved.
/// </remarks>
[JsonConverter(typeof(ActivityHandling.Converter))]
public readonly struct ActivityHandling : IEquatable<ActivityHandling>
{
    private readonly string? _value;

    /// <summary>
    /// Initializes a new instance of the <see cref="ActivityHandling"/> structure.
    /// </summary>
    /// <param name="value">
    /// The wire value, including values not declared by this type.
    /// </param>
    /// <exception cref="global::System.ArgumentNullException">
    /// <paramref name="value"/> is <see langword="null"/>.
    /// </exception>
    public ActivityHandling(string value)
    {
        global::System.ArgumentNullException.ThrowIfNull(value);
        _value = value;
    }

    /// <summary>
    /// If unspecified, the default behavior is <c>START_OF_ACTIVITY_INTERRUPTS</c>.
    /// </summary>
    public static ActivityHandling Unspecified { get; } = new("ACTIVITY_HANDLING_UNSPECIFIED");

    /// <summary>
    /// If true, start of activity will interrupt the model's response (also
    /// called "barge in"). The model's current response will be cut-off in the
    /// moment of the interruption. This is the default behavior.
    /// </summary>
    public static ActivityHandling StartOfActivityInterrupts { get; } = new("START_OF_ACTIVITY_INTERRUPTS");

    /// <summary>
    /// The model's response will not be interrupted.
    /// </summary>
    public static ActivityHandling NoInterruption { get; } = new("NO_INTERRUPTION");

    /// <summary>
    /// Gets the declared values in schema order.
    /// </summary>
    public static IReadOnlyList<ActivityHandling> KnownValues { get; } =
    [
        Unspecified,
        StartOfActivityInterrupts,
        NoInterruption,
    ];

    /// <summary>
    /// Gets the wire value.
    /// </summary>
    /// <value>
    /// The default instance has the value of <see cref="Unspecified"/>.
    /// </value>
    public string Value => _value ?? "ACTIVITY_HANDLING_UNSPECIFIED";

    /// <summary>
    /// Compares wire values using ordinal equality.
    /// </summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    public static bool operator ==(ActivityHandling left, ActivityHandling right) => left.Equals(right);

    /// <summary>
    /// Compares wire values using ordinal inequality.
    /// </summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    public static bool operator !=(ActivityHandling left, ActivityHandling right) => !left.Equals(right);

    /// <inheritdoc/>
    public bool Equals(ActivityHandling other) => string.Equals(Value, other.Value, global::System.StringComparison.Ordinal);

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is ActivityHandling other && Equals(other);

    /// <inheritdoc/>
    public override int GetHashCode() => global::System.StringComparer.Ordinal.GetHashCode(Value);

    /// <summary>
    /// Returns <see cref="Value"/>.
    /// </summary>
    public override string ToString() => Value;

    [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
    public sealed class Converter : JsonConverter<ActivityHandling>
    {
        /// <inheritdoc/>
        public override ActivityHandling Read(ref Utf8JsonReader reader, global::System.Type typeToConvert, JsonSerializerOptions options)
            => new(reader.GetString() ?? throw new JsonException("Expected a string for ActivityHandling."));

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, ActivityHandling value, JsonSerializerOptions options)
        {
            global::System.ArgumentNullException.ThrowIfNull(writer);
            writer.WriteStringValue(value.Value);
        }
    }
}


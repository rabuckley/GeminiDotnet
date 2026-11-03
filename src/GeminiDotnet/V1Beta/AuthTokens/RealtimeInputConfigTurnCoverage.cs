using System.Text.Json;
using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.AuthTokens;

/// <summary>
/// Optional. Defines which input is included in the user's turn.
/// </summary>
/// <remarks>
/// Unknown API values are preserved.
/// </remarks>
[JsonConverter(typeof(RealtimeInputConfigTurnCoverage.Converter))]
public readonly struct RealtimeInputConfigTurnCoverage : IEquatable<RealtimeInputConfigTurnCoverage>
{
    private readonly string? _value;

    /// <summary>
    /// Initializes a new instance of the <see cref="RealtimeInputConfigTurnCoverage"/> structure.
    /// </summary>
    /// <param name="value">
    /// The wire value, including values not declared by this type.
    /// </param>
    /// <exception cref="global::System.ArgumentNullException">
    /// <paramref name="value"/> is <see langword="null"/>.
    /// </exception>
    public RealtimeInputConfigTurnCoverage(string value)
    {
        global::System.ArgumentNullException.ThrowIfNull(value);
        _value = value;
    }

    /// <summary>
    /// If unspecified, a default behavior is selected based on the model. E.g.,
    /// for Gemini 2.5, the default is <c>TURN_INCLUDES_ONLY_ACTIVITY</c>, while for
    /// Gemini 3.1 and onwards, it's
    /// <c>TURN_INCLUDES_AUDIO_ACTIVITY_AND_ALL_VIDEO</c>.
    /// </summary>
    public static RealtimeInputConfigTurnCoverage Unspecified { get; } = new("TURN_COVERAGE_UNSPECIFIED");

    /// <summary>
    /// Includes activity since the last turn, excluding inactivity (e.g. silence
    /// on the audio stream).
    /// </summary>
    public static RealtimeInputConfigTurnCoverage TurnIncludesOnlyActivity { get; } = new("TURN_INCLUDES_ONLY_ACTIVITY");

    /// <summary>
    /// Includes all realtime input since the last turn, including inactivity
    /// (e.g. silence on the audio stream).
    /// </summary>
    public static RealtimeInputConfigTurnCoverage TurnIncludesAllInput { get; } = new("TURN_INCLUDES_ALL_INPUT");

    /// <summary>
    /// Includes audio activity and all video since the last turn. With automatic
    /// activity detection, audio activity means speech and excludes silence.
    /// </summary>
    public static RealtimeInputConfigTurnCoverage TurnIncludesAudioActivityAndAllVideo { get; } = new("TURN_INCLUDES_AUDIO_ACTIVITY_AND_ALL_VIDEO");

    /// <summary>
    /// Gets the declared values in schema order.
    /// </summary>
    public static IReadOnlyList<RealtimeInputConfigTurnCoverage> KnownValues { get; } =
    [
        Unspecified,
        TurnIncludesOnlyActivity,
        TurnIncludesAllInput,
        TurnIncludesAudioActivityAndAllVideo,
    ];

    /// <summary>
    /// Gets the wire value.
    /// </summary>
    /// <value>
    /// The default instance has the value of <see cref="Unspecified"/>.
    /// </value>
    public string Value => _value ?? "TURN_COVERAGE_UNSPECIFIED";

    /// <summary>
    /// Compares wire values using ordinal equality.
    /// </summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    public static bool operator ==(RealtimeInputConfigTurnCoverage left, RealtimeInputConfigTurnCoverage right) => left.Equals(right);

    /// <summary>
    /// Compares wire values using ordinal inequality.
    /// </summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    public static bool operator !=(RealtimeInputConfigTurnCoverage left, RealtimeInputConfigTurnCoverage right) => !left.Equals(right);

    /// <inheritdoc/>
    public bool Equals(RealtimeInputConfigTurnCoverage other) => string.Equals(Value, other.Value, global::System.StringComparison.Ordinal);

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is RealtimeInputConfigTurnCoverage other && Equals(other);

    /// <inheritdoc/>
    public override int GetHashCode() => global::System.StringComparer.Ordinal.GetHashCode(Value);

    /// <summary>
    /// Returns <see cref="Value"/>.
    /// </summary>
    public override string ToString() => Value;

    [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
    public sealed class Converter : JsonConverter<RealtimeInputConfigTurnCoverage>
    {
        /// <inheritdoc/>
        public override RealtimeInputConfigTurnCoverage Read(ref Utf8JsonReader reader, global::System.Type typeToConvert, JsonSerializerOptions options)
            => new(reader.GetString() ?? throw new JsonException("Expected a string for RealtimeInputConfigTurnCoverage."));

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, RealtimeInputConfigTurnCoverage value, JsonSerializerOptions options)
        {
            global::System.ArgumentNullException.ThrowIfNull(writer);
            writer.WriteStringValue(value.Value);
        }
    }
}


using System.Text.Json;
using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta;

/// <summary>
/// Optional. If specified, the media resolution specified will be used.
/// </summary>
/// <remarks>
/// Unknown API values are preserved.
/// </remarks>
[JsonConverter(typeof(GenerationConfigMediaResolution.Converter))]
public readonly struct GenerationConfigMediaResolution : IEquatable<GenerationConfigMediaResolution>
{
    private readonly string? _value;

    /// <summary>
    /// Initializes a new instance of the <see cref="GenerationConfigMediaResolution"/> structure.
    /// </summary>
    /// <param name="value">
    /// The wire value, including values not declared by this type.
    /// </param>
    /// <exception cref="global::System.ArgumentNullException">
    /// <paramref name="value"/> is <see langword="null"/>.
    /// </exception>
    public GenerationConfigMediaResolution(string value)
    {
        global::System.ArgumentNullException.ThrowIfNull(value);
        _value = value;
    }

    /// <summary>
    /// Media resolution has not been set.
    /// </summary>
    public static GenerationConfigMediaResolution Unspecified { get; } = new("MEDIA_RESOLUTION_UNSPECIFIED");

    /// <summary>
    /// Media resolution set to low (64 tokens).
    /// </summary>
    public static GenerationConfigMediaResolution Low { get; } = new("MEDIA_RESOLUTION_LOW");

    /// <summary>
    /// Media resolution set to medium (256 tokens).
    /// </summary>
    public static GenerationConfigMediaResolution Medium { get; } = new("MEDIA_RESOLUTION_MEDIUM");

    /// <summary>
    /// Media resolution set to high (zoomed reframing with 256 tokens).
    /// </summary>
    public static GenerationConfigMediaResolution High { get; } = new("MEDIA_RESOLUTION_HIGH");

    /// <summary>
    /// Gets the declared values in schema order.
    /// </summary>
    public static IReadOnlyList<GenerationConfigMediaResolution> KnownValues { get; } =
    [
        Unspecified,
        Low,
        Medium,
        High,
    ];

    /// <summary>
    /// Gets the wire value.
    /// </summary>
    /// <value>
    /// The default instance has the value of <see cref="Unspecified"/>.
    /// </value>
    public string Value => _value ?? "MEDIA_RESOLUTION_UNSPECIFIED";

    /// <summary>
    /// Compares wire values using ordinal equality.
    /// </summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    public static bool operator ==(GenerationConfigMediaResolution left, GenerationConfigMediaResolution right) => left.Equals(right);

    /// <summary>
    /// Compares wire values using ordinal inequality.
    /// </summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    public static bool operator !=(GenerationConfigMediaResolution left, GenerationConfigMediaResolution right) => !left.Equals(right);

    /// <inheritdoc/>
    public bool Equals(GenerationConfigMediaResolution other) => string.Equals(Value, other.Value, global::System.StringComparison.Ordinal);

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is GenerationConfigMediaResolution other && Equals(other);

    /// <inheritdoc/>
    public override int GetHashCode() => global::System.StringComparer.Ordinal.GetHashCode(Value);

    /// <summary>
    /// Returns <see cref="Value"/>.
    /// </summary>
    public override string ToString() => Value;

    [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
    public sealed class Converter : JsonConverter<GenerationConfigMediaResolution>
    {
        /// <inheritdoc/>
        public override GenerationConfigMediaResolution Read(ref Utf8JsonReader reader, global::System.Type typeToConvert, JsonSerializerOptions options)
            => new(reader.GetString() ?? throw new JsonException("Expected a string for GenerationConfigMediaResolution."));

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, GenerationConfigMediaResolution value, JsonSerializerOptions options)
        {
            global::System.ArgumentNullException.ThrowIfNull(writer);
            writer.WriteStringValue(value.Value);
        }
    }
}


using System.Text.Json;
using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta;

/// <summary>
/// Optional. Content layers to return.
/// If empty, defaults to [FRAMES, AUDIO].
/// </summary>
/// <remarks>
/// Unknown API values are preserved.
/// </remarks>
[JsonConverter(typeof(VideoMetadataContentLayers.Converter))]
public readonly struct VideoMetadataContentLayers : IEquatable<VideoMetadataContentLayers>
{
    private readonly string? _value;

    /// <summary>
    /// Initializes a new instance of the <see cref="VideoMetadataContentLayers"/> structure.
    /// </summary>
    /// <param name="value">
    /// The wire value, including values not declared by this type.
    /// </param>
    /// <exception cref="global::System.ArgumentNullException">
    /// <paramref name="value"/> is <see langword="null"/>.
    /// </exception>
    public VideoMetadataContentLayers(string value)
    {
        global::System.ArgumentNullException.ThrowIfNull(value);
        _value = value;
    }

    public static VideoMetadataContentLayers ContentLayerUnspecified { get; } = new("CONTENT_LAYER_UNSPECIFIED");

    public static VideoMetadataContentLayers Frames { get; } = new("FRAMES");

    public static VideoMetadataContentLayers Audio { get; } = new("AUDIO");

    public static VideoMetadataContentLayers Transcript { get; } = new("TRANSCRIPT");

    /// <summary>
    /// Gets the declared values in schema order.
    /// </summary>
    public static IReadOnlyList<VideoMetadataContentLayers> KnownValues { get; } =
    [
        ContentLayerUnspecified,
        Frames,
        Audio,
        Transcript,
    ];

    /// <summary>
    /// Gets the wire value.
    /// </summary>
    /// <value>
    /// The default instance has the value of <see cref="ContentLayerUnspecified"/>.
    /// </value>
    public string Value => _value ?? "CONTENT_LAYER_UNSPECIFIED";

    /// <summary>
    /// Compares wire values using ordinal equality.
    /// </summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    public static bool operator ==(VideoMetadataContentLayers left, VideoMetadataContentLayers right) => left.Equals(right);

    /// <summary>
    /// Compares wire values using ordinal inequality.
    /// </summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    public static bool operator !=(VideoMetadataContentLayers left, VideoMetadataContentLayers right) => !left.Equals(right);

    /// <inheritdoc/>
    public bool Equals(VideoMetadataContentLayers other) => string.Equals(Value, other.Value, global::System.StringComparison.Ordinal);

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is VideoMetadataContentLayers other && Equals(other);

    /// <inheritdoc/>
    public override int GetHashCode() => global::System.StringComparer.Ordinal.GetHashCode(Value);

    /// <summary>
    /// Returns <see cref="Value"/>.
    /// </summary>
    public override string ToString() => Value;

    [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
    public sealed class Converter : JsonConverter<VideoMetadataContentLayers>
    {
        /// <inheritdoc/>
        public override VideoMetadataContentLayers Read(ref Utf8JsonReader reader, global::System.Type typeToConvert, JsonSerializerOptions options)
            => new(reader.GetString() ?? throw new JsonException("Expected a string for VideoMetadataContentLayers."));

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, VideoMetadataContentLayers value, JsonSerializerOptions options)
        {
            global::System.ArgumentNullException.ThrowIfNull(writer);
            writer.WriteStringValue(value.Value);
        }
    }
}


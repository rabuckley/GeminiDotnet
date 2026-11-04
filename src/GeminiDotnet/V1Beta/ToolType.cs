using System.Text.Json;
using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta;

/// <remarks>
/// Unknown API values are preserved.
/// </remarks>
[JsonConverter(typeof(ToolType.Converter))]
public readonly struct ToolType : IEquatable<ToolType>
{
    private readonly string? _value;

    /// <summary>
    /// Initializes a new instance of the <see cref="ToolType"/> structure.
    /// </summary>
    /// <param name="value">
    /// The wire value, including values not declared by this type.
    /// </param>
    /// <exception cref="global::System.ArgumentNullException">
    /// <paramref name="value"/> is <see langword="null"/>.
    /// </exception>
    public ToolType(string value)
    {
        global::System.ArgumentNullException.ThrowIfNull(value);
        _value = value;
    }

    /// <summary>
    /// Unspecified tool type.
    /// </summary>
    public static ToolType Unspecified { get; } = new("TOOL_TYPE_UNSPECIFIED");

    /// <summary>
    /// Google search tool, maps to Tool.google_search.search_types.web_search.
    /// </summary>
    public static ToolType GoogleSearchWeb { get; } = new("GOOGLE_SEARCH_WEB");

    /// <summary>
    /// Image search tool, maps to Tool.google_search.search_types.image_search.
    /// </summary>
    public static ToolType GoogleSearchImage { get; } = new("GOOGLE_SEARCH_IMAGE");

    /// <summary>
    /// URL context tool, maps to Tool.url_context.
    /// </summary>
    public static ToolType UrlContext { get; } = new("URL_CONTEXT");

    /// <summary>
    /// Google maps tool, maps to Tool.google_maps.
    /// </summary>
    public static ToolType GoogleMaps { get; } = new("GOOGLE_MAPS");

    /// <summary>
    /// File search tool, maps to Tool.file_search.
    /// </summary>
    public static ToolType FileSearch { get; } = new("FILE_SEARCH");

    /// <summary>
    /// Media processing tool types (e.g. video, audio, etc.), maps to invisible
    /// tools used for multimodal understanding like GOOGLE_UNDERSTANDING_VIDEO.
    /// </summary>
    public static ToolType MediaProcessing { get; } = new("MEDIA_PROCESSING");

    /// <summary>
    /// Gets the declared values in schema order.
    /// </summary>
    public static IReadOnlyList<ToolType> KnownValues { get; } =
    [
        Unspecified,
        GoogleSearchWeb,
        GoogleSearchImage,
        UrlContext,
        GoogleMaps,
        FileSearch,
        MediaProcessing,
    ];

    /// <summary>
    /// Gets the wire value.
    /// </summary>
    /// <value>
    /// The default instance has the value of <see cref="Unspecified"/>.
    /// </value>
    public string Value => _value ?? "TOOL_TYPE_UNSPECIFIED";

    /// <summary>
    /// Compares wire values using ordinal equality.
    /// </summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    public static bool operator ==(ToolType left, ToolType right) => left.Equals(right);

    /// <summary>
    /// Compares wire values using ordinal inequality.
    /// </summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    public static bool operator !=(ToolType left, ToolType right) => !left.Equals(right);

    /// <inheritdoc/>
    public bool Equals(ToolType other) => string.Equals(Value, other.Value, global::System.StringComparison.Ordinal);

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is ToolType other && Equals(other);

    /// <inheritdoc/>
    public override int GetHashCode() => global::System.StringComparer.Ordinal.GetHashCode(Value);

    /// <summary>
    /// Returns <see cref="Value"/>.
    /// </summary>
    public override string ToString() => Value;

    [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
    public sealed class Converter : JsonConverter<ToolType>
    {
        /// <inheritdoc/>
        public override ToolType Read(ref Utf8JsonReader reader, global::System.Type typeToConvert, JsonSerializerOptions options)
            => new(reader.GetString() ?? throw new JsonException("Expected a string for ToolType."));

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, ToolType value, JsonSerializerOptions options)
        {
            global::System.ArgumentNullException.ThrowIfNull(writer);
            writer.WriteStringValue(value.Value);
        }
    }
}


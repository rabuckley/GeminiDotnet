using System.Text.Json;
using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta;

/// <remarks>
/// Unknown API values are preserved.
/// </remarks>
[JsonConverter(typeof(GenerativeLanguageModality.Converter))]
public readonly struct GenerativeLanguageModality : IEquatable<GenerativeLanguageModality>
{
    private readonly string? _value;

    /// <summary>
    /// Initializes a new instance of the <see cref="GenerativeLanguageModality"/> structure.
    /// </summary>
    /// <param name="value">
    /// The wire value, including values not declared by this type.
    /// </param>
    /// <exception cref="global::System.ArgumentNullException">
    /// <paramref name="value"/> is <see langword="null"/>.
    /// </exception>
    public GenerativeLanguageModality(string value)
    {
        global::System.ArgumentNullException.ThrowIfNull(value);
        _value = value;
    }

    /// <summary>
    /// Unspecified modality.
    /// </summary>
    public static GenerativeLanguageModality Unspecified { get; } = new("MODALITY_UNSPECIFIED");

    /// <summary>
    /// Plain text.
    /// </summary>
    public static GenerativeLanguageModality Text { get; } = new("TEXT");

    /// <summary>
    /// Image.
    /// </summary>
    public static GenerativeLanguageModality Image { get; } = new("IMAGE");

    /// <summary>
    /// Video.
    /// </summary>
    public static GenerativeLanguageModality Video { get; } = new("VIDEO");

    /// <summary>
    /// Audio.
    /// </summary>
    public static GenerativeLanguageModality Audio { get; } = new("AUDIO");

    /// <summary>
    /// Document, e.g. PDF.
    /// </summary>
    public static GenerativeLanguageModality Document { get; } = new("DOCUMENT");

    /// <summary>
    /// Gets the declared values in schema order.
    /// </summary>
    public static IReadOnlyList<GenerativeLanguageModality> KnownValues { get; } =
    [
        Unspecified,
        Text,
        Image,
        Video,
        Audio,
        Document,
    ];

    /// <summary>
    /// Gets the wire value.
    /// </summary>
    /// <value>
    /// The default instance has the value of <see cref="Unspecified"/>.
    /// </value>
    public string Value => _value ?? "MODALITY_UNSPECIFIED";

    /// <summary>
    /// Compares wire values using ordinal equality.
    /// </summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    public static bool operator ==(GenerativeLanguageModality left, GenerativeLanguageModality right) => left.Equals(right);

    /// <summary>
    /// Compares wire values using ordinal inequality.
    /// </summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    public static bool operator !=(GenerativeLanguageModality left, GenerativeLanguageModality right) => !left.Equals(right);

    /// <inheritdoc/>
    public bool Equals(GenerativeLanguageModality other) => string.Equals(Value, other.Value, global::System.StringComparison.Ordinal);

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is GenerativeLanguageModality other && Equals(other);

    /// <inheritdoc/>
    public override int GetHashCode() => global::System.StringComparer.Ordinal.GetHashCode(Value);

    /// <summary>
    /// Returns <see cref="Value"/>.
    /// </summary>
    public override string ToString() => Value;

    [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
    public sealed class Converter : JsonConverter<GenerativeLanguageModality>
    {
        /// <inheritdoc/>
        public override GenerativeLanguageModality Read(ref Utf8JsonReader reader, global::System.Type typeToConvert, JsonSerializerOptions options)
            => new(reader.GetString() ?? throw new JsonException("Expected a string for GenerativeLanguageModality."));

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, GenerativeLanguageModality value, JsonSerializerOptions options)
        {
            global::System.ArgumentNullException.ThrowIfNull(writer);
            writer.WriteStringValue(value.Value);
        }
    }
}


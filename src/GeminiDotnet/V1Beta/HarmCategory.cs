using System.Text.Json;
using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta;

/// <remarks>
/// Unknown API values are preserved.
/// </remarks>
[JsonConverter(typeof(HarmCategory.Converter))]
public readonly struct HarmCategory : IEquatable<HarmCategory>
{
    private readonly string? _value;

    /// <summary>
    /// Initializes a new instance of the <see cref="HarmCategory"/> structure.
    /// </summary>
    /// <param name="value">
    /// The wire value, including values not declared by this type.
    /// </param>
    /// <exception cref="global::System.ArgumentNullException">
    /// <paramref name="value"/> is <see langword="null"/>.
    /// </exception>
    public HarmCategory(string value)
    {
        global::System.ArgumentNullException.ThrowIfNull(value);
        _value = value;
    }

    /// <summary>
    /// Category is unspecified.
    /// </summary>
    public static HarmCategory Unspecified { get; } = new("HARM_CATEGORY_UNSPECIFIED");

    /// <summary>
    /// **PaLM** - Negative or harmful comments targeting identity and/or protected
    /// attribute.
    /// DEPRECATED: PaLM-only category, not supported by Gemini models.
    /// </summary>
    public static HarmCategory Derogatory { get; } = new("HARM_CATEGORY_DEROGATORY");

    /// <summary>
    /// **PaLM** - Content that is rude, disrespectful, or profane.
    /// DEPRECATED: PaLM-only category, not supported by Gemini models.
    /// </summary>
    public static HarmCategory Toxicity { get; } = new("HARM_CATEGORY_TOXICITY");

    /// <summary>
    /// **PaLM** - Describes scenarios depicting violence against an individual or
    /// group, or general descriptions of gore.
    /// DEPRECATED: PaLM-only category, not supported by Gemini models.
    /// </summary>
    public static HarmCategory Violence { get; } = new("HARM_CATEGORY_VIOLENCE");

    /// <summary>
    /// **PaLM** - Contains references to sexual acts or other lewd content.
    /// DEPRECATED: PaLM-only category, not supported by Gemini models.
    /// </summary>
    public static HarmCategory Sexual { get; } = new("HARM_CATEGORY_SEXUAL");

    /// <summary>
    /// **PaLM** - Promotes unchecked medical advice.
    /// DEPRECATED: PaLM-only category, not supported by Gemini models.
    /// </summary>
    public static HarmCategory Medical { get; } = new("HARM_CATEGORY_MEDICAL");

    /// <summary>
    /// **PaLM** - Dangerous content that promotes, facilitates, or encourages
    /// harmful acts.
    /// DEPRECATED: PaLM-only category, not supported by Gemini models.
    /// </summary>
    public static HarmCategory Dangerous { get; } = new("HARM_CATEGORY_DANGEROUS");

    /// <summary>
    /// **Gemini** - Harassment content.
    /// </summary>
    public static HarmCategory Harassment { get; } = new("HARM_CATEGORY_HARASSMENT");

    /// <summary>
    /// **Gemini** - Hate speech and content.
    /// </summary>
    public static HarmCategory HateSpeech { get; } = new("HARM_CATEGORY_HATE_SPEECH");

    /// <summary>
    /// **Gemini** - Sexually explicit content.
    /// </summary>
    public static HarmCategory SexuallyExplicit { get; } = new("HARM_CATEGORY_SEXUALLY_EXPLICIT");

    /// <summary>
    /// **Gemini** - Dangerous content.
    /// </summary>
    public static HarmCategory DangerousContent { get; } = new("HARM_CATEGORY_DANGEROUS_CONTENT");

    /// <summary>
    /// **Gemini** - Content that may be used to harm civic integrity.
    /// DEPRECATED: use enable_enhanced_civic_answers instead.
    /// </summary>
    public static HarmCategory CivicIntegrity { get; } = new("HARM_CATEGORY_CIVIC_INTEGRITY");

    /// <summary>
    /// **Gemini** - Prompts attempting to bypass or subvert the model's safety
    /// guidelines (jailbreak attempts).
    /// </summary>
    public static HarmCategory Jailbreak { get; } = new("HARM_CATEGORY_JAILBREAK");

    /// <summary>
    /// Gets the declared values in schema order.
    /// </summary>
    public static IReadOnlyList<HarmCategory> KnownValues { get; } =
    [
        Unspecified,
        Derogatory,
        Toxicity,
        Violence,
        Sexual,
        Medical,
        Dangerous,
        Harassment,
        HateSpeech,
        SexuallyExplicit,
        DangerousContent,
        CivicIntegrity,
        Jailbreak,
    ];

    /// <summary>
    /// Gets the wire value.
    /// </summary>
    /// <value>
    /// The default instance has the value of <see cref="Unspecified"/>.
    /// </value>
    public string Value => _value ?? "HARM_CATEGORY_UNSPECIFIED";

    /// <summary>
    /// Compares wire values using ordinal equality.
    /// </summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    public static bool operator ==(HarmCategory left, HarmCategory right) => left.Equals(right);

    /// <summary>
    /// Compares wire values using ordinal inequality.
    /// </summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    public static bool operator !=(HarmCategory left, HarmCategory right) => !left.Equals(right);

    /// <inheritdoc/>
    public bool Equals(HarmCategory other) => string.Equals(Value, other.Value, global::System.StringComparison.Ordinal);

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is HarmCategory other && Equals(other);

    /// <inheritdoc/>
    public override int GetHashCode() => global::System.StringComparer.Ordinal.GetHashCode(Value);

    /// <summary>
    /// Returns <see cref="Value"/>.
    /// </summary>
    public override string ToString() => Value;

    [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
    public sealed class Converter : JsonConverter<HarmCategory>
    {
        /// <inheritdoc/>
        public override HarmCategory Read(ref Utf8JsonReader reader, global::System.Type typeToConvert, JsonSerializerOptions options)
            => new(reader.GetString() ?? throw new JsonException("Expected a string for HarmCategory."));

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, HarmCategory value, JsonSerializerOptions options)
        {
            global::System.ArgumentNullException.ThrowIfNull(writer);
            writer.WriteStringValue(value.Value);
        }
    }
}


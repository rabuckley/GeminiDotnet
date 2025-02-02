using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

using GeminiDotnet.Text.Json;

namespace GeminiDotnet.ContentGeneration;

/// <summary>
/// Supported modalities of the response.
/// </summary>
[JsonConverter(typeof(JsonStringParsableFormattableConverter<Modality>))]
public readonly record struct Modality : IFormattable, IParsable<Modality>
{
    private readonly string _value;

    private Modality(string name)
    {
        _value = name;
    }

    /// <summary>
    /// Default value.
    /// </summary>
    public static Modality Unspecified => new("MODALITY_UNSPECIFIED");

    /// <summary>
    /// Indicates the model should return text.
    /// </summary>
    public static Modality Text => new("TEXT");

    /// <summary>
    /// Indicates the model should return an image.
    /// </summary>
    public static Modality Image => new("IMAGE");

    /// <summary>
    /// Indicates the model should return audio.
    /// </summary>
    public static Modality Audio => new("AUDIO");

    /// <summary>
    /// Indicates the model should return video.
    /// </summary>
    public static Modality Video => new("VIDEO");

    /// <summary>
    /// Indicates the model should return a document.
    /// </summary>
    public static Modality Document => new("DOCUMENT");

    public override string ToString()
    {
        return ToString("G", null);
    }

    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        return _value;
    }

    public static Modality Parse(string s, IFormatProvider? provider)
    {
        ArgumentNullException.ThrowIfNull(s);

        if (TryParse(s, provider, out var result))
        {
            return result;
        }

        throw new FormatException($"The provided value '{s}' is not a valid {nameof(Modality)}.");
    }

    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out Modality result)
    {
        switch (s)
        {
            case "MODALITY_UNSPECIFIED":
                result = Unspecified;
                return true;
            case "TEXT":
                result = Text;
                return true;
            case "IMAGE":
                result = Image;
                return true;
            case "AUDIO":
                result = Audio;
                return true;
            case "VIDEO":
                result = Video;
                return true;
            case "DOCUMENT":
                result = Document;
                return true;
            default:
                result = default;
                return false;
        }
    }
}
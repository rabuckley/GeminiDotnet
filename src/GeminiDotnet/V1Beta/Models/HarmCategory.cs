using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.Models;

[JsonConverter(typeof(JsonStringEnumConverter<HarmCategory>))]
public enum HarmCategory
{
    /// <summary>
    /// Category is unspecified.
    /// </summary>
    [JsonStringEnumMemberName("HARM_CATEGORY_UNSPECIFIED")]
    Unspecified,

    /// <summary>
    /// **PaLM** - Negative or harmful comments targeting identity and/or protected
    /// attribute.
    /// </summary>
    [JsonStringEnumMemberName("HARM_CATEGORY_DEROGATORY")]
    Derogatory,

    /// <summary>
    /// **PaLM** - Content that is rude, disrespectful, or profane.
    /// </summary>
    [JsonStringEnumMemberName("HARM_CATEGORY_TOXICITY")]
    Toxicity,

    /// <summary>
    /// **PaLM** - Describes scenarios depicting violence against an individual or
    /// group, or general descriptions of gore.
    /// </summary>
    [JsonStringEnumMemberName("HARM_CATEGORY_VIOLENCE")]
    Violence,

    /// <summary>
    /// **PaLM** - Contains references to sexual acts or other lewd content.
    /// </summary>
    [JsonStringEnumMemberName("HARM_CATEGORY_SEXUAL")]
    Sexual,

    /// <summary>
    /// **PaLM** - Promotes unchecked medical advice.
    /// </summary>
    [JsonStringEnumMemberName("HARM_CATEGORY_MEDICAL")]
    Medical,

    /// <summary>
    /// **PaLM** - Dangerous content that promotes, facilitates, or encourages
    /// harmful acts.
    /// </summary>
    [JsonStringEnumMemberName("HARM_CATEGORY_DANGEROUS")]
    Dangerous,

    /// <summary>
    /// **Gemini** - Harassment content.
    /// </summary>
    [JsonStringEnumMemberName("HARM_CATEGORY_HARASSMENT")]
    Harassment,

    /// <summary>
    /// **Gemini** - Hate speech and content.
    /// </summary>
    [JsonStringEnumMemberName("HARM_CATEGORY_HATE_SPEECH")]
    HateSpeech,

    /// <summary>
    /// **Gemini** - Sexually explicit content.
    /// </summary>
    [JsonStringEnumMemberName("HARM_CATEGORY_SEXUALLY_EXPLICIT")]
    SexuallyExplicit,

    /// <summary>
    /// **Gemini** - Dangerous content.
    /// </summary>
    [JsonStringEnumMemberName("HARM_CATEGORY_DANGEROUS_CONTENT")]
    DangerousContent,

    /// <summary>
    /// **Gemini** - Content that may be used to harm civic integrity.
    /// DEPRECATED: use enable_enhanced_civic_answers instead.
    /// </summary>
    [JsonStringEnumMemberName("HARM_CATEGORY_CIVIC_INTEGRITY")]
    CivicIntegrity,
}


using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text.Json.Serialization;

using GeminiDotnet.Text.Json;

namespace GeminiDotnet.ContentGeneration;

[JsonConverter(typeof(JsonStringParsableFormattableConverter<FinishReason>))]
public readonly record struct FinishReason : IFormattable, IParsable<FinishReason>
{
    private const string UnknownValue = "FINISH_REASON_UNKNOWN";
    private const string UnspecifiedValue = "FINISH_REASON_UNSPECIFIED";
    private const string StopValue = "STOP";
    private const string MaxTokensValue = "MAX_TOKENS";
    private const string SafetyValue = "SAFETY";
    private const string RecitationValue = "RECITATION";
    private const string OtherValue = "OTHER";

    private readonly string _value;

    private FinishReason(string value)
    {
        _value = value;
    }

    public static readonly FinishReason Unknown = new(UnknownValue);

    public static readonly FinishReason Unspecified = new(UnspecifiedValue);

    /// <summary>
    /// Natural stop point of the model or provided stop sequence.
    /// </summary>
    public static readonly FinishReason Stop = new(StopValue);

    /// <summary>
    /// The maximum number of tokens as specified in the request was reached.
    /// </summary>
    public static readonly FinishReason MaxTokens = new(MaxTokensValue);

    /// <summary>
    /// The token generation was stopped because the response was flagged for safety reasons.
    /// NOTE: When streaming, the candidate's content will be empty if content filters blocked the output.
    /// </summary>
    public static readonly FinishReason Safety = new(SafetyValue);

    /// <summary>
    /// The token generation was stopped because the response was flagged for unauthorized citations.
    /// </summary>
    public static readonly FinishReason Recitation = new(RecitationValue);

    /// <summary>
    /// All other reasons that stopped token generation.
    ///</summary>
    public static readonly FinishReason Other = new(OtherValue);

    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        return _value;
    }

    public bool TryFormat(
        Span<char> destination,
        out int charsWritten,
        ReadOnlySpan<char> format,
        IFormatProvider? provider)
    {
        if (destination.Length < _value.Length)
        {
            charsWritten = 0;
            return false;
        }

        _value.AsSpan().CopyTo(destination);
        charsWritten = _value.Length;
        return true;
    }

    public override string ToString()
    {
        return ToString(null, CultureInfo.InvariantCulture);
    }

    public static FinishReason Parse(string s, IFormatProvider? provider)
    {
        ArgumentNullException.ThrowIfNull(s);

        if (!TryParse(s, provider, out FinishReason result))
        {
            throw new FormatException();
        }

        return result;
    }

    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out FinishReason result)
    {
        if (s is null)
        {
            result = default;
            return false;
        }

        switch (s)
        {
            case UnknownValue:
                result = Unknown;
                return true;
            case UnspecifiedValue:
                result = Unspecified;
                return true;
            case StopValue:
                result = Stop;
                return true;
            case MaxTokensValue:
                result = MaxTokens;
                return true;
            case SafetyValue:
                result = Safety;
                return true;
            case RecitationValue:
                result = Recitation;
                return true;
            case OtherValue:
                result = Other;
                return true;
            default:
                result = default;
                return false;
        }
    }
}
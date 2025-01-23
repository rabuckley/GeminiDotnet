using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

using GeminiDotnet.Text.Json;

namespace GeminiDotnet.ContentGeneration.FunctionCalling;

[JsonConverter(typeof(JsonStringParsableFormattableConverter<CodeExecutionOutcome>))]
public readonly struct CodeExecutionOutcome : IParsable<CodeExecutionOutcome>, IFormattable
{
    private const string UnknownValue = "OUTCOME_UNKNOWN";
    private const string UnspecifiedValue = "OUTCOME_UNSPECIFIED";
    private const string OkValue = "OUTCOME_OK";
    private const string FailedValue = "OUTCOME_FAILED";
    private const string DeadlineExceededValue = "OUTCOME_DEADLINE_EXCEEDED";

    private readonly string _value;

    private CodeExecutionOutcome(string value)
    {
        _value = value;
    }

    public static readonly CodeExecutionOutcome Unknown = new(UnknownValue);

    public static readonly CodeExecutionOutcome Unspecified = new(UnspecifiedValue);

    public static readonly CodeExecutionOutcome Ok = new(OkValue);

    public static readonly CodeExecutionOutcome Failed = new(FailedValue);

    public static readonly CodeExecutionOutcome DeadlineExceeded = new(DeadlineExceededValue);

    public override string ToString() => ToString(null, null);

    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        return _value;
    }

    public static CodeExecutionOutcome Parse(string s, IFormatProvider? provider)
    {
        ArgumentNullException.ThrowIfNull(s);

        if (!TryParse(s, provider, out var result))
        {
            throw new FormatException();
        }

        return result;
    }

    public static bool TryParse(
        [NotNullWhen(true)] string? s,
        IFormatProvider? provider,
        out CodeExecutionOutcome result)
    {
        if (s is null)
        {
            result = default;
            return false;
        }

        if (s == UnknownValue)
        {
            result = Unknown;
            return true;
        }

        if (s == UnspecifiedValue)
        {
            result = Unspecified;
            return true;
        }

        if (s == OkValue)
        {
            result = Ok;
            return true;
        }

        if (s == FailedValue)
        {
            result = Failed;
            return true;
        }

        if (s == DeadlineExceededValue)
        {
            result = DeadlineExceeded;
            return true;
        }

        result = default;
        return false;
    }
}
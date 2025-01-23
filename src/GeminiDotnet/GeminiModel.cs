using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace GeminiDotnet;

public readonly record struct GeminiModel : ISpanFormattable, IParsable<GeminiModel>
{
    // https://ai.google.dev/gemini-api/docs/models/gemini

    private const string TextEmbedding004Id = "text-embedding-004";

    private const string Gemini1p5ProId = "gemini-1.5-pro";
    private const string Gemini1p5Flash8bId = "gemini-1.5-flash-8b";
    private const string Gemini1p5FlashId = "gemini-1.5-flash";
    private const string Gemini2FlashId = "gemini-2.0-flash-exp";

    private readonly string _id;

    private GeminiModel(string id)
    {
        _id = id;
    }

    /// <summary>
    /// The <c>text-embedding-004</c> model.
    /// </summary>
    public static GeminiModel TextEmbedding004 { get; } = new(TextEmbedding004Id);

    private static readonly FrozenSet<GeminiModel> s_textEmbeddingModels = [TextEmbedding004];

    /// <summary>
    /// The set of text embedding models.
    /// </summary>
    public static IReadOnlySet<GeminiModel> TextEmbeddingModels => s_textEmbeddingModels;

    /// <summary>
    /// The <c>gemini-1.5-pro</c> model.
    /// </summary>
    public static GeminiModel Gemini1p5Pro { get; } = new(Gemini1p5ProId);

    /// <summary>
    /// The <c>gemini-1.5-flash</c> model.
    /// </summary>
    public static GeminiModel Gemini1p5Flash { get; } = new(Gemini1p5FlashId);

    /// <summary>
    /// The <c>gemini-1.5-flash-8b</c> model.
    /// </summary>
    public static GeminiModel Gemini1p5Flash8b { get; } = new(Gemini1p5Flash8bId);

    /// <summary>
    /// The <c>gemini-2.0-flash-exp</c> model.
    /// </summary>
    public static GeminiModel Gemini2Flash { get; } = new(Gemini2FlashId);

    private static readonly FrozenSet<GeminiModel> s_chatModels = [Gemini1p5Pro, Gemini1p5Flash];

    /// <summary>
    /// The set of chat models.
    /// </summary>
    public static IReadOnlySet<GeminiModel> ChatModels => s_chatModels;

    private static readonly FrozenDictionary<string, GeminiModel> s_allModels = new Dictionary<string, GeminiModel>
    {
        [TextEmbedding004Id] = TextEmbedding004,
        [Gemini1p5ProId] = Gemini1p5Pro,
        [Gemini1p5FlashId] = Gemini1p5Flash,
        [Gemini1p5Flash8bId] = Gemini1p5Flash8b,
        [Gemini2FlashId] = Gemini2Flash,
    }.ToFrozenDictionary();

    public override string ToString()
    {
        return ToString(null, CultureInfo.InvariantCulture);
    }

    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        return _id;
    }

    public bool TryFormat(
        Span<char> destination,
        out int charsWritten,
        ReadOnlySpan<char> format,
        IFormatProvider? provider)
    {
        if (destination.Length < _id.Length)
        {
            charsWritten = 0;
            return false;
        }

        _id.AsSpan().CopyTo(destination);

        charsWritten = _id.Length;
        return true;
    }

    public static GeminiModel Parse(string s, IFormatProvider? provider)
    {
        if (!TryParse(s, provider, out var result))
        {
            throw new FormatException();
        }

        return result;
    }

    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out GeminiModel result)
    {
        if (s is null)
        {
            result = default;
            return false;
        }

        if (s_allModels.TryGetValue(s, out result))
        {
            return true;
        }

        result = default;
        return false;
    }
}
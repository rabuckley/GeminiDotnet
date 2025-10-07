namespace GeminiDotnet;

[Obsolete("GeminiModels will be removed in a future release. Please pass model names directly as strings.")]
public static class GeminiModels
{
    public const string TextEmbedding004 = "text-embedding-004";

    public const string Gemini1p5Pro = "gemini-1.5-pro";

    public const string Gemini1p5Flash = "gemini-1.5-flash";

    public const string Gemini1p5Flash8b = "gemini-1.5-flash-8b";

    public const string Gemini2Flash = "gemini-2.0-flash";

    public const string Gemini2FlashLite = "gemini-2.0-flash-lite";

    public static class Experimental
    {
        public const string Gemini2FlashThinking = "gemini-2.0-flash-thinking-exp";

        public const string Gemini2p5Pro = "gemini-2.5-pro-exp-03-25";
        
        public const string Gemini2p5FlashPreview = "gemini-2.5-flash-preview-05-20";
        
        public const string Gemini2p5ProPreview = "gemini-2.5-pro-preview-05-06";
    }
}

namespace GeminiDotnet;

public sealed record GeminiClientOptions : IGeminiClientOptions
{
    public required string ApiKey { get; set; }

    /// <summary>
    /// The model id to use unless overridden in by the caller.
    /// </summary>
    public string? ModelId { get; set; }

    public Uri? Endpoint { get; set; } = new Uri("https://generativelanguage.googleapis.com", UriKind.Absolute);
    
    public int? DefaultEmbeddingDimensions { get; set; }
}

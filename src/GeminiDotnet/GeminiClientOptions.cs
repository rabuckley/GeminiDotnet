namespace GeminiDotnet;

public sealed record GeminiClientOptions
{
    public required string ApiKey { get; set; }

    public string ApiVersion { get; set; } = GeminiApiVersions.V1;

    /// <summary>
    /// The model id to use unless overridden in by the caller.
    /// </summary>
    public string? ModelId { get; set; }

    /// <summary>
    /// The request timeout to use.
    /// </summary>
    /// <remarks>
    /// This has no effect if you pass a <see cref="HttpClient"/> in yourself.
    /// </remarks>
    public TimeSpan? RequestTimeout { get; set; }
}

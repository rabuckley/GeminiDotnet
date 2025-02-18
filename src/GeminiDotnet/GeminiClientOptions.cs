namespace GeminiDotnet;

public sealed record GeminiClientOptions
{
    public required string ApiKey { get; set; }

    public string ApiVersion { get; set; } = GeminiApiVersions.V1;

    /// <summary>
    /// The model id to use unless overridden in by the caller.
    /// </summary>
    public string? ModelId { get; set; }
}

namespace GeminiDotnet.ContentGeneration;

public sealed record TextContentPart : ContentPart
{
    public required string Text { get; init; }
}
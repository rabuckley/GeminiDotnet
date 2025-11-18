using GeminiDotnet.V1Beta;

namespace GeminiDotnet.Extensions.AI.Examples;

public sealed class ImageGenerationExample
{
    public static async Task ExecuteAsync(GeminiChatClient chatClient, CancellationToken cancellationToken)
    {
        var client = chatClient.GetService(typeof(IGeminiClient)) as IGeminiClient
            ?? throw new InvalidOperationException("Failed to get IGeminiClient from GeminiChatClient.");

        const string model = "gemini-2.5-flash-image";

        var request = new GenerateContentRequest
        {
            Model = model,
            Contents =
            [
                new Content
                {
                    Role = ChatRoles.User,
                    Parts = [new Part { Text = "Generate an image of a futuristic cityscape at sunset." }]
                }
            ],
        };

        var response = await client.V1Beta.Models.GenerateContentAsync(model, request, cancellationToken);

        if (response.Candidates?.Count != 1)
        {
            Console.WriteLine($"Unexpected number of candidates: {response.Candidates?.Count}");
            return;
        }
        
        var candidate = response.Candidates?[0];

        foreach (var part in candidate!.Content!.Parts!)
        {
            if (part.FileData is not null)
            {
                Console.WriteLine($"Image data: {part.FileData.FileUri}");
                continue;
            }

            if (part.InlineData is not null)
            {
                var outputFile = Path.Combine(Directory.GetCurrentDirectory(), "output_image.png");
                await using var stream = File.OpenWrite(outputFile);
                await stream.WriteAsync(part.InlineData.Data, cancellationToken);
                Console.WriteLine($"Image saved to: {outputFile}");
                continue;
            }

            if (part.Text is not null)
            {
                Console.WriteLine($"Text data: {part.Text}");
                continue;
            }

            Console.WriteLine(part);
        }
    }
}

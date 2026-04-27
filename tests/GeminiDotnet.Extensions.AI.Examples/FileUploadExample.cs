using Microsoft.Extensions.AI;

#pragma warning disable MEAI001 // Type is for evaluation purposes only

namespace GeminiDotnet.Extensions.AI.Examples;

public sealed class FileUploadExample
{
    public static async Task ExecuteAsync(GeminiChatClient chatClient, CancellationToken cancellationToken)
    {
        var geminiClient = chatClient.GetService(typeof(IGeminiClient)) as IGeminiClient
            ?? throw new InvalidOperationException("Failed to get IGeminiClient from GeminiChatClient.");

        // Use the portable IHostedFileClient abstraction for file management
        // instead of the Gemini-specific IFilesClient directly.
        using var fileClient = geminiClient.AsHostedFileClient();

        // Create a small CSV file in-memory to upload.
        var csvContent = """
            product,units_sold,revenue
            Widget A,150,4500.00
            Widget B,230,6900.00
            Widget C,80,2400.00
            """u8;

        using var stream = new MemoryStream(csvContent.ToArray());

        Console.WriteLine("Uploading CSV file...");

        var uploadedFile = await fileClient.UploadAsync(
            stream,
            mediaType: "text/csv",
            fileName: "sales-data.csv",
            cancellationToken: cancellationToken);

        Console.WriteLine($"Uploaded: {uploadedFile.FileId}");

        // Use the uploaded file in a chat request via HostedFileContent, which maps
        // to Gemini's FileData part type.
        try
        {
            List<ChatMessage> messages =
            [
                new(ChatRole.User,
                [
                    uploadedFile,
                    new TextContent("Which product had the highest revenue? Reply in one sentence."),
                ]),
            ];

            Console.WriteLine("\nAsking model about the uploaded file...\n");

            await foreach (var update in chatClient.GetStreamingResponseAsync(messages, cancellationToken: cancellationToken))
            {
                Console.Write(update.Text);
            }

            Console.WriteLine();
        }
        finally
        {
            Console.WriteLine($"\nDeleting uploaded file...");

            await fileClient.DeleteAsync(uploadedFile.FileId, cancellationToken: cancellationToken);

            Console.WriteLine("File deleted.");
        }
    }
}

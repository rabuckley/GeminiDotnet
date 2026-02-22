using GeminiDotnet.V1Beta;
using Microsoft.Extensions.AI;

namespace GeminiDotnet.Extensions.AI.Examples;

public sealed class FileUploadExample
{
    public static async Task ExecuteAsync(GeminiChatClient chatClient, CancellationToken cancellationToken)
    {
        var geminiClient = chatClient.GetService(typeof(IGeminiClient)) as IGeminiClient
            ?? throw new InvalidOperationException("Failed to get IGeminiClient from GeminiChatClient.");

        var filesClient = geminiClient.V1Beta.Files;

        // Create a small CSV file in-memory to upload.
        var csvContent = """
            product,units_sold,revenue
            Widget A,150,4500.00
            Widget B,230,6900.00
            Widget C,80,2400.00
            """u8;

        using var stream = new MemoryStream(csvContent.ToArray());

        Console.WriteLine("Uploading CSV file...");

        var uploadedFile = await filesClient.UploadFileAsync(
            stream,
            csvContent.Length,
            new UploadFileOptions { DisplayName = "sales-data.csv", MimeType = "text/csv" },
            cancellationToken);

        Console.WriteLine($"Uploaded: {uploadedFile.Name} (state: {uploadedFile.State})");

        // Use the uploaded file in a chat request via HostedFileContent, which maps
        // to Gemini's FileData part type.
        try
        {
            List<ChatMessage> messages =
            [
                new(ChatRole.User,
                [
                    new HostedFileContent(uploadedFile.Uri!) { MediaType = "text/csv" },
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
            // Always delete the uploaded file to avoid leaving resources on the
            // caller's account. The Name is in "files/{id}" format — DeleteFileAsync
            // expects just the resource ID segment.
            var fileId = uploadedFile.Name!.Replace("files/", "");

            Console.WriteLine($"\nDeleting uploaded file ({fileId})...");

            await filesClient.DeleteFileAsync(fileId, cancellationToken);

            Console.WriteLine("File deleted.");
        }
    }
}

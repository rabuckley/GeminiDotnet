using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

using GeminiDotnet.ContentGeneration;
using GeminiDotnet.Text.Json;

namespace GeminiDotnet;

public sealed class GenerateContentRequestTests
{
    [Fact]
    public void Deserialize_Example()
    {
        // Act
        var request = JsonSerializer.Deserialize(
            ChatExample1Json,
            JsonContext.Default.GetTypeInfo<GenerateContentRequest>());

        // Assert
        Assert.NotNull(request);
        Assert.Equal(3, request.Contents.Count());
    }

    [Fact]
    public void JsonRoundtrip_Text()
    {
        var request = new GenerateContentRequest
        {
            Contents =
            [
                new ChatMessage
                {
                    Role = ChatRole.User,
                    Parts = new List<ContentPart> { new TextContentPart { Text = "Hello, world!" } }
                }
            ]
        };

        var json = JsonSerializer.Serialize(request);
        var deserialized = JsonSerializer.Deserialize<GenerateContentRequest>(json);

        Assert.Equivalent(request, deserialized);
    }

    [StringSyntax(StringSyntaxAttribute.Json)]
    const string ChatExample1Json =
        """
        {
          "contents": [
            {
              "role": "user",
              "parts": [
                {
                  "text": "Hello"
                }
              ]
            },
            {
              "role": "model",
              "parts": [
                {
                  "text": "Great to meet you. What would you like to know?"
                }
              ]
            },
            {
              "role": "user",
              "parts": [
                {
                  "text": "I have two dogs in my house. How many paws are in my house?"
                }
              ]
            }
          ]
        }
        """;
}
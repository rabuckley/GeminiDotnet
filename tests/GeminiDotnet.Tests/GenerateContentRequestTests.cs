using GeminiDotnet.Text.Json;
using GeminiDotnet.V1Beta;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace GeminiDotnet;

public sealed class GenerateContentRequestTests
{
    [Theory]
    [MemberData(nameof(Examples))]
    public void Deserialize_Examples(string example)
    {
        // Act
        var request = JsonSerializer.Deserialize(
            example,
            V1BetaJsonContext.Default.GetTypeInfo<GenerateContentRequest>());

        // Assert
        Assert.NotNull(request);
        Assert.Equal(3, request.Contents.Count);
    }

    [Fact]
    public void JsonRoundtrip_WithTextPart()
    {
        var request = new GenerateContentRequest
        {
            Model = "models/gemini-1.5-pro",
            Contents =
            [
                new Content { Role = ChatRoles.User, Parts = new List<Part> { new() { Text = "Hello, world!" } } }
            ]
        };

        var json = JsonSerializer.Serialize(request);
        var deserialized = JsonSerializer.Deserialize<GenerateContentRequest>(json);

        Assert.Equivalent(request, deserialized);
    }

    public static IEnumerable<TheoryDataRow<string>> Examples()
    {
        yield return ChatExample1Json;
    }

    [StringSyntax(StringSyntaxAttribute.Json)]
    const string ChatExample1Json =
        """
        {
          "model": "models/gemini-1.5-pro",
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

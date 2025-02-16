using GeminiDotnet.ContentGeneration;
using Microsoft.Extensions.AI;

namespace GeminiDotnet.Extensions.AI;

public sealed class MEAIToGeminiMapperTests
{
    [Fact]
    public void CreateMappedGenerateRequest_WithSystemRole_ShouldPopulateSystemInstruction()
    {
        // Arrange
        List<ChatMessage> messages =
        [
            new() { Role = ChatRole.System, Text = "You are Neko the cat. Respond like one." },
            new() { Role = ChatRole.User, Text = "Hello cat!" },
            new() { Role = ChatRole.Assistant, Text = "Meow!" },
            new() { Role = ChatRole.User, Text = "What is your name? What do like to drink?" }
        ];

        // Act
        var request = MEAIToGeminiMapper.CreateMappedGenerateContentRequest(messages);

        // Assert
        Assert.NotNull(request);
        Assert.NotNull(request.SystemInstruction);
        Assert.Null(request.SystemInstruction.Role);
        var part = Assert.Single(request.SystemInstruction.Parts);
        Assert.Equal("You are Neko the cat. Respond like one.", part.Text);

        for (int i = 1; i < messages.Count; i++)
        {
            var message = messages[i];
            var content = request.Contents.ElementAt(i - 1);
            var p = Assert.Single(content.Parts);

            Assert.Equal(message.Text, p.Text);

            if (message.Role == ChatRole.User)
            {
                Assert.Equal(ChatRoles.User, content.Role);
            }
            else if (message.Role == ChatRole.Assistant)
            {
                Assert.Equal(ChatRoles.Model, content.Role);
            }
        }
    }
}

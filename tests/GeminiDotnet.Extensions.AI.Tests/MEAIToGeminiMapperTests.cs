using GeminiDotnet.ContentGeneration;
using Microsoft.Extensions.AI;
using System.Net.Mime;
using System.Text.Json;
using System.Text.Json.Serialization;

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
        var request = MEAIToGeminiMapper.CreateMappedGenerateContentRequest(messages, new ChatOptions());

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

    [Fact]
    public void CreateMappedGenerateContentRequest_WithChatOptions_ShouldMapOptions()
    {
        // Arrange
        var messages = new List<ChatMessage>
        {
            new() { Role = ChatRole.User, Text = "Who was the first person to walk on the moon?" }
        };

        var options = new ChatOptions
        {
            Temperature = 0.42f,
            MaxOutputTokens = 1234,
            TopP = 42,
            TopK = 24,
            FrequencyPenalty = 12,
            PresencePenalty = 254,
            Seed = 3,
            ResponseFormat = null,
            ModelId = null,
            StopSequences = ["please_stop!"],
            ToolMode = ChatToolMode.Auto,
            Tools = null,
            AdditionalProperties = null,
        };

        // Act
        var request = MEAIToGeminiMapper.CreateMappedGenerateContentRequest(messages, options);

        // Assert
        Assert.Equal(options.Temperature, request.GenerationConfiguration?.Temperature);
        Assert.Equal(options.MaxOutputTokens, request.GenerationConfiguration?.MaxOutputTokens);
        Assert.Equal(options.TopP, request.GenerationConfiguration?.TopP);
        Assert.Equal(options.TopK, request.GenerationConfiguration?.TopK);
        Assert.Equal(options.FrequencyPenalty, request.GenerationConfiguration?.FrequencyPenalty);
        Assert.Equal(options.PresencePenalty, request.GenerationConfiguration?.PresencePenalty);
        Assert.Equal(options.Seed, request.GenerationConfiguration?.Seed);
        Assert.Equal(options.StopSequences, request.GenerationConfiguration?.StopSequences);
    }

    [Fact]
    public void CreateMappedGenerateContentRequest_WithJsonSchema_ShouldMapSchema()
    {
        // Arrange
        var messages = new List<ChatMessage>
        {
            new() { Role = ChatRole.User, Text = "Who was the first person to walk on the moon?" }
        };

        var schema = AIJsonUtilities.CreateJsonSchema(typeof(TestObject),
            inferenceOptions: new AIJsonSchemaCreateOptions
            {
                TransformSchemaNode = null,
                IncludeTypeInEnumSchemas = false,
                DisallowAdditionalProperties = true,
                IncludeSchemaKeyword = false,
                RequireAllProperties = false,
            });

        var options = new ChatOptions { ResponseFormat = ChatResponseFormat.ForJsonSchema(schema) };

        // Act
        var request = MEAIToGeminiMapper.CreateMappedGenerateContentRequest(messages, options);

        // Assert
        Assert.Equal(MediaTypeNames.Application.Json, request.GenerationConfiguration?.ResponseMimeType);

        var actual = Assert.IsType<ObjectSchema>(request.GenerationConfiguration?.ResponseSchema);
        Assert.NotNull(actual.RequiredProperties);
        Assert.Contains("name", actual.RequiredProperties);

        if (actual.Properties?.TryGetValue("name", out var name) is not true)
        {
            Assert.Fail("Expected 'name' property in schema.");
            return;
        }

        var nameSchema = Assert.IsType<StringSchema>(name);
        Assert.Null(nameSchema.Nullable);
    }

    [Fact]
    public void CreateMappedGenerateContentRequest_WithCodeInterpreterTool_ShouldIncludeCodeExecutionTool()
    {
        // Arrange
        var messages = new List<ChatMessage>
        {
            new() { Role = ChatRole.User, Text = "Who was the first person to walk on the moon?" }
        };

        var options = new ChatOptions { Tools = [new CodeInterpreterTool()] };

        // Act
        var request = MEAIToGeminiMapper.CreateMappedGenerateContentRequest(messages, options);

        // Assert
        Assert.NotNull(request.Tools);
        Assert.Single(request.Tools, t => t.CodeExecution is not null);
    }

    [Fact]
    public void CreateMappedGenerateContentRequest_WithAIFunction_ShouldIncludeFunctionDeclaration()
    {
        // Arrange
        var messages = new List<ChatMessage>
        {
            new() { Role = ChatRole.User, Text = "Who was the first person to walk on the moon?" }
        };

        var expectedFunction = new TestFunction();
        var options = new ChatOptions { Tools = [expectedFunction] };

        // Act
        var request = MEAIToGeminiMapper.CreateMappedGenerateContentRequest(messages, options);

        // Assert
        Assert.NotNull(request.Tools);
        var tool = Assert.Single(request.Tools, t => t.FunctionDeclarations is not null);
        var functionDeclaration = Assert.Single(tool.FunctionDeclarations!);

        Assert.Equal(expectedFunction.Name, functionDeclaration.Name);
        Assert.Equal(expectedFunction.Description, functionDeclaration.Description);

        // Tricky to compare the schema directly, so just check the type for now.
        Assert.Equal(Schema.FromJsonElement(expectedFunction.JsonSchema).GetType(),
            functionDeclaration.Schema?.GetType());
    }

    private sealed class TestFunction : AIFunction
    {
        public override JsonElement JsonSchema { get; } = AIJsonUtilities.CreateJsonSchema(typeof(TestObject));

        protected override Task<object?> InvokeCoreAsync(
            IEnumerable<KeyValuePair<string, object?>> arguments,
            CancellationToken cancellationToken)
        {
            return Task.FromResult<object?>(null);
        }
    }

    private sealed record TestObject
    {
        [JsonPropertyName("name")]
        public required string Name { get; init; }
    }
}

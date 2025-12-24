using GeminiDotnet.V1Beta;
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
            new(ChatRole.System, "You are Neko the cat. Respond like one."),
            new(ChatRole.User, "Hello cat!"),
            new(ChatRole.Assistant, "Meow!"),
            new(ChatRole.User, "What is your name? What do like to drink?")
        ];

        // Act
        var request = MEAIToGeminiMapper.CreateMappedGenerateContentRequest("model", messages, new ChatOptions());

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
            new(ChatRole.User, "Who was the first person to walk on the moon?")
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
        var request = MEAIToGeminiMapper.CreateMappedGenerateContentRequest("", messages, options);

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
            new(ChatRole.User, "Who was the first person to walk on the moon?")
        };

        var schema = AIJsonUtilities.CreateJsonSchema(typeof(TestObject),
            inferenceOptions: new AIJsonSchemaCreateOptions
            {
                TransformSchemaNode = null,
                IncludeSchemaKeyword = false,
                TransformOptions = new()
                {
                    DisallowAdditionalProperties = true,
                    RequireAllProperties = false,
                },
            });

        var options = new ChatOptions { ResponseFormat = ChatResponseFormat.ForJsonSchema(schema) };

        // Act
        var request = MEAIToGeminiMapper.CreateMappedGenerateContentRequest("", messages, options);

        // Assert
        Assert.Equal(MediaTypeNames.Application.Json, request.GenerationConfiguration?.ResponseMimeType);
        Assert.Equal(schema, request.GenerationConfiguration?.ResponseJsonSchema);
    }

    [Fact]
    public void CreateMappedGenerateContentRequest_WithCodeInterpreterTool_ShouldIncludeCodeExecutionTool()
    {
        // Arrange
        var messages = new List<ChatMessage>
        {
            new(ChatRole.User, "Who was the first person to walk on the moon?")
        };

        var options = new ChatOptions { Tools = [new HostedCodeInterpreterTool()] };

        // Act
        var request = MEAIToGeminiMapper.CreateMappedGenerateContentRequest("", messages, options);

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
            new(ChatRole.User, "Who was the first person to walk on the moon?")
        };

        var expectedFunction = new TestFunction();
        var options = new ChatOptions { Tools = [expectedFunction] };

        // Act
        var request = MEAIToGeminiMapper.CreateMappedGenerateContentRequest("", messages, options);

        // Assert
        Assert.NotNull(request.Tools);
        var tool = Assert.Single(request.Tools, t => t.FunctionDeclarations is not null);
        var functionDeclaration = Assert.Single(tool.FunctionDeclarations!);

        Assert.Equal(expectedFunction.Name, functionDeclaration.Name);
        Assert.Equal(expectedFunction.Description, functionDeclaration.Description);
        Assert.Equal(expectedFunction.JsonSchema, functionDeclaration.ParametersJsonSchema);
    }

    [Fact]
    public void CreateMappedGenerateContentRequest_WithThinkingConfiguration_ShouldMapThinkingConfig()
    {
        // Arrange
        var messages = new List<ChatMessage>
        {
            new(ChatRole.User, "Who was the first person to walk on the moon?")
        };

        var thinkingConfig = new ThinkingConfiguration
        {
            IncludeThoughts = true,
            ThinkingBudget = 1000
        };

        var options = new ChatOptions
        {
            AdditionalProperties = new AdditionalPropertiesDictionary
            {
                ["thinkingConfig"] = thinkingConfig
            }
        };

        // Act
        var request = MEAIToGeminiMapper.CreateMappedGenerateContentRequest("", messages, options);

        // Assert
        Assert.Equal(thinkingConfig, request.GenerationConfiguration?.ThinkingConfiguration);
    }

    [Fact]
    public void CreateMappedGenerateContentRequest_WithThinkingConfigurationAsJsonElement_ShouldMapThinkingConfig()
    {
        // Arrange - This test simulates what happens when Semantic Kernel's
        // PromptExecutionSettings.ToChatOptions() roundtrips settings through JSON serialization,
        // causing typed objects to become JsonElements in AdditionalProperties.
        var messages = new List<ChatMessage>
        {
            new(ChatRole.User, "Who was the first person to walk on the moon?")
        };

        var thinkingConfig = new ThinkingConfiguration
        {
            IncludeThoughts = true,
            ThinkingBudget = 1000
        };

        // Serialize to JsonElement to simulate SK's JSON roundtrip behavior
        var thinkingConfigJson = JsonSerializer.SerializeToElement(thinkingConfig);

        var options = new ChatOptions
        {
            AdditionalProperties = new AdditionalPropertiesDictionary
            {
                ["thinkingConfig"] = thinkingConfigJson // JsonElement instead of ThinkingConfiguration
            }
        };

        // Act
        var request = MEAIToGeminiMapper.CreateMappedGenerateContentRequest("", messages, options);

        // Assert
        Assert.NotNull(request.GenerationConfiguration?.ThinkingConfiguration);
        Assert.True(request.GenerationConfiguration?.ThinkingConfiguration?.IncludeThoughts);
        Assert.Equal(1000, request.GenerationConfiguration?.ThinkingConfiguration?.ThinkingBudget);
    }

    [Fact]
    public void HostedWebSearchTool_ShouldMapToGoogleSearchTool()
    {
        // Arrange
        var messages = new List<ChatMessage>
        {
            new(ChatRole.User, "Who was the first person to walk on the moon?")
        };

        var options = new ChatOptions { Tools = [new HostedWebSearchTool()] };

        // Act
        var request = MEAIToGeminiMapper.CreateMappedGenerateContentRequest("", messages, options);

        // Assert
        Assert.NotNull(request.Tools);
        Assert.Single(request.Tools, t => t.GoogleSearch is not null);
    }

    [Fact]
    public void OptionsInstruction_ShouldBeInsertedIntoSystemInstruction()
    {
        // Arrange
        List<ChatMessage> messages = [new(ChatRole.User, "Who was the first person to walk on the moon?")];

        const string instructions = "You are a helpful assistant.";

        var options = new ChatOptions { Instructions = instructions };

        // Act
        var request = MEAIToGeminiMapper.CreateMappedGenerateContentRequest("", messages, options);

        // Assert
        Assert.NotNull(request.SystemInstruction);
        var part = Assert.Single(request.SystemInstruction.Parts);
        Assert.Null(request.SystemInstruction.Role);
        Assert.Equal(instructions, part.Text);
    }

    [Fact]
    public void OptionsInstructionAndSystemMessage_ShouldBeCombinedIntoSingleSystemInstruction()
    {
        // Arrange
        const string systemMessage = "You are a helpful assistant that translates text.";

        List<ChatMessage> messages =
        [
            new(ChatRole.System, systemMessage),
            new(ChatRole.User, "Who was the first person to walk on the moon?")
        ];

        const string instructions = "Also, be very concise in your answers.";

        var options = new ChatOptions { Instructions = instructions };

        // Act
        var request = MEAIToGeminiMapper.CreateMappedGenerateContentRequest("", messages, options);

        // Assert
        Assert.NotNull(request.SystemInstruction);
        Assert.Equal(2, request.SystemInstruction.Parts.Count);
        Assert.Null(request.SystemInstruction.Role);
        Assert.Equal(instructions, request.SystemInstruction.Parts[0].Text);
        Assert.Equal(systemMessage, request.SystemInstruction.Parts[1].Text);
    }

    [Fact]
    public void MultipleSystemMessages_ShouldBeCombinedIntoSingleSystemInstruction()
    {
        // Arrange
        const string firstMessage = "You are a helpful assistant that translates text.";
        const string secondMessage = "Always respond in a cheerful tone.";

        List<ChatMessage> messages =
        [
            new(ChatRole.System, firstMessage),
            new(ChatRole.System, secondMessage),
            new(ChatRole.User, "Translate the following text to French: 'Hello, how are you?'")
        ];

        // Act
        var request = MEAIToGeminiMapper.CreateMappedGenerateContentRequest("", messages, null);

        // Assert
        Assert.NotNull(request.SystemInstruction);
        Assert.Equal(2, request.SystemInstruction.Parts.Count);
        Assert.Null(request.SystemInstruction.Role);
        Assert.Equal(firstMessage, request.SystemInstruction.Parts[0].Text);
        Assert.Equal(secondMessage, request.SystemInstruction.Parts[1].Text);
    }

    [Fact]
    public void CreateMappedGenerateContentRequest_WithRefsResponseFormat_ShouldMapResponseFormat()
    {
        var responseFormat = ChatResponseFormat.ForJsonSchema<Parent>();
        var options = new ChatOptions { ResponseFormat = responseFormat };
        var request = MEAIToGeminiMapper.CreateMappedGenerateContentRequest("", [], options);

        Assert.Equal(MediaTypeNames.Application.Json, request.GenerationConfiguration?.ResponseMimeType);
        Assert.Equal(responseFormat.Schema, request.GenerationConfiguration?.ResponseJsonSchema);
    }

    class Parent
    {
        public Child[] Children { get; set; } = [];
        public Child[] StepChildren { get; set; } = [];
    }

    class Child
    {
        public string Name { get; set; } = string.Empty;
    }

    private sealed class TestFunction : AIFunction
    {
        public override JsonElement JsonSchema { get; } = AIJsonUtilities.CreateJsonSchema(typeof(TestObject));

        protected override ValueTask<object?> InvokeCoreAsync(
            AIFunctionArguments arguments,
            CancellationToken cancellationToken)
        {
            return ValueTask.FromResult<object?>(null);
        }
    }

    private sealed record TestObject
    {
        [JsonPropertyName("name")]
        public required string Name { get; init; }
    }
}

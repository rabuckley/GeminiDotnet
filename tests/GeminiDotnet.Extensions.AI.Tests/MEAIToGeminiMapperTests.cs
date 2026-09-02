using GeminiDotnet.V1Beta;
using GeminiDotnet.V1Beta.Models;
using Microsoft.Extensions.AI;
using System.Diagnostics.CodeAnalysis;
using System.Net.Mime;
using System.Text.Json;
using System.Text.Json.Serialization;

#pragma warning disable CS0618 // EmbedContentRequest.OutputDimensionality is the only field the API honours

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
        var messages = new List<ChatMessage> { new(ChatRole.User, "Who was the first person to walk on the moon?") };

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
        var messages = new List<ChatMessage> { new(ChatRole.User, "Who was the first person to walk on the moon?") };

        var schema = AIJsonUtilities.CreateJsonSchema(typeof(TestObject),
            inferenceOptions: new AIJsonSchemaCreateOptions
            {
                TransformSchemaNode = null,
                IncludeSchemaKeyword = false,
                TransformOptions = new() { DisallowAdditionalProperties = true, RequireAllProperties = false, },
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
        var messages = new List<ChatMessage> { new(ChatRole.User, "Who was the first person to walk on the moon?") };

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
        var messages = new List<ChatMessage> { new(ChatRole.User, "Who was the first person to walk on the moon?") };

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
        var messages = new List<ChatMessage> { new(ChatRole.User, "Who was the first person to walk on the moon?") };

        var thinkingConfig = new ThinkingConfiguration { IncludeThoughts = true, ThinkingBudget = 1000 };

        var options = new ChatOptions
        {
            AdditionalProperties = new AdditionalPropertiesDictionary { ["thinkingConfig"] = thinkingConfig }
        };

        // Act
        var request = MEAIToGeminiMapper.CreateMappedGenerateContentRequest("", messages, options);

        // Assert
        Assert.Equal(thinkingConfig, request.GenerationConfiguration?.ThinkingConfiguration);
    }

    [Theory]
    [InlineData(ReasoningEffort.None, ThinkingConfigThinkingLevel.Minimal)]
    [InlineData(ReasoningEffort.Low, ThinkingConfigThinkingLevel.Low)]
    [InlineData(ReasoningEffort.Medium, ThinkingConfigThinkingLevel.Medium)]
    [InlineData(ReasoningEffort.High, ThinkingConfigThinkingLevel.High)]
    [InlineData(ReasoningEffort.ExtraHigh, ThinkingConfigThinkingLevel.High)]
    public void CreateMappedGenerateContentRequest_WithReasoningEffort_ShouldMapToThinkingLevel(
        ReasoningEffort effort,
        ThinkingConfigThinkingLevel expectedLevel)
    {
        // Arrange
        var messages = new List<ChatMessage> { new(ChatRole.User, "Think about this.") };

        var options = new ChatOptions
        {
            Reasoning = new ReasoningOptions { Effort = effort },
        };

        // Act
        var request = MEAIToGeminiMapper.CreateMappedGenerateContentRequest("", messages, options);

        // Assert
        Assert.Equal(expectedLevel, request.GenerationConfiguration?.ThinkingConfiguration?.ThinkingLevel);
    }

    [Theory]
    [InlineData(ReasoningOutput.None, false)]
    [InlineData(ReasoningOutput.Summary, true)]
    [InlineData(ReasoningOutput.Full, true)]
    public void CreateMappedGenerateContentRequest_WithReasoningOutput_ShouldMapToIncludeThoughts(
        ReasoningOutput output,
        bool expectedIncludeThoughts)
    {
        // Arrange
        var messages = new List<ChatMessage> { new(ChatRole.User, "Think about this.") };

        var options = new ChatOptions
        {
            Reasoning = new ReasoningOptions { Output = output },
        };

        // Act
        var request = MEAIToGeminiMapper.CreateMappedGenerateContentRequest("", messages, options);

        // Assert
        Assert.Equal(expectedIncludeThoughts, request.GenerationConfiguration?.ThinkingConfiguration?.IncludeThoughts);
    }

    [Fact]
    public void CreateMappedGenerateContentRequest_WithExplicitThinkingConfig_ShouldOverrideReasoning()
    {
        // Arrange — when both ChatOptions.Reasoning and AdditionalProperties["thinkingConfig"]
        // are set, the explicit ThinkingConfiguration takes precedence as a provider-specific override.
        var messages = new List<ChatMessage> { new(ChatRole.User, "Think about this.") };

        var explicitConfig = new ThinkingConfiguration { IncludeThoughts = true, ThinkingBudget = 2000 };

        var options = new ChatOptions
        {
            Reasoning = new ReasoningOptions { Effort = ReasoningEffort.Low, Output = ReasoningOutput.None },
            AdditionalProperties = new AdditionalPropertiesDictionary { ["thinkingConfig"] = explicitConfig },
        };

        // Act
        var request = MEAIToGeminiMapper.CreateMappedGenerateContentRequest("", messages, options);

        // Assert — the explicit config wins, not the Reasoning-derived one
        Assert.Equal(explicitConfig, request.GenerationConfiguration?.ThinkingConfiguration);
    }

    [Fact]
    public void CreateMappedGenerateContentRequest_WithThinkingConfigurationAsJsonElement_ShouldMapThinkingConfig()
    {
        // Arrange - This test simulates what happens when Semantic Kernel's
        // PromptExecutionSettings.ToChatOptions() roundtrips settings through JSON serialization,
        // causing typed objects to become JsonElements in AdditionalProperties.
        var messages = new List<ChatMessage> { new(ChatRole.User, "Who was the first person to walk on the moon?") };

        var thinkingConfig = new ThinkingConfiguration { IncludeThoughts = true, ThinkingBudget = 1000 };

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
        var messages = new List<ChatMessage> { new(ChatRole.User, "Who was the first person to walk on the moon?") };

        var options = new ChatOptions { Tools = [new HostedWebSearchTool()] };

        // Act
        var request = MEAIToGeminiMapper.CreateMappedGenerateContentRequest("", messages, options);

        // Assert
        Assert.NotNull(request.Tools);
        Assert.Single(request.Tools, t => t.GoogleSearch is not null);
    }

    [Fact]
    public void HostedFileSearchTool_ShouldMapToFileSearchTool()
    {
        // Arrange
        var messages = new List<ChatMessage> { new(ChatRole.User, "Tell me about I, Claudius") };

        var tool = new HostedFileSearchTool(new Dictionary<string, object?>
        {
            [GeminiAdditionalProperties.MetadataFilter] = "author = \"Robert Graves\"",
        })
        {
            Inputs = [new HostedVectorStoreContent("fileSearchStores/poems")],
            MaximumResultCount = 5,
        };

        var options = new ChatOptions { Tools = [tool] };

        // Act
        var request = MEAIToGeminiMapper.CreateMappedGenerateContentRequest("", messages, options);

        // Assert
        var fileSearch = Assert.Single(request.Tools!).FileSearch;
        Assert.NotNull(fileSearch);
        Assert.Equal("fileSearchStores/poems", Assert.Single(fileSearch.FileSearchStoreNames));
        Assert.Equal(5, fileSearch.TopK);
        Assert.Equal("author = \"Robert Graves\"", fileSearch.MetadataFilter);
    }

    [Fact]
    public void HostedFileSearchTool_WithInputsOnly_ShouldLeaveOptionalFieldsNull()
    {
        // Arrange
        var messages = new List<ChatMessage> { new(ChatRole.User, "Tell me about I, Claudius") };

        var options = new ChatOptions
        {
            Tools =
            [
                new HostedFileSearchTool { Inputs = [new HostedVectorStoreContent("fileSearchStores/poems")] },
            ],
        };

        // Act
        var request = MEAIToGeminiMapper.CreateMappedGenerateContentRequest("", messages, options);

        // Assert
        var fileSearch = Assert.Single(request.Tools!).FileSearch;
        Assert.NotNull(fileSearch);
        Assert.Null(fileSearch.TopK);
        Assert.Null(fileSearch.MetadataFilter);
    }

    [Fact]
    public void HostedFileSearchTool_WithSeveralStores_ShouldPreserveOrder()
    {
        // Arrange
        var messages = new List<ChatMessage> { new(ChatRole.User, "Tell me about I, Claudius") };

        var options = new ChatOptions
        {
            Tools =
            [
                new HostedFileSearchTool
                {
                    Inputs =
                    [
                        new HostedVectorStoreContent("fileSearchStores/poems"),
                        new HostedVectorStoreContent("fileSearchStores/letters"),
                    ],
                },
            ],
        };

        // Act
        var request = MEAIToGeminiMapper.CreateMappedGenerateContentRequest("", messages, options);

        // Assert
        var fileSearch = Assert.Single(request.Tools!).FileSearch;
        Assert.NotNull(fileSearch);
        Assert.Equal(["fileSearchStores/poems", "fileSearchStores/letters"], fileSearch.FileSearchStoreNames);
    }

    [Fact]
    public void HostedFileSearchTool_WithNonStringMetadataFilter_ShouldThrow()
    {
        // Arrange — dropping the filter would widen retrieval to the whole store, which nothing in the
        // response would reveal.
        var messages = new List<ChatMessage> { new(ChatRole.User, "Tell me about I, Claudius") };

        var tool = new HostedFileSearchTool(new Dictionary<string, object?>
        {
            [GeminiAdditionalProperties.MetadataFilter] = 42,
        })
        {
            Inputs = [new HostedVectorStoreContent("fileSearchStores/poems")],
        };

        var options = new ChatOptions { Tools = [tool] };

        // Act & Assert
        Assert.Throws<GeminiMappingException>(
            () => MEAIToGeminiMapper.CreateMappedGenerateContentRequest("", messages, options));
    }

    [Fact]
    public void HostedFileSearchTool_WithNullMetadataFilter_ShouldMapWithoutAFilter()
    {
        // Arrange — an explicitly null value reads as "no filter", not as a mistake.
        var messages = new List<ChatMessage> { new(ChatRole.User, "Tell me about I, Claudius") };

        var tool = new HostedFileSearchTool(new Dictionary<string, object?>
        {
            [GeminiAdditionalProperties.MetadataFilter] = null,
        })
        {
            Inputs = [new HostedVectorStoreContent("fileSearchStores/poems")],
        };

        var options = new ChatOptions { Tools = [tool] };

        // Act
        var request = MEAIToGeminiMapper.CreateMappedGenerateContentRequest("", messages, options);

        // Assert
        var fileSearch = Assert.Single(request.Tools!).FileSearch;
        Assert.NotNull(fileSearch);
        Assert.Null(fileSearch.MetadataFilter);
    }

    [Fact]
    public void HostedFileSearchTool_WithJsonElementMetadataFilter_ShouldMapTheFilter()
    {
        // Arrange — simulate SK's JSON roundtrip behavior, which delivers additional properties as
        // JsonElement rather than the original CLR type.
        var messages = new List<ChatMessage> { new(ChatRole.User, "Tell me about I, Claudius") };

        var tool = new HostedFileSearchTool(new Dictionary<string, object?>
        {
            [GeminiAdditionalProperties.MetadataFilter] =
                JsonSerializer.SerializeToElement("author = \"Robert Graves\""),
        })
        {
            Inputs = [new HostedVectorStoreContent("fileSearchStores/poems")],
        };

        var options = new ChatOptions { Tools = [tool] };

        // Act
        var request = MEAIToGeminiMapper.CreateMappedGenerateContentRequest("", messages, options);

        // Assert
        var fileSearch = Assert.Single(request.Tools!).FileSearch;
        Assert.NotNull(fileSearch);
        Assert.Equal("author = \"Robert Graves\"", fileSearch.MetadataFilter);
    }

    [Fact]
    public void HostedFileSearchTool_AlongsideAFunction_ShouldProduceBothTools()
    {
        // Arrange
        var messages = new List<ChatMessage> { new(ChatRole.User, "Tell me about I, Claudius") };

        var options = new ChatOptions
        {
            Tools =
            [
                new HostedFileSearchTool { Inputs = [new HostedVectorStoreContent("fileSearchStores/poems")] },
                AIFunctionFactory.Create(() => "sunny", "GetWeather"),
            ],
        };

        // Act
        var request = MEAIToGeminiMapper.CreateMappedGenerateContentRequest("", messages, options);

        // Assert
        Assert.Single(request.Tools!, t => t.FileSearch is not null);

        var functionTool = Assert.Single(request.Tools!, t => t.FunctionDeclarations is not null);
        Assert.Equal("GetWeather", Assert.Single(functionTool.FunctionDeclarations!).Name);
    }

    [Fact]
    public void HostedFileSearchTool_WithoutInputs_ShouldThrow()
    {
        // Arrange
        var messages = new List<ChatMessage> { new(ChatRole.User, "Tell me about I, Claudius") };
        var options = new ChatOptions { Tools = [new HostedFileSearchTool()] };

        // Act
        void Act() => MEAIToGeminiMapper.CreateMappedGenerateContentRequest("", messages, options);

        // Assert
        Assert.Throws<GeminiMappingException>(Act);
    }

    [Fact]
    public void HostedFileSearchTool_WithEmptyInputs_ShouldThrow()
    {
        // Arrange
        var messages = new List<ChatMessage> { new(ChatRole.User, "Tell me about I, Claudius") };
        var options = new ChatOptions { Tools = [new HostedFileSearchTool { Inputs = [] }] };

        // Act
        void Act() => MEAIToGeminiMapper.CreateMappedGenerateContentRequest("", messages, options);

        // Assert
        Assert.Throws<GeminiMappingException>(Act);
    }

    [Fact]
    public void HostedFileSearchTool_WithUnsupportedInput_ShouldThrow()
    {
        // Arrange
        var messages = new List<ChatMessage> { new(ChatRole.User, "Tell me about I, Claudius") };

        var options = new ChatOptions
        {
            Tools = [new HostedFileSearchTool { Inputs = [new HostedFileContent("files/abc123")] }],
        };

        // Act
        void Act() => MEAIToGeminiMapper.CreateMappedGenerateContentRequest("", messages, options);

        // Assert
        Assert.Throws<GeminiMappingException>(Act);
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

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void EmptySystemMessage_ShouldNotProduceASystemInstruction(string? text)
    {
        // Arrange — an empty part says nothing, and a null one serializes to a part with no field
        // set, which the API rejects with "required oneof field 'data' must have one initialized field".
        List<ChatMessage> messages =
        [
            new(ChatRole.System, [new TextContent(text!)]),
            new(ChatRole.User, "Who was the first person to walk on the moon?")
        ];

        // Act
        var request = MEAIToGeminiMapper.CreateMappedGenerateContentRequest("", messages, null);

        // Assert
        Assert.Null(request.SystemInstruction);
        var content = Assert.Single(request.Contents);
        Assert.Equal(ChatRoles.User, content.Role);
    }

    [Fact]
    public void EmptySystemMessage_ShouldNotDropTheInstructionsAroundIt()
    {
        // Arrange
        const string instructions = "You are a helpful assistant.";
        const string systemMessage = "Always respond in a cheerful tone.";

        List<ChatMessage> messages =
        [
            new(ChatRole.System, [new TextContent(""), new TextContent(systemMessage)]),
            new(ChatRole.User, "Who was the first person to walk on the moon?")
        ];

        // Act
        var request = MEAIToGeminiMapper.CreateMappedGenerateContentRequest(
            "",
            messages,
            new ChatOptions { Instructions = instructions });

        // Assert
        Assert.NotNull(request.SystemInstruction);
        Assert.Equal(2, request.SystemInstruction.Parts.Count);
        Assert.Equal(instructions, request.SystemInstruction.Parts[0].Text);
        Assert.Equal(systemMessage, request.SystemInstruction.Parts[1].Text);
    }

    [Fact]
    public void EmptyOptionsInstructions_ShouldNotProduceASystemInstruction()
    {
        // Arrange
        List<ChatMessage> messages = [new(ChatRole.User, "Who was the first person to walk on the moon?")];

        var options = new ChatOptions { Instructions = "" };

        // Act
        var request = MEAIToGeminiMapper.CreateMappedGenerateContentRequest("", messages, options);

        // Assert
        Assert.Null(request.SystemInstruction);
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

    [Fact]
    public void CreateMappedGenerateContentRequest_WithRawRepresentation_ShouldUse()
    {
        // Arrange
        List<ChatMessage> messages =
        [
            new(ChatRole.User, "Goodbye!"),
        ];

        const string rawCachedContent = "cached content name";

        var rawGenerationConfig = new GenerationConfiguration { MaxOutputTokens = 1000, Temperature = 0.5f, };

        List<Content> rawContents =
        [
            new() { Role = ChatRoles.User, Parts = [new Part { Text = "Hello!" }] }
        ];

        List<SafetySetting> rawSafetySettings = [];

        var rawSystemInstruction = new Content { Parts = [new Part { Text = "You are a helpful assistant." }] };

        var rawToolConfiguration = new ToolConfiguration { };

        List<Tool> rawTools = [];

        var rawRepresentation = new GenerateContentRequest
        {
            CachedContent = rawCachedContent,
            GenerationConfiguration = rawGenerationConfig,
            Contents = rawContents,
            Model = "",
            SafetySettings = rawSafetySettings,
            SystemInstruction = rawSystemInstruction,
            ToolConfiguration = rawToolConfiguration,
            Tools = rawTools,
        };

        // Act
        var request = MEAIToGeminiMapper.CreateMappedGenerateContentRequest(
            model: "model",
            messages,
            options: new ChatOptions(),
            rawRepresentation);

        // Assert
        Assert.Same(rawCachedContent, request.CachedContent);
        Assert.Same(rawGenerationConfig, request.GenerationConfiguration);
        Assert.Same(rawContents, request.Contents);
        Assert.Same(rawSafetySettings, request.SafetySettings);
        Assert.Same(rawSystemInstruction, request.SystemInstruction);
        Assert.Same(rawToolConfiguration, request.ToolConfiguration);
        Assert.Same(rawTools, request.Tools);
    }

    [Fact]
    public void CreateMappedBatchEmbeddingRequest_CreatesOneRequestPerInputValue()
    {
        // Arrange
        var inputValues = new[] { "First text", "Second text", "Third text" };
        var options = new EmbeddingGenerationOptions { Dimensions = 768 };
        var clientOptions = new GeminiClientOptions { DefaultEmbeddingDimensions = 512, ApiKey = "not needed" };

        // Act
        var result = MEAIToGeminiMapper.CreateMappedBatchEmbeddingRequest(
            "text-embedding-004",
            inputValues,
            options,
            clientOptions);

        // Assert
        Assert.Equal(3, result.Requests.Count);

        for (int i = 0; i < inputValues.Length; i++)
        {
            var request = result.Requests[i];
            // Model should be prefixed with "models/" for BatchEmbedContents API
            Assert.Equal("models/text-embedding-004", request.Model);
            Assert.Equal(768, request.OutputDimensionality);
            Assert.NotNull(request.Content);

            Assert.NotNull(request.Content.Parts);
            var part = Assert.Single(request.Content.Parts);
            Assert.Equal(inputValues[i], part.Text);
        }
    }

    [Fact]
    public void CreateMappedBatchEmbeddingRequest_UsesDefaultDimensionsWhenNotSpecified()
    {
        // Arrange
        var inputValues = new[] { "Sample text" };
        var options = new EmbeddingGenerationOptions { };
        var clientOptions = new GeminiClientOptions { DefaultEmbeddingDimensions = 1024, ApiKey = "not needed" };

        // Act
        var result = MEAIToGeminiMapper.CreateMappedBatchEmbeddingRequest(
            "gemini-embedding-001",
            inputValues,
            options,
            clientOptions);

        // Assert
        Assert.Single(result.Requests);
        var request = result.Requests[0];
        Assert.Equal(clientOptions.DefaultEmbeddingDimensions, request.OutputDimensionality);
    }

    [Fact]
    public void CreateMappedBatchEmbeddingRequest_WithoutDimensions_ShouldNotSetOutputDimensionality()
    {
        // Arrange
        var inputValues = new[] { "Sample text" };
        var clientOptions = new GeminiClientOptions { ApiKey = "not needed" };

        // Act
        var result = MEAIToGeminiMapper.CreateMappedBatchEmbeddingRequest(
            "gemini-embedding-001",
            inputValues,
            options: null,
            clientOptions);

        // Assert
        var request = Assert.Single(result.Requests);
        Assert.Null(request.OutputDimensionality);
    }

    [Fact]
    public void CreateMappedBatchEmbeddingRequest_WithRawRepresentation_ShouldUse()
    {
        // Arrange
        var inputValues = new[] { "Sample text" };
        var options = new EmbeddingGenerationOptions { Dimensions = 256 };
        var clientOptions = new GeminiClientOptions { ApiKey = "not needed" };

        var rawRepresentation = new BatchEmbedContentsRequest { Requests = [] };

        // Act
        var result = MEAIToGeminiMapper.CreateMappedBatchEmbeddingRequest(
            "gemini-embedding-001",
            inputValues,
            options,
            clientOptions,
            rawRepresentation);

        // Assert
        Assert.Same(rawRepresentation, result);
    }

    [Fact]
    public void CreateMappedGenerateContentRequest_WithRequiredToolMode_ShouldMapToAny()
    {
        // Arrange
        var messages = new List<ChatMessage> { new(ChatRole.User, "Call a function") };
        var options = new ChatOptions
        {
            ToolMode = ChatToolMode.RequireAny,
            Tools = [new TestFunction()],
        };

        // Act
        var request = MEAIToGeminiMapper.CreateMappedGenerateContentRequest("model", messages, options);

        // Assert
        Assert.NotNull(request.ToolConfiguration);
        Assert.Equal(FunctionCallingConfigMode.Any, request.ToolConfiguration.FunctionCallingConfiguration?.Mode);
        Assert.Null(request.ToolConfiguration.FunctionCallingConfiguration?.AllowedFunctionNames);
    }

    [Fact]
    public void CreateMappedGenerateContentRequest_WithRequiredSpecificFunction_ShouldSetAllowedFunctionNames()
    {
        // Arrange
        var messages = new List<ChatMessage> { new(ChatRole.User, "Call a function") };
        var options = new ChatOptions
        {
            ToolMode = ChatToolMode.RequireSpecific("get_weather"),
            Tools = [new TestFunction()],
        };

        // Act
        var request = MEAIToGeminiMapper.CreateMappedGenerateContentRequest("model", messages, options);

        // Assert
        Assert.NotNull(request.ToolConfiguration);
        var config = request.ToolConfiguration.FunctionCallingConfiguration;
        Assert.Equal(FunctionCallingConfigMode.Any, config?.Mode);
        var allowedName = Assert.Single(config!.AllowedFunctionNames!);
        Assert.Equal("get_weather", allowedName);
    }

    [Fact]
    public void CreateMappedGenerateContentRequest_WithFunctionResult_ShouldResolveFunctionName()
    {
        // Arrange — the assistant message contains a FunctionCallContent with a known
        // name, and the tool message contains a FunctionResultContent referencing it by
        // CallId. The mapper should resolve the function name from the call.
        const string callId = "call-123";
        const string functionName = "get_weather";

        List<ChatMessage> messages =
        [
            new(ChatRole.User, "What's the weather?"),
            new(ChatRole.Assistant,
            [
                new FunctionCallContent(callId, functionName, new Dictionary<string, object?> { ["city"] = "London" }),
            ]),
            new(ChatRole.Tool,
            [
                new FunctionResultContent(callId, "Sunny, 22°C"),
            ]),
        ];

        // Act
        var request = MEAIToGeminiMapper.CreateMappedGenerateContentRequest("model", messages, null);

        // Assert — the last content (tool message) should have a FunctionResponse
        // with the resolved function name, not the call ID.
        var toolContent = request.Contents.Last();
        var functionResponse = toolContent.Parts.Single().FunctionResponse;

        Assert.NotNull(functionResponse);
        Assert.Equal(functionName, functionResponse.Name);
        Assert.Equal(callId, functionResponse.Id);
    }

    [Fact]
    public void CreateMappedGenerateContentRequest_WithFunctionResult_NoMatchingCall_FallsBackToCallId()
    {
        // Arrange — no matching FunctionCallContent exists in the conversation.
        const string callId = "orphan-call-456";

        List<ChatMessage> messages =
        [
            new(ChatRole.Tool,
            [
                new FunctionResultContent(callId, "some result"),
            ]),
        ];

        // Act
        var request = MEAIToGeminiMapper.CreateMappedGenerateContentRequest("model", messages, null);

        // Assert — falls back to CallId when no matching FunctionCallContent is found.
        var functionResponse = request.Contents.Single().Parts.Single().FunctionResponse;

        Assert.NotNull(functionResponse);
        Assert.Equal(callId, functionResponse.Name);
    }

    [Fact]
    public void CreateMappedGenerateContentRequest_WithEmptyTextContent_ShouldNotEmitATextPart()
    {
        // Arrange — response mapping synthesizes an empty TextContent to carry citations that ground no
        // span. Gemini rejects a part whose text is empty, so feeding that response back must not send one.
        List<ChatMessage> messages =
        [
            new(ChatRole.Assistant,
            [
                new TextContent("Grounded answer."),
                new TextContent(string.Empty) { Annotations = [new CitationAnnotation { Title = "Source" }] },
            ]),
        ];

        // Act
        var request = MEAIToGeminiMapper.CreateMappedGenerateContentRequest("model", messages, null);

        // Assert
        var part = Assert.Single(request.Contents.Single().Parts);
        Assert.Equal("Grounded answer.", part.Text);
    }

    [Fact]
    public void CreateMappedGenerateContentRequest_WithOnlyEmptyTextContent_ShouldDropTheMessage()
    {
        // Arrange — a candidate grounded without a text part of its own maps to a message whose only
        // content is the empty citation carrier. Fed back as history, it must not become an
        // empty-parts Content, which the API rejects.
        var candidate = new Candidate
        {
            Content = new Content { Role = "model", Parts = null },
            GroundingMetadata = new GroundingMetadata
            {
                GroundingChunks =
                [
                    new GroundingChunk { Web = new Web { Uri = "https://example.com", Title = "Example" } },
                ],
            },
            FinishReason = CandidateFinishReason.Stop,
        };

        var response = GeminiToMEAIMapper.CreateMappedChatResponse(
            new GenerateContentResponse { Candidates = [candidate], ModelVersion = "gemini-2.0-flash" },
            DateTimeOffset.UtcNow);

        var carrier = Assert.IsType<TextContent>(Assert.Single(Assert.Single(response.Messages).Contents));
        Assert.Equal(string.Empty, carrier.Text);

        List<ChatMessage> messages = [new(ChatRole.User, "Who?"), .. response.Messages];

        // Act
        var request = MEAIToGeminiMapper.CreateMappedGenerateContentRequest("model", messages, null);

        // Assert
        var content = Assert.Single(request.Contents);
        Assert.Equal(ChatRoles.User, content.Role);
        Assert.Equal("Who?", Assert.Single(content.Parts!).Text);
        Assert.Null(request.SystemInstruction);
    }

    [Fact]
    public void CreateMappedGenerateContentRequest_WithCandidateThatOmittedItsRole_ShouldKeepItInTheConversation()
    {
        // Arrange — Content.Role is optional on a candidate. The turn is still the model's, so fed back
        // as history it belongs in Contents, not in SystemInstruction.
        var candidate = new Candidate
        {
            Content = new Content { Role = null, Parts = [new Part { Text = "The answer is 42." }] },
            FinishReason = CandidateFinishReason.Stop,
        };

        var response = GeminiToMEAIMapper.CreateMappedChatResponse(
            new GenerateContentResponse { Candidates = [candidate], ModelVersion = "gemini-2.0-flash" },
            DateTimeOffset.UtcNow);

        List<ChatMessage> messages = [new(ChatRole.User, "Who?"), .. response.Messages];

        // Act
        var request = MEAIToGeminiMapper.CreateMappedGenerateContentRequest("model", messages, null);

        // Assert
        Assert.Null(request.SystemInstruction);
        Assert.Collection(
            request.Contents,
            content =>
            {
                Assert.Equal(ChatRoles.User, content.Role);
                Assert.Equal("Who?", Assert.Single(content.Parts!).Text);
            },
            content =>
            {
                Assert.Equal(ChatRoles.Model, content.Role);
                Assert.Equal("The answer is 42.", Assert.Single(content.Parts!).Text);
            });
    }

    [Fact]
    public void HostedFileContent_ShouldMapToFileDataPart()
    {
        // Arrange
        const string fileUri = "https://generativelanguage.googleapis.com/v1beta/files/abc123";
        const string mimeType = "text/csv";

        List<ChatMessage> messages =
        [
            new(ChatRole.User, [new HostedFileContent(fileUri) { MediaType = mimeType }]),
        ];

        // Act
        var request = MEAIToGeminiMapper.CreateMappedGenerateContentRequest("model", messages, null);

        // Assert
        var part = Assert.Single(request.Contents.Single().Parts!);
        Assert.NotNull(part.FileData);
        Assert.Equal(fileUri, part.FileData.FileUri);
        Assert.Equal(mimeType, part.FileData.MimeType);
    }

    [Fact]
    public void HostedCodeInterpreterTool_WithInputs_ShouldInjectFilePartsIntoLastUserContent()
    {
        // Arrange
        const string fileUri = "https://generativelanguage.googleapis.com/v1beta/files/sales-data";
        const string mimeType = "text/csv";
        const string userPrompt = "Analyze this data and find the top sellers.";

        List<ChatMessage> messages =
        [
            new(ChatRole.User, userPrompt),
        ];

        var codeInterpreter = new HostedCodeInterpreterTool
        {
            Inputs = [new HostedFileContent(fileUri) { MediaType = mimeType }],
        };

        var options = new ChatOptions { Tools = [codeInterpreter] };

        // Act
        var request = MEAIToGeminiMapper.CreateMappedGenerateContentRequest("model", messages, options);

        // Assert — the file part should be prepended before the text part
        var userContent = Assert.Single(request.Contents);
        Assert.Equal(2, userContent.Parts!.Count);

        var filePart = userContent.Parts[0];
        Assert.NotNull(filePart.FileData);
        Assert.Equal(fileUri, filePart.FileData.FileUri);
        Assert.Equal(mimeType, filePart.FileData.MimeType);

        var textPart = userContent.Parts[1];
        Assert.Equal(userPrompt, textPart.Text);
    }

    [Fact]
    public void HostedCodeInterpreterTool_WithNoInputs_ShouldNotModifyContent()
    {
        // Arrange
        const string userPrompt = "Write a hello world program.";

        List<ChatMessage> messages =
        [
            new(ChatRole.User, userPrompt),
        ];

        var options = new ChatOptions { Tools = [new HostedCodeInterpreterTool()] };

        // Act
        var request = MEAIToGeminiMapper.CreateMappedGenerateContentRequest("model", messages, options);

        // Assert — content should be unchanged
        var userContent = Assert.Single(request.Contents);
        var part = Assert.Single(userContent.Parts!);
        Assert.Equal(userPrompt, part.Text);
    }

    [Fact]
    public void HostedCodeInterpreterTool_WithMultipleInputs_ShouldInjectAllFilePartsIntoLastUserContent()
    {
        // Arrange
        const string csvUri = "https://generativelanguage.googleapis.com/v1beta/files/data-csv";
        const string pdfUri = "https://generativelanguage.googleapis.com/v1beta/files/report-pdf";

        List<ChatMessage> messages =
        [
            new(ChatRole.Assistant, "Sure, send me the files."),
            new(ChatRole.User, "Here are the files to analyze."),
        ];

        var codeInterpreter = new HostedCodeInterpreterTool
        {
            Inputs =
            [
                new HostedFileContent(csvUri) { MediaType = "text/csv" },
                new HostedFileContent(pdfUri) { MediaType = "application/pdf" },
            ],
        };

        var options = new ChatOptions { Tools = [codeInterpreter] };

        // Act
        var request = MEAIToGeminiMapper.CreateMappedGenerateContentRequest("model", messages, options);

        // Assert — file parts should be prepended to the last user content (index 1)
        var lastUserContent = request.Contents.Last();
        Assert.Equal(3, lastUserContent.Parts!.Count);

        Assert.Equal(csvUri, lastUserContent.Parts[0].FileData!.FileUri);
        Assert.Equal(pdfUri, lastUserContent.Parts[1].FileData!.FileUri);
        Assert.Equal("Here are the files to analyze.", lastUserContent.Parts[2].Text);
    }

    [Fact]
    public void HostedFileContent_WithNullMediaType_ShouldMapToFileDataWithNullMimeType()
    {
        // Arrange — HostedFileContent with no MediaType set
        const string fileUri = "https://generativelanguage.googleapis.com/v1beta/files/unknown-type";

        List<ChatMessage> messages =
        [
            new(ChatRole.User, [new HostedFileContent(fileUri)]),
        ];

        // Act
        var request = MEAIToGeminiMapper.CreateMappedGenerateContentRequest("model", messages, null);

        // Assert
        var part = Assert.Single(request.Contents.Single().Parts!);
        Assert.NotNull(part.FileData);
        Assert.Equal(fileUri, part.FileData.FileUri);
        Assert.Null(part.FileData.MimeType);
    }

    [Fact]
    public void HostedCodeInterpreterTool_WithInputs_AndInlineHostedFileContent_ShouldCombineParts()
    {
        // Arrange — a user message already contains an inline HostedFileContent part,
        // and the tool also provides files via Inputs. The tool-input files should be
        // prepended before the inline parts.
        const string inlineFileUri = "https://generativelanguage.googleapis.com/v1beta/files/inline-file";
        const string toolInputFileUri = "https://generativelanguage.googleapis.com/v1beta/files/tool-input-file";
        const string userPrompt = "Analyze both files.";

        List<ChatMessage> messages =
        [
            new(ChatRole.User,
            [
                new HostedFileContent(inlineFileUri) { MediaType = "text/csv" },
                new TextContent(userPrompt),
            ]),
        ];

        var codeInterpreter = new HostedCodeInterpreterTool
        {
            Inputs = [new HostedFileContent(toolInputFileUri) { MediaType = "application/pdf" }],
        };

        var options = new ChatOptions { Tools = [codeInterpreter] };

        // Act
        var request = MEAIToGeminiMapper.CreateMappedGenerateContentRequest("model", messages, options);

        // Assert — tool-input file prepended, then inline file, then text
        var userContent = Assert.Single(request.Contents);
        Assert.Equal(3, userContent.Parts!.Count);

        Assert.Equal(toolInputFileUri, userContent.Parts[0].FileData!.FileUri);
        Assert.Equal(inlineFileUri, userContent.Parts[1].FileData!.FileUri);
        Assert.Equal(userPrompt, userContent.Parts[2].Text);
    }

    [Fact]
    public void HostedCodeInterpreterTool_WithInputs_NoUserContent_ShouldThrow()
    {
        // Arrange — only assistant messages, no user content to attach files to
        List<ChatMessage> messages =
        [
            new(ChatRole.Assistant, "I can help analyze data."),
        ];

        var codeInterpreter = new HostedCodeInterpreterTool
        {
            Inputs = [new HostedFileContent("https://generativelanguage.googleapis.com/v1beta/files/orphan") { MediaType = "text/csv" }],
        };

        var options = new ChatOptions { Tools = [codeInterpreter] };

        // Act
        Action act = () => MEAIToGeminiMapper.CreateMappedGenerateContentRequest("model", messages, options);

        // Assert
        Assert.Throws<InvalidOperationException>(act);
    }

    [Fact]
    public void WebSearchContent_ShouldBeSkippedInReverseMapping()
    {
        // Arrange — an assistant message containing web search content (from a
        // previous grounded response) should not throw when mapped back to Gemini.
        const string callId = "web-search/test-id";

        List<ChatMessage> messages =
        [
            new(ChatRole.Assistant,
            [
                new TextContent("Here are the results."),
                new WebSearchToolCallContent(callId) { Queries = ["test query"] },
                new WebSearchToolResultContent(callId)
                {
                    Outputs = [new UriContent("https://example.com", "text/html")],
                },
            ]),
        ];

        // Act
        var request = MEAIToGeminiMapper.CreateMappedGenerateContentRequest("model", messages, null);

        // Assert — only the text part should survive; web search content is skipped
        var content = Assert.Single(request.Contents);
        var part = Assert.Single(content.Parts!);
        Assert.Equal("Here are the results.", part.Text);
    }

    [Fact]
    public void HostedMcpServerTool_ShouldMapToMcpServer()
    {
        // Arrange
        var messages = new List<ChatMessage> { new(ChatRole.User, "Is it raining in London?") };

        var tool = new HostedMcpServerTool("weather", "https://example.com/mcp")
        {
            ApprovalMode = HostedMcpServerToolApprovalMode.NeverRequire,
            ServerDescription = "Forecasts for UK cities.",
            Headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["Authorization"] = "Bearer token",
            },
        };

        var options = new ChatOptions { Tools = [tool] };

        // Act
        var request = MEAIToGeminiMapper.CreateMappedGenerateContentRequest("", messages, options);

        // Assert
        var mcpServer = Assert.Single(Assert.Single(request.Tools!).McpServers!);
        Assert.Equal("weather", mcpServer.Name);
        Assert.Equal("https://example.com/mcp", mcpServer.StreamableHttpTransport?.Url);
        Assert.Equal(
            new KeyValuePair<string, string>("Authorization", "Bearer token"),
            Assert.Single(mcpServer.StreamableHttpTransport!.Headers!));

        // ServerDescription has no Gemini counterpart, so it must not reach the wire under any name.
        Assert.DoesNotContain("Forecasts for UK cities.", JsonSerializer.Serialize(request.Tools));
    }

    [Fact]
    public void HostedMcpServerTool_ShouldMatchTheWireShapeGeminiAccepts()
    {
        // Arrange — this is the tools payload of a request the live v1beta API answered with HTTP 200.
        const string expected =
            """[{"mcpServers":[{"name":"weather","streamableHttpTransport":{"url":"https://gemini-api-demos.uc.r.appspot.com/mcp"}}]}]""";

        var messages = new List<ChatMessage> { new(ChatRole.User, "Is it raining in London?") };

        var options = new ChatOptions
        {
            Tools =
            [
                new HostedMcpServerTool("weather", "https://gemini-api-demos.uc.r.appspot.com/mcp")
                    { ApprovalMode = HostedMcpServerToolApprovalMode.NeverRequire },
            ],
        };

        // Act
        var request = MEAIToGeminiMapper.CreateMappedGenerateContentRequest("", messages, options);

        // Assert
        Assert.Equal(expected, JsonSerializer.Serialize(request.Tools));
    }

    [Fact]
    public void HostedMcpServerTool_WithHeadersMutatedAfterMapping_ShouldKeepTheRequestUnchanged()
    {
        // Arrange
        var messages = new List<ChatMessage> { new(ChatRole.User, "Is it raining in London?") };

        var headers = new Dictionary<string, string> { ["Authorization"] = "Bearer token" };
        var tool = new HostedMcpServerTool("weather", "https://example.com/mcp")
        {
            ApprovalMode = HostedMcpServerToolApprovalMode.NeverRequire,
            Headers = headers,
        };
        var options = new ChatOptions { Tools = [tool] };

        // Act
        var request = MEAIToGeminiMapper.CreateMappedGenerateContentRequest("", messages, options);
        headers["Authorization"] = "Bearer leaked";
        headers["X-Extra"] = "added";

        // Assert
        var mcpServer = Assert.Single(Assert.Single(request.Tools!).McpServers!);
        Assert.Equal(
            new KeyValuePair<string, string>("Authorization", "Bearer token"),
            Assert.Single(mcpServer.StreamableHttpTransport!.Headers!));
    }

    [Fact]
    public void HostedMcpServerTool_WithEmptyHeaders_ShouldMapHeadersToNull()
    {
        // Arrange — an empty dictionary is not omitted by WhenWritingDefault, so it would ship "headers":{}.
        var messages = new List<ChatMessage> { new(ChatRole.User, "Is it raining in London?") };

        var tool = new HostedMcpServerTool("weather", "https://example.com/mcp")
        {
            ApprovalMode = HostedMcpServerToolApprovalMode.NeverRequire,
            Headers = new Dictionary<string, string>(),
        };

        var options = new ChatOptions { Tools = [tool] };

        // Act
        var request = MEAIToGeminiMapper.CreateMappedGenerateContentRequest("", messages, options);

        // Assert
        var mcpServer = Assert.Single(Assert.Single(request.Tools!).McpServers!);
        Assert.Null(mcpServer.StreamableHttpTransport?.Headers);
    }

    [Fact]
    public void HostedMcpServerTool_WithSeveralServers_ShouldMapEachOne()
    {
        // Arrange
        var messages = new List<ChatMessage> { new(ChatRole.User, "Is it raining in London?") };

        var options = new ChatOptions
        {
            Tools =
            [
                new HostedMcpServerTool("weather", "https://example.com/weather/mcp")
                    { ApprovalMode = HostedMcpServerToolApprovalMode.NeverRequire },
                new HostedMcpServerTool("tides", "https://example.com/tides/mcp")
                    { ApprovalMode = HostedMcpServerToolApprovalMode.NeverRequire },
            ],
        };

        // Act
        var request = MEAIToGeminiMapper.CreateMappedGenerateContentRequest("", messages, options);

        // Assert — how the servers are grouped into Tool entries is not observable to the API.
        var mcpServers = request.Tools!.SelectMany(t => t.McpServers ?? []).ToList();
        Assert.Equal(["weather", "tides"], mcpServers.Select(s => s.Name));
        Assert.Equal(
            ["https://example.com/weather/mcp", "https://example.com/tides/mcp"],
            mcpServers.Select(s => s.StreamableHttpTransport?.Url));
    }

    [Fact]
    public void HostedMcpServerTool_WithAFunction_ShouldMapBoth()
    {
        // Arrange
        var messages = new List<ChatMessage> { new(ChatRole.User, "Is it raining in London?") };

        var options = new ChatOptions
        {
            Tools =
            [
                new HostedMcpServerTool("weather", "https://example.com/mcp")
                    { ApprovalMode = HostedMcpServerToolApprovalMode.NeverRequire },
                new TestFunction(),
            ],
        };

        // Act
        var request = MEAIToGeminiMapper.CreateMappedGenerateContentRequest("", messages, options);

        // Assert
        var mcpServer = Assert.Single(request.Tools!.SelectMany(t => t.McpServers ?? []));
        Assert.Equal("weather", mcpServer.Name);
        Assert.Single(request.Tools!.SelectMany(t => t.FunctionDeclarations ?? []));
    }

    public static TheoryData<IList<string>> AllowedToolLists => new()
    {
        new List<string> { "get_weather" },
        new List<string>(),
    };

    [Theory]
    [MemberData(nameof(AllowedToolLists))]
    public void HostedMcpServerTool_WithAllowedTools_ShouldThrow(IList<string> allowedTools)
    {
        // Arrange — Gemini accepts an allow-list and then ignores it, so mapping one would promise a
        // restriction that never applies.
        var messages = new List<ChatMessage> { new(ChatRole.User, "Is it raining in London?") };

        var tool = new HostedMcpServerTool("weather", "https://example.com/mcp")
        {
            ApprovalMode = HostedMcpServerToolApprovalMode.NeverRequire,
            AllowedTools = allowedTools,
        };
        var options = new ChatOptions { Tools = [tool] };

        // Act
        void Act() => MEAIToGeminiMapper.CreateMappedGenerateContentRequest("", messages, options);

        // Assert
        Assert.Throws<GeminiMappingException>(Act);
    }

    public static TheoryData<HostedMcpServerToolApprovalMode?> ApprovalModesGeminiCannotHonour => new()
    {
        // Null is the default, and M.E.AI documents it as a value providers may read as AlwaysRequire, so
        // it has to be rejected rather than resolved into consent the caller never gave.
        null,
        HostedMcpServerToolApprovalMode.AlwaysRequire,
        HostedMcpServerToolApprovalMode.RequireSpecific(alwaysRequireApprovalToolNames: ["get_weather"],
            neverRequireApprovalToolNames: null),
    };

    [Theory]
    [MemberData(nameof(ApprovalModesGeminiCannotHonour))]
    public void HostedMcpServerTool_WithoutNeverRequireApproval_ShouldThrow(
        HostedMcpServerToolApprovalMode? approvalMode)
    {
        // Arrange — Gemini runs the tools server-side with no approval hook.
        var messages = new List<ChatMessage> { new(ChatRole.User, "Is it raining in London?") };

        var tool = new HostedMcpServerTool("weather", "https://example.com/mcp") { ApprovalMode = approvalMode };
        var options = new ChatOptions { Tools = [tool] };

        // Act
        void Act() => MEAIToGeminiMapper.CreateMappedGenerateContentRequest("", messages, options);

        // Assert
        Assert.Throws<GeminiMappingException>(Act);
    }

    [Fact]
    public void HostedMcpServerTool_WithNeverRequireApproval_ShouldMap()
    {
        // Arrange
        var messages = new List<ChatMessage> { new(ChatRole.User, "Is it raining in London?") };

        var tool = new HostedMcpServerTool("weather", "https://example.com/mcp")
        {
            ApprovalMode = HostedMcpServerToolApprovalMode.NeverRequire,
        };

        var options = new ChatOptions { Tools = [tool] };

        // Act
        var request = MEAIToGeminiMapper.CreateMappedGenerateContentRequest("", messages, options);

        // Assert
        var mcpServer = Assert.Single(Assert.Single(request.Tools!).McpServers!);
        Assert.Equal("weather", mcpServer.Name);
    }

    public static TheoryData<AITool> BuiltInTools => new()
    {
        new HostedWebSearchTool(),
        new HostedCodeInterpreterTool(),
        new HostedFileSearchTool { Inputs = [new HostedVectorStoreContent("fileSearchStores/poems")] },
    };

    [Theory]
    [MemberData(nameof(BuiltInTools))]
    public void HostedMcpServerTool_WithABuiltInTool_ShouldThrow(AITool builtInTool)
    {
        // Arrange — Gemini rejects this combination outright.
        var messages = new List<ChatMessage> { new(ChatRole.User, "Is it raining in London?") };

        var options = new ChatOptions
        {
            Tools =
            [
                new HostedMcpServerTool("weather", "https://example.com/mcp")
                    { ApprovalMode = HostedMcpServerToolApprovalMode.NeverRequire },
                builtInTool,
            ],
        };

        // Act
        void Act() => MEAIToGeminiMapper.CreateMappedGenerateContentRequest("", messages, options);

        // Assert
        Assert.Throws<GeminiMappingException>(Act);
    }

    public static TheoryData<AITool> ToolsThatDeclareNoFunctions => new()
    {
        new HostedMcpServerTool("weather", "https://example.com/mcp")
            { ApprovalMode = HostedMcpServerToolApprovalMode.NeverRequire },
        new HostedWebSearchTool(),
        new HostedCodeInterpreterTool(),
        new HostedFileSearchTool { Inputs = [new HostedVectorStoreContent("fileSearchStores/poems")] },
    };

    [Theory]
    [MemberData(nameof(ToolsThatDeclareNoFunctions))]
    public void CreateMappedGenerateContentRequest_WithRequiredToolModeAndNoFunctions_ShouldThrow(AITool tool)
    {
        // Arrange — asked to require a function call when the request declares no functions, Gemini burns
        // tool-call round-trips and answers with an empty TOO_MANY_TOOL_CALLS candidate, so this has to fail
        // here rather than bill the caller for nothing.
        var messages = new List<ChatMessage> { new(ChatRole.User, "Is it raining in London?") };

        var options = new ChatOptions { Tools = [tool], ToolMode = ChatToolMode.RequireAny };

        // Act
        void Act() => MEAIToGeminiMapper.CreateMappedGenerateContentRequest("", messages, options);

        // Assert
        Assert.Throws<GeminiMappingException>(Act);
    }

    [Fact]
    public void CreateMappedGenerateContentRequest_WithRequiredToolModeAndFunctionsFromTheRawRepresentation_ShouldMapToAny()
    {
        // Arrange — the functions the request declares come from the raw representation, not from
        // ChatOptions.Tools, so ANY mode is satisfiable and must not be rejected.
        var messages = new List<ChatMessage> { new(ChatRole.User, "Is it raining in London?") };

        var options = new ChatOptions
        {
            Tools =
            [
                new HostedMcpServerTool("weather", "https://example.com/mcp")
                    { ApprovalMode = HostedMcpServerToolApprovalMode.NeverRequire },
            ],
            ToolMode = ChatToolMode.RequireAny,
        };

        var rawRepresentation = new GenerateContentRequest
        {
            Model = "",
            Contents = [],
            Tools =
            [
                new Tool
                {
                    FunctionDeclarations =
                        [new FunctionDeclaration { Name = "get_weather", Description = "Gets the weather." }],
                },
            ],
        };

        // Act
        var request = MEAIToGeminiMapper.CreateMappedGenerateContentRequest("", messages, options, rawRepresentation);

        // Assert
        Assert.Equal(FunctionCallingConfigMode.Any, request.ToolConfiguration?.FunctionCallingConfiguration?.Mode);
    }

    [Fact]
    public void CreateMappedGenerateContentRequest_WithRequiredToolModeAndAFunctionAlongsideAnMcpServer_ShouldMapToAny()
    {
        // Arrange
        var messages = new List<ChatMessage> { new(ChatRole.User, "Is it raining in London?") };

        var options = new ChatOptions
        {
            Tools =
            [
                new HostedMcpServerTool("weather", "https://example.com/mcp")
                    { ApprovalMode = HostedMcpServerToolApprovalMode.NeverRequire },
                new TestFunction(),
            ],
            ToolMode = ChatToolMode.RequireAny,
        };

        // Act
        var request = MEAIToGeminiMapper.CreateMappedGenerateContentRequest("", messages, options);

        // Assert
        Assert.Equal(FunctionCallingConfigMode.Any, request.ToolConfiguration?.FunctionCallingConfiguration?.Mode);
    }

    [Fact]
    public void CreateMappedGenerateContentRequest_WithAnUnsupportedToolType_ShouldThrow()
    {
        // Arrange
        var messages = new List<ChatMessage> { new(ChatRole.User, "Is it raining in London?") };

        var options = new ChatOptions { Tools = [new UnsupportedTool()] };

        // Act
        void Act() => MEAIToGeminiMapper.CreateMappedGenerateContentRequest("", messages, options);

        // Assert
        Assert.Throws<GeminiMappingException>(Act);
    }

    [Fact]
    public void CreateMappedGenerateContentRequest_WithAToolCallCarryingItsPart_ShouldEchoThePart()
    {
        // Arrange
        var part = new Part
        {
            ToolCall = new ToolCall
            {
                Id = "call-1",
                ToolName = "google_search",
                ToolType = ToolType.GoogleSearchWeb,
                Arguments = JsonSerializer.Deserialize<JsonElement>("""{"query":"weather in London"}"""),
            },
            ThoughtSignature = "signature",
        };

        var messages = new List<ChatMessage>
        {
            new(ChatRole.Assistant, [new ToolCallContent("call-1") { RawRepresentation = part }]),
        };

        // Act
        var request = MEAIToGeminiMapper.CreateMappedGenerateContentRequest("", messages, new ChatOptions());

        // Assert
        var parts = Assert.Single(request.Contents).Parts;
        Assert.NotNull(parts);
        Assert.Same(part, Assert.Single(parts));
    }

    [Fact]
    public void CreateMappedGenerateContentRequest_WithAToolResultCarryingItsPart_ShouldEchoThePart()
    {
        // Arrange
        var part = new Part
        {
            ToolResponse = new ToolResponse
            {
                Id = "call-1",
                ToolType = ToolType.GoogleSearchWeb,
                Response = JsonSerializer.Deserialize<JsonElement>("""{"results":["18C and raining"]}"""),
            },
            ThoughtSignature = "signature",
        };

        var messages = new List<ChatMessage>
        {
            new(ChatRole.Assistant, [new ToolResultContent("call-1") { RawRepresentation = part }]),
        };

        // Act
        var request = MEAIToGeminiMapper.CreateMappedGenerateContentRequest("", messages, new ChatOptions());

        // Assert
        var parts = Assert.Single(request.Contents).Parts;
        Assert.NotNull(parts);
        Assert.Same(part, Assert.Single(parts));
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void CreateMappedGenerateContentRequest_WithAToolCallCarryingOnlyProperties_ShouldRebuildThePart(
        bool roundTripThroughJson)
    {
        // Arrange — RawRepresentation does not survive serialization, so a caller who persisted the
        // history as JSON arrives with only the additional properties, and with each value as a
        // JsonElement.
        var arguments = JsonSerializer.Deserialize<JsonElement>("""{"query":"weather in London"}""");

        var properties = MaybeRoundTripThroughJson(
            new AdditionalPropertiesDictionary
            {
                [GeminiContentProperties.Id] = "call-1",
                [GeminiContentProperties.ToolType] = ToolType.GoogleSearchWeb,
                [GeminiContentProperties.ToolName] = "google_search",
                [GeminiContentProperties.Arguments] = arguments,
                [GeminiContentProperties.ThoughtSignature] = "signature",
            },
            roundTripThroughJson);

        var messages = new List<ChatMessage>
        {
            new(ChatRole.Assistant, [new ToolCallContent("call-1") { AdditionalProperties = properties }]),
        };

        // Act
        var request = MEAIToGeminiMapper.CreateMappedGenerateContentRequest("", messages, new ChatOptions());

        // Assert
        var parts = Assert.Single(request.Contents).Parts;
        Assert.NotNull(parts);
        var part = Assert.Single(parts);
        Assert.Equal("signature", part.ThoughtSignature);
        Assert.NotNull(part.ToolCall);
        Assert.Equal("call-1", part.ToolCall.Id);
        Assert.Equal("google_search", part.ToolCall.ToolName);
        Assert.Equal(ToolType.GoogleSearchWeb, part.ToolCall.ToolType);
        Assert.Equal(arguments.GetRawText(), part.ToolCall.Arguments.GetRawText());
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void CreateMappedGenerateContentRequest_WithAToolResultCarryingOnlyProperties_ShouldRebuildThePart(
        bool roundTripThroughJson)
    {
        // Arrange
        var toolResponse = JsonSerializer.Deserialize<JsonElement>("""{"results":["18C and raining"]}""");

        var properties = MaybeRoundTripThroughJson(
            new AdditionalPropertiesDictionary
            {
                [GeminiContentProperties.Id] = "call-1",
                [GeminiContentProperties.ToolType] = ToolType.GoogleSearchWeb,
                [GeminiContentProperties.Response] = toolResponse,
                [GeminiContentProperties.ThoughtSignature] = "signature",
            },
            roundTripThroughJson);

        var messages = new List<ChatMessage>
        {
            new(ChatRole.Assistant, [new ToolResultContent("call-1") { AdditionalProperties = properties }]),
        };

        // Act
        var request = MEAIToGeminiMapper.CreateMappedGenerateContentRequest("", messages, new ChatOptions());

        // Assert
        var parts = Assert.Single(request.Contents).Parts;
        Assert.NotNull(parts);
        var part = Assert.Single(parts);
        Assert.Equal("signature", part.ThoughtSignature);
        Assert.NotNull(part.ToolResponse);
        Assert.Equal("call-1", part.ToolResponse.Id);
        Assert.Equal(ToolType.GoogleSearchWeb, part.ToolResponse.ToolType);
        Assert.Equal(toolResponse.GetRawText(), part.ToolResponse.Response.GetRawText());
    }

    [Fact]
    public void CreateMappedGenerateContentRequest_WithARebuiltIdLessToolInvocation_ShouldSendNoId()
    {
        // Arrange — GeminiToMEAIMapper fills CallId in when Gemini issued no id, so rebuilding from it
        // would send back an id the server never handed out.
        var response = new GenerateContentResponse
        {
            Candidates =
            [
                new Candidate
                {
                    Content = new Content
                    {
                        Role = "model",
                        Parts =
                        [
                            new Part { ToolCall = new ToolCall { ToolType = ToolType.UrlContext } },
                            new Part { ToolResponse = new ToolResponse { ToolType = ToolType.UrlContext } },
                        ],
                    },
                },
            ],
        };

        var mapped = GeminiToMEAIMapper.CreateMappedChatResponse(response, DateTimeOffset.UtcNow);
        var message = Assert.Single(mapped.Messages);

        // Persisting the history as JSON loses the parts, leaving only the additional properties.
        foreach (var content in message.Contents)
        {
            content.RawRepresentation = null;
        }

        // Act
        var request = MEAIToGeminiMapper.CreateMappedGenerateContentRequest(
            "",
            mapped.Messages,
            new ChatOptions());

        // Assert
        var parts = Assert.Single(request.Contents).Parts;
        Assert.NotNull(parts);
        Assert.Null(Assert.IsType<Part>(parts[0]).ToolCall!.Id);
        Assert.Null(Assert.IsType<Part>(parts[1]).ToolResponse!.Id);
    }

    [Fact]
    public void CreateMappedGenerateContentRequest_WithAToolCallCarryingNoToolType_ShouldThrow()
    {
        // Arrange — ToolCall.toolType is required, and TOOL_TYPE_UNSPECIFIED is not a stand-in for it.
        var messages = new List<ChatMessage>
        {
            new(ChatRole.Assistant, [new ToolCallContent("call-1")]),
        };

        // Act
        void Act() => MEAIToGeminiMapper.CreateMappedGenerateContentRequest("", messages, new ChatOptions());

        // Assert
        Assert.Throws<GeminiMappingException>(Act);
    }

    [Fact]
    public void CreateMappedGenerateContentRequest_WithAToolCallCarryingAWrongTypedToolType_ShouldThrow()
    {
        // Arrange
        var messages = new List<ChatMessage>
        {
            new(ChatRole.Assistant,
            [
                new ToolCallContent("call-1")
                {
                    AdditionalProperties = new() { [GeminiContentProperties.ToolType] = 42 },
                },
            ]),
        };

        // Act
        void Act() => MEAIToGeminiMapper.CreateMappedGenerateContentRequest("", messages, new ChatOptions());

        // Assert
        Assert.Throws<GeminiMappingException>(Act);
    }

    [Fact]
    public void CreateMappedGenerateContentRequest_WithAnMcpServerToolCall_ShouldThrow()
    {
        // Arrange — McpServerToolCallContent derives from ToolCallContent, and this mapper does not
        // support it. It must be reported rather than sent as a server-side invocation Gemini never made.
        var messages = new List<ChatMessage>
        {
            new(ChatRole.Assistant, [new McpServerToolCallContent("call-1", "get_weather", "weather")]),
        };

        // Act
        void Act() => MEAIToGeminiMapper.CreateMappedGenerateContentRequest("", messages, new ChatOptions());

        // Assert
        Assert.Throws<GeminiMappingException>(Act);
    }

    [Fact]
    public void CreateMappedGenerateContentRequest_WithAMappedServerSideToolInvocation_ShouldRoundTripTheParts()
    {
        // Arrange
        var response = new GenerateContentResponse
        {
            Candidates =
            [
                new Candidate
                {
                    Content = new Content
                    {
                        Role = "model",
                        Parts =
                        [
                            new Part
                            {
                                ToolCall = new ToolCall
                                {
                                    Id = "call-1",
                                    ToolName = "google_search",
                                    ToolType = ToolType.GoogleSearchWeb,
                                    Arguments =
                                        JsonSerializer.Deserialize<JsonElement>("""{"query":"weather"}"""),
                                },
                                ThoughtSignature = "signature",
                            },
                            new Part
                            {
                                ToolResponse = new ToolResponse
                                {
                                    Id = "call-1",
                                    ToolType = ToolType.GoogleSearchWeb,
                                    Response =
                                        JsonSerializer.Deserialize<JsonElement>("""{"results":["18C"]}"""),
                                },
                            },
                        ],
                    },
                },
            ],
        };

        var expectedParts = response.Candidates[0].Content!.Parts;

        var mapped = GeminiToMEAIMapper.CreateMappedChatResponse(response, DateTimeOffset.UtcNow);

        // Act
        var request = MEAIToGeminiMapper.CreateMappedGenerateContentRequest(
            "",
            mapped.Messages,
            new ChatOptions());

        // Assert
        Assert.Equal(expectedParts, Assert.Single(request.Contents).Parts);
    }

    [Fact]
    public void CreateMappedGenerateContentRequest_WithAMappedCodeExecution_ShouldRoundTripTheParts()
    {
        // Arrange — the mapped contents still carry the parts on RawRepresentation, so the request
        // echoes them verbatim, thought signature included.
        var response = JsonSerializer.Deserialize<GenerateContentResponse>(CodeExecutionResponseWithIds)!;
        var expectedParts = response.Candidates![0].Content!.Parts;

        var mapped = GeminiToMEAIMapper.CreateMappedChatResponse(response, DateTimeOffset.UtcNow);

        // Act
        var request = MEAIToGeminiMapper.CreateMappedGenerateContentRequest("", mapped.Messages, new ChatOptions());

        // Assert
        Assert.Equal(expectedParts, Assert.Single(request.Contents).Parts);
    }

    [Fact]
    public void CreateMappedGenerateContentRequest_WithACodeExecutionPersistedAsJson_ShouldRebuildTheParts()
    {
        // Arrange — RawRepresentation does not survive serialization, so a caller who persisted the
        // history as JSON arrives with only Inputs, Outputs and the additional properties.
        var response = JsonSerializer.Deserialize<GenerateContentResponse>(CodeExecutionResponseWithIds)!;
        var mapped = GeminiToMEAIMapper.CreateMappedChatResponse(response, DateTimeOffset.UtcNow);

        var json = JsonSerializer.Serialize(mapped.Messages, GeminiJsonUtilities.DefaultOptions);
        var messages = JsonSerializer.Deserialize<List<ChatMessage>>(json, GeminiJsonUtilities.DefaultOptions)!;

        Assert.All(messages.SelectMany(m => m.Contents), c => Assert.Null(c.RawRepresentation));

        // Act
        var request = MEAIToGeminiMapper.CreateMappedGenerateContentRequest("", messages, new ChatOptions());

        // Assert
        var parts = Assert.Single(request.Contents).Parts;
        Assert.NotNull(parts);
        Assert.Equal(4, parts.Count);

        var executableCode = parts[1].ExecutableCode;
        Assert.NotNull(executableCode);
        Assert.Equal("call_318937", executableCode.Id);
        Assert.Equal(ExecutableCodeLanguage.Python, executableCode.Language);
        Assert.Equal("print(sum(range(1, 11)))", executableCode.Code);
        Assert.Equal("signature", parts[1].ThoughtSignature);

        var codeExecutionResult = parts[2].CodeExecutionResult;
        Assert.NotNull(codeExecutionResult);
        Assert.Equal("call_318937", codeExecutionResult.Id);
        Assert.Equal(CodeExecutionResultOutcome.Ok, codeExecutionResult.Outcome);
        Assert.Equal("55\n", codeExecutionResult.Output);
        Assert.Null(parts[2].ThoughtSignature);
    }

    [Fact]
    public void CreateMappedGenerateContentRequest_WithAStreamedCodeExecution_ShouldRoundTripTheParts()
    {
        // Arrange — a streamed turn delivers each part in its own chunk, so the aggregated response holds
        // one content per part with the chunk's Part still on RawRepresentation. The parts sent back must
        // be the ones the same turn produces unstreamed, with the split prose rejoined.
        var expectedParts = JsonSerializer.Deserialize<GenerateContentResponse>(CodeExecutionResponseWithIds)!
            .Candidates![0].Content!.Parts;

        var streamed = CreateStreamedCodeExecutionResponse();

        // Act
        var request = MEAIToGeminiMapper.CreateMappedGenerateContentRequest("", streamed.Messages, new ChatOptions());

        // Assert
        Assert.Equal(expectedParts, Assert.Single(request.Contents).Parts);
    }

    [Fact]
    public void CreateMappedGenerateContentRequest_WithAStreamedCodeExecutionPersistedAsJson_ShouldRebuildTheParts()
    {
        // Arrange — aggregating the stream keeps the additional properties the rebuild reads, so a caller
        // who persisted the streamed history as JSON sends the same parts as one who persisted an
        // unstreamed one.
        var expectedParts = JsonSerializer.Deserialize<GenerateContentResponse>(CodeExecutionResponseWithIds)!
            .Candidates![0].Content!.Parts;

        var streamed = CreateStreamedCodeExecutionResponse();

        var json = JsonSerializer.Serialize(streamed.Messages, GeminiJsonUtilities.DefaultOptions);
        var messages = JsonSerializer.Deserialize<List<ChatMessage>>(json, GeminiJsonUtilities.DefaultOptions)!;

        Assert.All(messages.SelectMany(m => m.Contents), c => Assert.Null(c.RawRepresentation));

        // Act
        var request = MEAIToGeminiMapper.CreateMappedGenerateContentRequest("", messages, new ChatOptions());

        // Assert
        Assert.Equal(expectedParts, Assert.Single(request.Contents).Parts);
    }

    [Theory]
    [InlineData(CodeExecutionResultOutcome.Ok)]
    [InlineData(CodeExecutionResultOutcome.Failed)]
    [InlineData(CodeExecutionResultOutcome.DeadlineExceeded)]
    public void CreateMappedGenerateContentRequest_WithAPersistedOutcome_ShouldRebuildIt(
        CodeExecutionResultOutcome outcome)
    {
        // Arrange
        var properties = MaybeRoundTripThroughJson(
            new AdditionalPropertiesDictionary { [GeminiContentProperties.Outcome] = outcome },
            roundTrip: true);

        var messages = new List<ChatMessage>
        {
            new(ChatRole.Assistant,
            [
                new CodeInterpreterToolResultContent("call-1")
                {
                    Outputs = [new TextContent("out")], AdditionalProperties = properties,
                },
            ]),
        };

        // Act
        var request = MEAIToGeminiMapper.CreateMappedGenerateContentRequest("", messages, new ChatOptions());

        // Assert
        var part = Assert.Single(Assert.Single(request.Contents).Parts!);
        Assert.Equal(outcome, part.CodeExecutionResult!.Outcome);
    }

    [Fact]
    public void CreateMappedGenerateContentRequest_WithACodeExecutionResultCarryingNoOutcome_ShouldSendUnspecified()
    {
        // Arrange — Gemini accepts an unspecified outcome on an echoed part (probed 2026-09-01).
        var messages = new List<ChatMessage>
        {
            new(ChatRole.Assistant, [new CodeInterpreterToolResultContent("call-1") { Outputs = [new TextContent("1")] }]),
        };

        // Act
        var request = MEAIToGeminiMapper.CreateMappedGenerateContentRequest("", messages, new ChatOptions());

        // Assert
        var part = Assert.Single(Assert.Single(request.Contents).Parts!);
        Assert.Equal(CodeExecutionResultOutcome.Unspecified, part.CodeExecutionResult!.Outcome);
        Assert.Equal("1", part.CodeExecutionResult.Output);
    }

    [Fact]
    public void CreateMappedGenerateContentRequest_WithACodeExecutionResultCarryingNoOutputs_ShouldSendNoOutput()
    {
        // Arrange
        var messages = new List<ChatMessage>
        {
            new(ChatRole.Assistant, [new CodeInterpreterToolResultContent("call-1")]),
        };

        // Act
        var request = MEAIToGeminiMapper.CreateMappedGenerateContentRequest("", messages, new ChatOptions());

        // Assert
        var part = Assert.Single(Assert.Single(request.Contents).Parts!);
        Assert.Null(part.CodeExecutionResult!.Output);
    }

    [Fact]
    public void CreateMappedGenerateContentRequest_WithACodeInterpreterCallCarryingNoCode_ShouldThrow()
    {
        // Arrange — ExecutableCode.code is required.
        var messages = new List<ChatMessage>
        {
            new(ChatRole.Assistant, [new CodeInterpreterToolCallContent("call-1")]),
        };

        // Act
        void Act() => MEAIToGeminiMapper.CreateMappedGenerateContentRequest("", messages, new ChatOptions());

        // Assert
        var exception = Assert.Throws<GeminiMappingException>(Act);
        Assert.Contains(nameof(CodeInterpreterToolCallContent.Inputs), exception.Message);
    }

    [Fact]
    public void CreateMappedGenerateContentRequest_WithACodeInterpreterCallCarryingTwoCodeInputs_ShouldThrow()
    {
        // Arrange — two code strings cannot be joined without guessing the separator, so unlike two
        // output strings they are reported rather than concatenated.
        var messages = new List<ChatMessage>
        {
            new(ChatRole.Assistant,
            [
                new CodeInterpreterToolCallContent("call-1")
                {
                    Inputs = [new TextContent("import sys"), new TextContent("print(1)")],
                },
            ]),
        };

        // Act
        void Act() => MEAIToGeminiMapper.CreateMappedGenerateContentRequest("", messages, new ChatOptions());

        // Assert
        var exception = Assert.Throws<GeminiMappingException>(Act);
        Assert.Contains("more than one", exception.Message);
    }

    [Fact]
    public void CreateMappedGenerateContentRequest_WithCodeExecutionContentsCarryingParts_ShouldEchoThePartsOverTheirInputsAndOutputs()
    {
        // Arrange — the part Gemini sent is the one it needs back, whatever a consumer has since put in
        // Inputs and Outputs, and it may hold fields the rebuild has no key for.
        var executableCodePart = new Part
        {
            ExecutableCode = new ExecutableCode { Language = ExecutableCodeLanguage.Unspecified, Code = "x" },
        };

        var codeExecutionResultPart = new Part
        {
            CodeExecutionResult = new CodeExecutionResult { Outcome = CodeExecutionResultOutcome.Failed, Output = "x" },
        };

        var messages = new List<ChatMessage>
        {
            new(ChatRole.Assistant,
            [
                new CodeInterpreterToolCallContent("call-1")
                {
                    Inputs = [new TextContent("y")], RawRepresentation = executableCodePart,
                },
                new CodeInterpreterToolResultContent("call-1")
                {
                    Outputs = [new TextContent("y")], RawRepresentation = codeExecutionResultPart,
                },
            ]),
        };

        // Act
        var request = MEAIToGeminiMapper.CreateMappedGenerateContentRequest("", messages, new ChatOptions());

        // Assert
        var parts = Assert.Single(request.Contents).Parts;
        Assert.NotNull(parts);
        Assert.Equal([executableCodePart, codeExecutionResultPart], parts);
    }

    [Fact]
    public void CreateMappedGenerateContentRequest_WithACodeInterpreterCallCarryingAFile_ShouldThrow()
    {
        // Arrange — an executableCode part has nowhere to put a file, and a turn that succeeds while the
        // model never sees it is worse than one that fails.
        var messages = new List<ChatMessage>
        {
            new(ChatRole.Assistant,
            [
                new CodeInterpreterToolCallContent("call-1")
                {
                    Inputs = [new TextContent("print(1)"), new HostedFileContent("files/abc")],
                },
            ]),
        };

        // Act
        void Act() => MEAIToGeminiMapper.CreateMappedGenerateContentRequest("", messages, new ChatOptions());

        // Assert
        var exception = Assert.Throws<GeminiMappingException>(Act);
        Assert.Contains(typeof(HostedFileContent).ToString(), exception.Message);
    }

    [Fact]
    public void CreateMappedGenerateContentRequest_WithACodeExecutionResultCarryingData_ShouldThrow()
    {
        // Arrange — a codeExecutionResult part carries only an output string.
        var messages = new List<ChatMessage>
        {
            new(ChatRole.Assistant,
            [
                new CodeInterpreterToolResultContent("call-1")
                {
                    Outputs = [new DataContent(new byte[] { 1, 2, 3 }, "image/png")],
                },
            ]),
        };

        // Act
        void Act() => MEAIToGeminiMapper.CreateMappedGenerateContentRequest("", messages, new ChatOptions());

        // Assert
        var exception = Assert.Throws<GeminiMappingException>(Act);
        Assert.Contains(typeof(DataContent).ToString(), exception.Message);
    }

    [Fact]
    public void CreateMappedGenerateContentRequest_WithTwoRebuiltCodeExecutions_ShouldMapFourPartsInOrder()
    {
        // Arrange
        var messages = new List<ChatMessage>
        {
            new(ChatRole.Assistant,
            [
                new CodeInterpreterToolCallContent("call-1")
                {
                    Inputs = [new DataContent("data:text/x-python;base64,YQ==")],
                },
                new CodeInterpreterToolResultContent("call-1") { Outputs = [new TextContent("1")] },
                new CodeInterpreterToolCallContent("call-2") { Inputs = [new TextContent("b")] },
                new CodeInterpreterToolResultContent("call-2") { Outputs = [new TextContent("2"), new TextContent("3")] },
            ]),
        };

        // Act
        var request = MEAIToGeminiMapper.CreateMappedGenerateContentRequest("", messages, new ChatOptions());

        // Assert
        var parts = Assert.Single(request.Contents).Parts;
        Assert.NotNull(parts);
        Assert.Equal(4, parts.Count);
        Assert.Equal("a", parts[0].ExecutableCode!.Code);
        Assert.Equal("1", parts[1].CodeExecutionResult!.Output);
        Assert.Equal("b", parts[2].ExecutableCode!.Code);
        Assert.Equal("23", parts[3].CodeExecutionResult!.Output);
    }

    [StringSyntax(StringSyntaxAttribute.Json)]
    private const string CodeExecutionResponseWithIds =
        """
        {
          "candidates": [
            {
              "content": {
                "parts": [
                  { "text": "I will sum the numbers with Python." },
                  {
                    "executableCode": { "id": "call_318937", "language": "PYTHON", "code": "print(sum(range(1, 11)))" },
                    "thoughtSignature": "signature"
                  },
                  {
                    "codeExecutionResult": { "id": "call_318937", "outcome": "OUTCOME_OK", "output": "55\n" }
                  },
                  { "text": "The sum is 55." }
                ],
                "role": "model"
              },
              "finishReason": "STOP"
            }
          ],
          "modelVersion": "gemini-3.1-flash-lite",
          "responseId": "test-code-execution-ids"
        }
        """;

    /// <summary>
    /// The same turn as <see cref="CodeExecutionResponseWithIds"/> as Gemini streams it: each part whole
    /// in its own chunk, the prose split across chunk boundaries, and the call and its result correlated
    /// by a shared id rather than by adjacency.
    /// </summary>
    private const string StreamedCodeExecutionChunks =
        """
        [
          {
            "candidates": [
              { "content": { "parts": [{ "text": "I will sum the numbers " }], "role": "model" } }
            ],
            "modelVersion": "gemini-3.1-flash-lite",
            "responseId": "test-streamed-code-execution"
          },
          {
            "candidates": [
              { "content": { "parts": [{ "text": "with Python." }], "role": "model" } }
            ],
            "modelVersion": "gemini-3.1-flash-lite",
            "responseId": "test-streamed-code-execution"
          },
          {
            "candidates": [
              {
                "content": {
                  "parts": [
                    {
                      "executableCode": { "id": "call_318937", "language": "PYTHON", "code": "print(sum(range(1, 11)))" },
                      "thoughtSignature": "signature"
                    }
                  ],
                  "role": "model"
                }
              }
            ],
            "modelVersion": "gemini-3.1-flash-lite",
            "responseId": "test-streamed-code-execution"
          },
          {
            "candidates": [
              {
                "content": {
                  "parts": [
                    { "codeExecutionResult": { "id": "call_318937", "outcome": "OUTCOME_OK", "output": "55\n" } }
                  ],
                  "role": "model"
                }
              }
            ],
            "modelVersion": "gemini-3.1-flash-lite",
            "responseId": "test-streamed-code-execution"
          },
          {
            "candidates": [
              {
                "content": { "parts": [{ "text": "The sum is 55." }], "role": "model" },
                "finishReason": "STOP"
              }
            ],
            "modelVersion": "gemini-3.1-flash-lite",
            "responseId": "test-streamed-code-execution"
          }
        ]
        """;

    private static ChatResponse CreateStreamedCodeExecutionResponse()
    {
        var chunks = JsonSerializer.Deserialize<List<GenerateContentResponse>>(StreamedCodeExecutionChunks)!;
        var state = new CandidateMappingState();

        return chunks
            .Select(chunk => GeminiToMEAIMapper.CreateMappedChatResponseUpdate(chunk, state, DateTimeOffset.UtcNow))
            .ToChatResponse();
    }

    private static AdditionalPropertiesDictionary MaybeRoundTripThroughJson(
        AdditionalPropertiesDictionary properties,
        bool roundTrip)
    {
        if (!roundTrip)
        {
            return properties;
        }

        var json = JsonSerializer.Serialize(properties);
        return JsonSerializer.Deserialize<AdditionalPropertiesDictionary>(json)!;
    }

    private sealed class UnsupportedTool : AITool;
}

using GeminiDotnet.V1Beta;
using GeminiDotnet.V1Beta.Models;
using System.Diagnostics.CodeAnalysis;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using MEAI = Microsoft.Extensions.AI;

namespace GeminiDotnet.Extensions.AI;

internal static class MEAIToGeminiMapper
{
    public static GenerateContentRequest CreateMappedGenerateContentRequest(
        string model,
        IEnumerable<MEAI.ChatMessage> chatMessages,
        MEAI.ChatOptions? options,
        GenerateContentRequest? rawRepresentation = null)
    {
        Content? systemInstruction = null;
        List<Content>? contents = null;

        if (rawRepresentation?.Contents is null)
        {
            contents = chatMessages.TryGetNonEnumeratedCount(out var count)
                ? new List<Content>(count)
                : [];

            List<Part> systemInstructionParts = options?.Instructions is { Length: > 0 } instructions
                ? [new Part { Text = instructions }]
                : [];

            foreach (var message in chatMessages)
            {
                if (message.Role == MEAI.ChatRole.System)
                {
                    AppendSystemInstructionParts(message, systemInstructionParts);
                    continue;
                }

                contents.Add(CreateMappedContent(message));
            }

            systemInstruction = systemInstructionParts.Count > 0
                ? new Content { Role = null, Parts = systemInstructionParts }
                : null;

            // In Gemini's API, files for code execution are passed as Parts in
            // the user Content — not in the tool configuration. When
            // HostedCodeInterpreterTool.Inputs has items, inject those as
            // additional Parts into the last user Content.
            //
            // This is intentionally skipped when rawRepresentation provides its
            // own Contents, as the caller has full control in that case.
            InjectToolInputParts(options, contents);

            // A message whose contents all mapped away (an empty text carrier for citations, or web
            // search content synthesized during response mapping) leaves a Content with no parts, which
            // the API rejects. Done after injection so an empty user message can still host tool inputs.
            contents.RemoveAll(static content => content.Parts is not { Count: > 0 });
        }

        var tools = rawRepresentation?.Tools ?? CreateMappedTools(options?.Tools);

        return new GenerateContentRequest
        {
            Model = rawRepresentation?.Model ?? model,
            SystemInstruction = rawRepresentation?.SystemInstruction ?? systemInstruction,
            GenerationConfiguration =
                rawRepresentation?.GenerationConfiguration ?? CreateMappedGenerationConfiguration(options),
            CachedContent = rawRepresentation?.CachedContent,
            Contents = rawRepresentation?.Contents ?? contents!,
            Tools = tools,
            ToolConfiguration = rawRepresentation?.ToolConfiguration ?? CreateMappedToolConfiguration(options, tools),
            SafetySettings = rawRepresentation?.SafetySettings,
        };

        static IReadOnlyList<Tool>? CreateMappedTools(IList<MEAI.AITool>? tools)
        {
            if (tools is null)
            {
                return null;
            }

            List<Tool> mappedTools = new(tools.Count);
            List<FunctionDeclaration>? functionDeclarations = null;
            List<McpServer>? mcpServers = null;
            MEAI.AITool? builtInTool = null;

            foreach (var tool in tools)
            {
                switch (tool)
                {
                    case MEAI.AIFunctionDeclaration declaration:
                        functionDeclarations ??= [];

                        functionDeclarations.Add(new FunctionDeclaration
                        {
                            Name = declaration.Name,
                            Description = declaration.Description,
                            ParametersJsonSchema = declaration.JsonSchema,
                            ResponseJsonSchema = declaration.ReturnJsonSchema ?? default,
                        });

                        break;
                    case MEAI.HostedCodeInterpreterTool:
                        builtInTool = tool;
                        mappedTools.Add(new Tool { CodeExecution = new CodeExecution() });
                        break;
                    case MEAI.HostedWebSearchTool:
                        builtInTool = tool;
                        mappedTools.Add(new Tool { GoogleSearch = new GoogleSearch() });
                        break;
                    case MEAI.HostedFileSearchTool fileSearchTool:
                        builtInTool = tool;
                        mappedTools.Add(new Tool { FileSearch = CreateMappedFileSearch(fileSearchTool) });
                        break;
                    case MEAI.HostedMcpServerTool mcpServerTool:
                        mcpServers ??= [];
                        mcpServers.Add(CreateMappedMcpServer(mcpServerTool));
                        break;
                    default:
                        GeminiMappingException.Throw(
                            fromPropertyName: $"{typeof(MEAI.AITool)}",
                            toPropertyName: $"{typeof(Tool)}",
                            reason: $"Unsupported tool type: {tool.GetType()}");

                        break;
                }
            }

            if (mcpServers is not null)
            {
                if (builtInTool is not null)
                {
                    // Gemini expands each MCP server into synthetic function declarations, and rejects
                    // those alongside a built-in tool unless toolConfig.includeServerSideToolInvocations
                    // is set — which in turn returns an MCP invocation as {args, id, toolName}, a shape
                    // the generated ToolCall rejects because it declares toolType required.
                    GeminiMappingException.Throw(
                        fromPropertyName: $"{typeof(MEAI.ChatOptions)}.{nameof(MEAI.ChatOptions.Tools)}",
                        toPropertyName: $"{typeof(Tool)}.{nameof(Tool.McpServers)}",
                        reason:
                        $"Gemini rejects a request that combines MCP servers with a built-in tool, so {typeof(MEAI.HostedMcpServerTool)} cannot be used alongside {builtInTool.GetType()}.");
                }

                mappedTools.Add(new Tool { McpServers = mcpServers });
            }

            if (functionDeclarations is not null)
            {
                mappedTools.Add(new Tool { FunctionDeclarations = functionDeclarations });
            }

            return mappedTools;
        }

        static GenerationConfiguration? CreateMappedGenerationConfiguration(MEAI.ChatOptions? options)
        {
            if (options is null)
            {
                return null;
            }

#pragma warning disable CS0618 // Type or member is obsolete
            var thinkingConfiguration = options.AdditionalProperties?.GetValueOrDefault<ThinkingConfiguration>(
                GeminiAdditionalProperties.ThinkingConfiguration)
#pragma warning restore CS0618 // Type or member is obsolete
                ?? CreateMappedThinkingConfiguration(options.Reasoning);

            var responseModalities = options.AdditionalProperties?
                    .GetValueOrDefault<IEnumerable<ResponseModality>>(GeminiAdditionalProperties.ResponseModalities)
                switch
                {
                    IReadOnlyList<ResponseModality> list => list,
                    IEnumerable<ResponseModality> enumerable => enumerable.ToList(),
                    _ => null
                };

            var imageConfiguration = options.AdditionalProperties?.GetValueOrDefault<ImageConfiguration>(
                GeminiAdditionalProperties.ImageConfiguration);

            var configuration = new GenerationConfiguration
            {
                StopSequences = options.StopSequences is null ? null : [.. options.StopSequences],
                ResponseMimeType = CreateMappedResponseMimeType(options.ResponseFormat),
                ResponseJsonSchema = CreateMappedResponseSchema(options.ResponseFormat),
                ResponseModalities = responseModalities,
                ImageConfiguration = imageConfiguration,
                CandidateCount = null,
                MaxOutputTokens = options.MaxOutputTokens,
                Temperature = options.Temperature,
                TopP = options.TopP,
                TopK = options.TopK,
                Seed = (int?)options.Seed, // TODO: can we support long seeds?
                PresencePenalty = options.PresencePenalty,
                FrequencyPenalty = options.FrequencyPenalty,
                ResponseLogprobs = null,
                Logprobs = null,
                EnableEnhancedCivicAnswers = null,
                SpeechConfiguration = null,
                ThinkingConfiguration = thinkingConfiguration
            };


            return configuration;
        }

        static JsonElement CreateMappedResponseSchema(MEAI.ChatResponseFormat? responseFormat)
        {
            if (responseFormat is null or MEAI.ChatResponseFormatText)
            {
                return default;
            }

            if (responseFormat is MEAI.ChatResponseFormatJson { Schema: JsonElement schema })
            {
                return schema;
            }

            GeminiMappingException.Throw(
                fromPropertyName: $"{typeof(MEAI.ChatOptions)}.{nameof(MEAI.ChatOptions.ResponseFormat)}",
                toPropertyName: $"{typeof(GenerationConfiguration)}.{nameof(GenerationConfiguration.ResponseJsonSchema)}",
                reason: $"Unsupported {typeof(MEAI.ChatResponseFormat)}: '{responseFormat}'");

            return default; // unreachable
        }

        Content CreateMappedContent(MEAI.ChatMessage chatMessage)
        {
            return new Content
            {
                Role = CreateMappedRole(chatMessage.Role), Parts = CreateMappedParts(chatMessage.Contents),
            };
        }

        static string? CreateMappedRole(MEAI.ChatRole role)
        {
            if (role == MEAI.ChatRole.System)
            {
                return null;
            }

            if (role == MEAI.ChatRole.User)
            {
                return ChatRoles.User;
            }

            if (role == MEAI.ChatRole.Assistant || role == MEAI.ChatRole.Tool)
            {
                return ChatRoles.Model;
            }

            GeminiMappingException.Throw(
                fromPropertyName: $"{typeof(MEAI.ChatMessage)}.{nameof(MEAI.ChatMessage.Role)}",
                toPropertyName: $"{typeof(Content)}.{nameof(Content.Role)}",
                reason: $"Unsupported {typeof(MEAI.ChatRole)}: '{role}'");

            return null!; // unreachable
        }

        IReadOnlyList<Part> CreateMappedParts(IList<MEAI.AIContent> contents)
        {
            List<Part> parts = new(contents.Count);

            foreach (var content in contents)
            {
                // Web search content is synthesized from GroundingMetadata during response
                // mapping and has no corresponding Gemini Part representation.
                if (content is MEAI.WebSearchToolCallContent or MEAI.WebSearchToolResultContent)
                    continue;

                // Empty text carries nothing, and a null one leaves a part with no field set, which
                // the API rejects. Response mapping produces an empty text content as a carrier for
                // citations that ground no span, so a response fed back as history would otherwise fail.
                if (content is MEAI.TextContent { Text: null or "" })
                    continue;

                var mapped = content switch
                {
                    MEAI.TextContent textContent => CreateTextPart(textContent),
                    MEAI.TextReasoningContent textReasoningContent => CreateTextReasoningPart(textReasoningContent),
                    MEAI.DataContent dataContent => CreateInlineDataPart(dataContent),
                    MEAI.UriContent uriContent => CreateFileDataPart(uriContent),
                    MEAI.HostedFileContent fileContent => CreateHostedFileDataPart(fileContent),
                    MEAI.FunctionCallContent functionCall => CreateFunctionCallPart(functionCall),
                    MEAI.FunctionResultContent functionResult => CreateFunctionResponsePart(functionResult),
                    // Every hosted-tool content type derives from ToolCallContent or ToolResultContent,
                    // so these arms match the exact type: a pattern on the base type would swallow the
                    // subclasses this mapper does not support and send Gemini a wrong part instead of
                    // reporting them below.
                    MEAI.ToolCallContent toolCall when toolCall.GetType() == typeof(MEAI.ToolCallContent) =>
                        CreateToolCallPart(toolCall),
                    MEAI.ToolResultContent toolResult when toolResult.GetType() == typeof(MEAI.ToolResultContent) =>
                        CreateToolResponsePart(toolResult),
                    // Both code-interpreter types are sealed, so no exact-type guard is needed.
                    MEAI.CodeInterpreterToolCallContent codeCall => CreateExecutableCodePart(codeCall),
                    MEAI.CodeInterpreterToolResultContent codeResult => CreateCodeExecutionResultPart(codeResult),
                    _ => ThrowUnsupportedContentException(content),
                };

                parts.Add(mapped);
            }

            return parts;

            [DoesNotReturn]
            static Part ThrowUnsupportedContentException(MEAI.AIContent content)
            {
                GeminiMappingException.Throw(
                    fromPropertyName: content.GetType().ToString(),
                    toPropertyName: $"{typeof(Part)}",
                    reason: $"Unsupported {typeof(MEAI.AIContent)} type: {content.GetType()}");

                return null!; // unreachable
            }

            static Part CreateTextPart(MEAI.TextContent textContent)
            {
                return new Part { Text = textContent.Text };
            }

            static Part CreateInlineDataPart(MEAI.DataContent dataContent)
            {
                if (dataContent.Data.IsEmpty)
                {
                    GeminiMappingException.Throw(
                        fromPropertyName: $"{typeof(MEAI.DataContent)}.{nameof(MEAI.DataContent.Data)}",
                        toPropertyName: $"{typeof(Part)}.{nameof(Part.InlineData)}",
                        reason:
                        $"{nameof(MEAI.DataContent.Data)} cannot be empty when creating an {nameof(Part.InlineData)} part.");
                }

                return new Part
                {
                    InlineData = new Blob { Data = dataContent.Data, MimeType = dataContent.MediaType },
                    ThoughtSignature = GetThoughtSignature(dataContent),
                };
            }


            static Part CreateFileDataPart(MEAI.UriContent uriContent)
            {
                return new Part
                {
                    FileData = new FileData
                    {
                        FileUri = uriContent.Uri.ToString(), MimeType = uriContent.MediaType,
                    },
                    ThoughtSignature = GetThoughtSignature(uriContent),
                };
            }

            static Part CreateFunctionCallPart(MEAI.FunctionCallContent functionCall)
            {
                JsonElement arguments = JsonSerializer.SerializeToElement(
                    functionCall.Arguments,
                    JsonContext.Default.IDictionaryStringObject);

                return new Part
                {
                    FunctionCall = new FunctionCall
                    {
                        Id = functionCall.CallId, Name = functionCall.Name, Arguments = arguments
                    },
                    ThoughtSignature = GetThoughtSignature(functionCall)
                };
            }

            Part CreateFunctionResponsePart(MEAI.FunctionResultContent functionResult)
            {
                var response = functionResult.Exception is null
                    ? new Dictionary<string, object?> { { "result", functionResult.Result } }
                    : new Dictionary<string, object?> { { "error", functionResult.Result }, };

                // Gemini's FunctionResponse.Name requires the function name, but
                // FunctionResultContent only carries CallId. Resolve the name by
                // finding the matching FunctionCallContent in the conversation.
                var functionName = ResolveFunctionName(chatMessages, functionResult.CallId)
                    ?? functionResult.CallId;

                return new Part
                {
                    FunctionResponse = new FunctionResponse
                    {
                        Id = functionResult.CallId,
                        Name = functionName,
                        Response = JsonSerializer.SerializeToElement(response,
                            JsonContext.Default.IDictionaryStringObject)
                    },
                    ThoughtSignature = GetThoughtSignature(functionResult)
                };
            }

            static Part CreateToolCallPart(MEAI.ToolCallContent toolCall)
            {
                // Gemini requires a server-side invocation echoed back unchanged, so prefer the part it
                // sent: it carries the thought signature and any field a future spec revision adds.
                if (toolCall.RawRepresentation is Part { ToolCall: not null } part)
                {
                    return part;
                }

                // RawRepresentation does not survive serialization, so a caller who persisted the
                // history as JSON arrives here with only the properties below.
                MEAI.AdditionalPropertiesDictionary properties = toolCall.AdditionalProperties ?? [];

                var fromPropertyName =
                    $"{typeof(MEAI.ToolCallContent)}.{nameof(MEAI.AIContent.AdditionalProperties)}";
                var toPropertyName = $"{typeof(Part)}.{nameof(Part.ToolCall)}";

                return new Part
                {
                    ToolCall = new ToolCall
                    {
                        // Not CallId: that is filled in when Gemini issued no id, and an id the server
                        // never handed out is not an invocation echoed back unchanged.
                        Id = properties.GetValueOrThrow<string>(
                            GeminiContentProperties.Id, fromPropertyName, toPropertyName),
                        ToolName = properties.GetValueOrThrow<string>(
                            GeminiContentProperties.ToolName, fromPropertyName, toPropertyName),
                        Arguments = properties.GetValueOrThrow<JsonElement>(
                            GeminiContentProperties.Arguments, fromPropertyName, toPropertyName),
                        ToolType = GetRequiredToolType(properties, fromPropertyName, toPropertyName),
                    },
                    ThoughtSignature = properties.GetValueOrThrow<string>(
                        GeminiContentProperties.ThoughtSignature, fromPropertyName, toPropertyName),
                };
            }

            static Part CreateToolResponsePart(MEAI.ToolResultContent toolResult)
            {
                if (toolResult.RawRepresentation is Part { ToolResponse: not null } part)
                {
                    return part;
                }

                MEAI.AdditionalPropertiesDictionary properties = toolResult.AdditionalProperties ?? [];

                var fromPropertyName =
                    $"{typeof(MEAI.ToolResultContent)}.{nameof(MEAI.AIContent.AdditionalProperties)}";
                var toPropertyName = $"{typeof(Part)}.{nameof(Part.ToolResponse)}";

                return new Part
                {
                    ToolResponse = new ToolResponse
                    {
                        Id = properties.GetValueOrThrow<string>(
                            GeminiContentProperties.Id, fromPropertyName, toPropertyName),
                        Response = properties.GetValueOrThrow<JsonElement>(
                            GeminiContentProperties.Response, fromPropertyName, toPropertyName),
                        ToolType = GetRequiredToolType(properties, fromPropertyName, toPropertyName),
                    },
                    ThoughtSignature = properties.GetValueOrThrow<string>(
                        GeminiContentProperties.ThoughtSignature, fromPropertyName, toPropertyName),
                };
            }

            static Part CreateExecutableCodePart(MEAI.CodeInterpreterToolCallContent codeCall)
            {
                // Gemini needs the code it ran echoed back, with the thought signature that led to it,
                // so prefer the part it sent.
                if (codeCall.RawRepresentation is Part { ExecutableCode: not null } part)
                {
                    return part;
                }

                // RawRepresentation does not survive serialization, so a caller who persisted the
                // history as JSON arrives here with only Inputs and the additional properties.
                MEAI.AdditionalPropertiesDictionary properties = codeCall.AdditionalProperties ?? [];

                var fromPropertyName =
                    $"{typeof(MEAI.CodeInterpreterToolCallContent)}.{nameof(MEAI.AIContent.AdditionalProperties)}";
                var toPropertyName = $"{typeof(Part)}.{nameof(Part.ExecutableCode)}";

                return new Part
                {
                    ExecutableCode = new ExecutableCode
                    {
                        Code = GetExecutableCode(codeCall.Inputs),
                        // Python is the only language the tool runs and the spec calls it the default, so
                        // nothing records the language on the way out and nothing reads it back here.
                        Language = ExecutableCodeLanguage.Python,
                        // Not CallId: that is filled in when Gemini issued no id, and an id the server
                        // never handed out is not a part echoed back unchanged.
                        Id = properties.GetValueOrThrow<string>(
                            GeminiContentProperties.Id, fromPropertyName, toPropertyName),
                    },
                    ThoughtSignature = properties.GetValueOrThrow<string>(
                        GeminiContentProperties.ThoughtSignature, fromPropertyName, toPropertyName),
                };
            }

            static Part CreateCodeExecutionResultPart(MEAI.CodeInterpreterToolResultContent codeResult)
            {
                if (codeResult.RawRepresentation is Part { CodeExecutionResult: not null } part)
                {
                    return part;
                }

                MEAI.AdditionalPropertiesDictionary properties = codeResult.AdditionalProperties ?? [];

                var fromPropertyName =
                    $"{typeof(MEAI.CodeInterpreterToolResultContent)}.{nameof(MEAI.AIContent.AdditionalProperties)}";
                var toPropertyName = $"{typeof(Part)}.{nameof(Part.CodeExecutionResult)}";

                return new Part
                {
                    CodeExecutionResult = new CodeExecutionResult
                    {
                        Id = properties.GetValueOrThrow<string>(
                            GeminiContentProperties.Id, fromPropertyName, toPropertyName),
                        // An absent outcome reads as Unspecified, which Gemini accepts on an echoed part.
                        Outcome = properties.GetValueOrThrow<CodeExecutionResultOutcome>(
                            GeminiContentProperties.Outcome, fromPropertyName, toPropertyName),
                        Output = GetCodeExecutionOutput(codeResult.Outputs),
                    },
                    ThoughtSignature = properties.GetValueOrThrow<string>(
                        GeminiContentProperties.ThoughtSignature, fromPropertyName, toPropertyName),
                };
            }

            // An executableCode part carries a code string and nothing else, so an input the part cannot
            // hold is reported rather than skipped: a turn that succeeds while the model never sees the
            // input is the failure replaying history is meant to prevent.
            static string GetExecutableCode(IList<MEAI.AIContent>? inputs)
            {
                var fromPropertyName =
                    $"{typeof(MEAI.CodeInterpreterToolCallContent)}.{nameof(MEAI.CodeInterpreterToolCallContent.Inputs)}";
                var toPropertyName = $"{typeof(ExecutableCode)}.{nameof(ExecutableCode.Code)}";

                string? code = null;

                foreach (var input in inputs ?? [])
                {
                    var inputCode = input switch
                    {
                        MEAI.TextContent text => text.Text,
                        MEAI.DataContent data when data.HasTopLevelMediaType("text") =>
                            Encoding.UTF8.GetString(data.Data.Span),
                        _ => null,
                    };

                    if (inputCode is null || code is not null)
                    {
                        GeminiMappingException.Throw(
                            fromPropertyName: fromPropertyName,
                            toPropertyName: toPropertyName,
                            reason: inputCode is null
                                ? $"An {nameof(Part.ExecutableCode)} part holds only a code string, and cannot carry an input of type {input.GetType()}."
                                : $"An {nameof(Part.ExecutableCode)} part holds one code string, but {nameof(MEAI.CodeInterpreterToolCallContent.Inputs)} holds more than one code-bearing entry.");
                    }

                    code = inputCode;
                }

                if (code is null)
                {
                    GeminiMappingException.Throw(
                        fromPropertyName: fromPropertyName,
                        toPropertyName: toPropertyName,
                        reason:
                        $"{nameof(ExecutableCode.Code)} is required, but {nameof(MEAI.CodeInterpreterToolCallContent.Inputs)} holds no {typeof(MEAI.TextContent)} or text {typeof(MEAI.DataContent)} to read it from.");
                }

                return code;
            }

            // A codeExecutionResult part carries a stdout string and nothing else; same rule as above.
            static string? GetCodeExecutionOutput(IList<MEAI.AIContent>? outputs)
            {
                StringBuilder? output = null;

                foreach (var entry in outputs ?? [])
                {
                    if (entry is MEAI.TextContent text)
                    {
                        output ??= new StringBuilder();
                        output.Append(text.Text);
                        continue;
                    }

                    GeminiMappingException.Throw(
                        fromPropertyName:
                        $"{typeof(MEAI.CodeInterpreterToolResultContent)}.{nameof(MEAI.CodeInterpreterToolResultContent.Outputs)}",
                        toPropertyName: $"{typeof(CodeExecutionResult)}.{nameof(CodeExecutionResult.Output)}",
                        reason:
                        $"A {nameof(Part.CodeExecutionResult)} part holds only an output string, and cannot carry an output of type {entry.GetType()}.");
                }

                return output?.ToString();
            }
        }

        static string? CreateMappedResponseMimeType(MEAI.ChatResponseFormat? responseFormat)
        {
            return responseFormat is MEAI.ChatResponseFormatJson ? MediaTypeNames.Application.Json : null;
        }
    }

    /// <param name="requestTools">
    /// The tools the request carries, which a raw representation may supply in place of the mapped ones.
    /// </param>
    private static ToolConfiguration? CreateMappedToolConfiguration(
        MEAI.ChatOptions? options,
        IReadOnlyList<Tool>? requestTools)
    {
        if (options?.ToolMode is null)
        {
            return null;
        }

        if (options.Tools?.Count is null or 0)
        {
            return null;
        }

        // Gemini's ANY mode forces the model to call a function. Asked to require one when the request
        // declares none — only hosted tools — the model loops until it hits the tool-call cap and returns an
        // empty candidate with finishReason TOO_MANY_TOOL_CALLS, after billing every round-trip it made
        // (2026-08-31, v1beta: ~30k tool-use prompt tokens for one such request). An MCP server is no
        // exception; Gemini runs its tools server-side, so no client-visible call can satisfy the mode.
        if (options.ToolMode is MEAI.RequiredChatToolMode
            && requestTools?.Any(static tool => tool.FunctionDeclarations is { Count: > 0 }) is not true)
        {
            GeminiMappingException.Throw(
                fromPropertyName: $"{typeof(MEAI.ChatOptions)}.{nameof(MEAI.ChatOptions.ToolMode)}",
                toPropertyName:
                $"{typeof(FunctionCallingConfiguration)}.{nameof(FunctionCallingConfiguration.Mode)}",
                reason:
                $"{typeof(MEAI.RequiredChatToolMode)} maps to {nameof(FunctionCallingConfigMode.Any)}, which requires the request to declare at least one function; add an {typeof(MEAI.AIFunctionDeclaration)} to {nameof(MEAI.ChatOptions.Tools)}.");
        }

        var functionCallingConfig = options.ToolMode switch
        {
            MEAI.AutoChatToolMode => new FunctionCallingConfiguration { Mode = FunctionCallingConfigMode.Auto },
            MEAI.NoneChatToolMode => new FunctionCallingConfiguration { Mode = FunctionCallingConfigMode.None },
            MEAI.RequiredChatToolMode required => new FunctionCallingConfiguration
            {
                Mode = FunctionCallingConfigMode.Any,
                AllowedFunctionNames = required.RequiredFunctionName is { } name ? [name] : null,
            },
            _ => new FunctionCallingConfiguration(),
        };

        return new ToolConfiguration { FunctionCallingConfiguration = functionCallingConfig };
    }

    /// <summary>
    /// Searches the conversation history for a <see cref="MEAI.FunctionCallContent"/> whose
    /// <see cref="MEAI.FunctionCallContent.CallId"/> matches <paramref name="callId"/> and
    /// returns its <see cref="MEAI.FunctionCallContent.Name"/>.
    /// </summary>
    /// <returns>
    /// The function name, or <c>null</c> if no matching call was found.
    /// </returns>
    private static string? ResolveFunctionName(IEnumerable<MEAI.ChatMessage> chatMessages, string callId)
    {
        foreach (var message in chatMessages)
        {
            foreach (var content in message.Contents)
            {
                if (content is MEAI.FunctionCallContent functionCall && functionCall.CallId == callId)
                {
                    return functionCall.Name;
                }
            }
        }

        return null;
    }

    private static ThinkingConfiguration? CreateMappedThinkingConfiguration(MEAI.ReasoningOptions? reasoning)
    {
        if (reasoning is null)
        {
            return null;
        }

        ThinkingConfigThinkingLevel? thinkingLevel = reasoning.Effort switch
        {
            MEAI.ReasoningEffort.None => ThinkingConfigThinkingLevel.Minimal,
            MEAI.ReasoningEffort.Low => ThinkingConfigThinkingLevel.Low,
            MEAI.ReasoningEffort.Medium => ThinkingConfigThinkingLevel.Medium,
            // Gemini caps at High; ExtraHigh maps to the same level.
            MEAI.ReasoningEffort.High or MEAI.ReasoningEffort.ExtraHigh => ThinkingConfigThinkingLevel.High,
            _ => null,
        };

        // Gemini doesn't distinguish between summary and full thought output;
        // any non-None value enables thought inclusion.
        bool? includeThoughts = reasoning.Output switch
        {
            MEAI.ReasoningOutput.None => false,
            MEAI.ReasoningOutput.Summary or MEAI.ReasoningOutput.Full => true,
            _ => null,
        };

        if (thinkingLevel is null && includeThoughts is null)
        {
            return null;
        }

        return new ThinkingConfiguration
        {
            ThinkingLevel = thinkingLevel,
            IncludeThoughts = includeThoughts,
        };
    }

    /// <exception cref="GeminiMappingException">
    /// <paramref name="properties"/> carries no <see cref="GeminiContentProperties.ToolType"/>, one
    /// that is <see cref="ToolType.Unspecified"/>, or one that is not a <see cref="ToolType"/>.
    /// </exception>
    private static ToolType GetRequiredToolType(
        IReadOnlyDictionary<string, object?> properties,
        string fromPropertyName,
        string toPropertyName)
    {
        var toolType = properties.GetValueOrThrow<ToolType>(
            GeminiContentProperties.ToolType,
            fromPropertyName,
            toPropertyName);

        if (toolType is ToolType.Unspecified)
        {
            GeminiMappingException.Throw(
                fromPropertyName: $"{fromPropertyName}[\"{GeminiContentProperties.ToolType}\"]",
                toPropertyName: toPropertyName,
                reason:
                $"Gemini needs the tool type of a server-side invocation echoed back, so {nameof(GeminiContentProperties)}.{nameof(GeminiContentProperties.ToolType)} must hold the {typeof(ToolType)} the response reported.");
        }

        return toolType;
    }

    private static string? GetThoughtSignature(MEAI.AIContent content)
    {
        return (content.RawRepresentation as Part)?.ThoughtSignature;
    }

    private static Part CreateTextReasoningPart(MEAI.TextReasoningContent content)
    {
        return new Part { Thought = true, Text = content.Text, ThoughtSignature = content.ProtectedData };
    }

    private static Part CreateHostedFileDataPart(MEAI.HostedFileContent fileContent)
    {
        return new Part
        {
            FileData = new FileData
            {
                FileUri = fileContent.FileId,
                MimeType = fileContent.MediaType,
            },
            ThoughtSignature = GetThoughtSignature(fileContent),
        };
    }

    /// <remarks>
    /// <para>
    /// Store names are passed through verbatim; the API's own error on an unknown store is clearer than
    /// anything this mapper could infer from the shape of the identifier.
    /// </para>
    /// <para>
    /// This deviates from <see cref="MEAI.HostedFileSearchTool.Inputs"/>, which lets a service pick the
    /// inputs when none are given: Gemini has no default store, so a bare
    /// <see cref="MEAI.HostedFileSearchTool"/> — as a generic host such as Semantic Kernel adds — cannot
    /// be mapped at all and is reported here rather than as an API error.
    /// </para>
    /// </remarks>
    /// <exception cref="GeminiMappingException">
    /// <see cref="MEAI.HostedFileSearchTool.Inputs"/> is empty, or holds content that is not a
    /// <see cref="MEAI.HostedVectorStoreContent"/>; or the tool carries a
    /// <see cref="GeminiAdditionalProperties.MetadataFilter"/> that is not a string.
    /// </exception>
    private static FileSearch CreateMappedFileSearch(MEAI.HostedFileSearchTool tool)
    {
        if (tool.Inputs is not { Count: > 0 })
        {
            GeminiMappingException.Throw(
                fromPropertyName:
                $"{typeof(MEAI.HostedFileSearchTool)}.{nameof(MEAI.HostedFileSearchTool.Inputs)}",
                toPropertyName: $"{typeof(FileSearch)}.{nameof(FileSearch.FileSearchStoreNames)}",
                reason:
                $"Gemini requires at least one file search store to retrieve from, so {nameof(MEAI.HostedFileSearchTool.Inputs)} must contain at least one {typeof(MEAI.HostedVectorStoreContent)}.");
        }

        var storeNames = new List<string>(tool.Inputs.Count);

        foreach (var input in tool.Inputs)
        {
            if (input is not MEAI.HostedVectorStoreContent vectorStore)
            {
                GeminiMappingException.Throw(
                    fromPropertyName:
                    $"{typeof(MEAI.HostedFileSearchTool)}.{nameof(MEAI.HostedFileSearchTool.Inputs)}",
                    toPropertyName: $"{typeof(FileSearch)}.{nameof(FileSearch.FileSearchStoreNames)}",
                    reason: $"Unsupported tool input type: {input.GetType()}");

                return null!; // unreachable
            }

            storeNames.Add(vectorStore.VectorStoreId);
        }

        return new FileSearch
        {
            FileSearchStoreNames = storeNames,
            TopK = tool.MaximumResultCount,
            MetadataFilter = tool.AdditionalProperties.GetValueOrThrow<string>(
                GeminiAdditionalProperties.MetadataFilter,
                fromPropertyName: $"{typeof(MEAI.AITool)}.{nameof(MEAI.AITool.AdditionalProperties)}",
                toPropertyName: $"{typeof(FileSearch)}.{nameof(FileSearch.MetadataFilter)}"),
        };
    }

    /// <remarks>
    /// <para>
    /// <see cref="MEAI.HostedMcpServerTool.ServerName"/> and
    /// <see cref="MEAI.HostedMcpServerTool.ServerAddress"/> are passed through verbatim. Gemini requires the
    /// name to be lowercase snake_case and unique across the request, and the address to be an absolute URL;
    /// its own errors name the offending value, so they are not re-checked here.
    /// </para>
    /// <para>
    /// <see cref="MEAI.HostedMcpServerTool.ServerDescription"/> has no Gemini counterpart and is dropped. It
    /// is advisory, so nothing observable changes; the two properties below are restrictions, so silently
    /// dropping either would leave a caller believing in a limit that does not apply.
    /// </para>
    /// </remarks>
    /// <exception cref="GeminiMappingException">
    /// <see cref="MEAI.HostedMcpServerTool.AllowedTools"/> is not <see langword="null"/>, or
    /// <see cref="MEAI.HostedMcpServerTool.ApprovalMode"/> is not
    /// <see cref="MEAI.HostedMcpServerToolApprovalMode.NeverRequire"/>.
    /// </exception>
    private static McpServer CreateMappedMcpServer(MEAI.HostedMcpServerTool tool)
    {
        if (tool.AllowedTools is not null)
        {
            // Gemini has a hidden allowedTools field that it accepts and then ignores: a request
            // restricted to one tool still had the model calling the others.
            GeminiMappingException.Throw(
                fromPropertyName:
                $"{typeof(MEAI.HostedMcpServerTool)}.{nameof(MEAI.HostedMcpServerTool.AllowedTools)}",
                toPropertyName: $"{typeof(McpServer)}",
                reason:
                $"Gemini offers the model every tool the MCP server exposes and enforces no allow-list, so restrict the tools on the server itself and leave {nameof(MEAI.HostedMcpServerTool.AllowedTools)} null.");
        }

        // Null, the default, is rejected alongside the modes that ask for approval. M.E.AI documents it as a
        // value "some providers might treat the same as AlwaysRequire", and OpenAIResponsesChatClient does
        // exactly that by leaving the policy unset, letting OpenAI's own "always" default apply. Gemini has
        // no approval hook to leave unset, so reading null as NeverRequire would turn an unstated default
        // into unattended server-side execution with the caller's Headers attached.
        if (tool.ApprovalMode is not MEAI.HostedMcpServerToolNeverRequireApprovalMode)
        {
            GeminiMappingException.Throw(
                fromPropertyName:
                $"{typeof(MEAI.HostedMcpServerTool)}.{nameof(MEAI.HostedMcpServerTool.ApprovalMode)}",
                toPropertyName: $"{typeof(McpServer)}",
                reason:
                $"Gemini invokes remote MCP tools server-side with no approval hook, so only {nameof(MEAI.HostedMcpServerToolApprovalMode)}.{nameof(MEAI.HostedMcpServerToolApprovalMode.NeverRequire)} can be honoured; set it explicitly to accept that.");
        }

        return new McpServer
        {
            Name = tool.ServerName,
            StreamableHttpTransport = new StreamableHttpTransport
            {
                Url = tool.ServerAddress,
                // Copied so that mutating the tool afterwards cannot alter the built request. An empty
                // dictionary would be written as "headers":{} rather than omitted, so it maps to null.
                Headers = tool.Headers is { Count: > 0 } headers ? new Dictionary<string, string>(headers) : null,
            },
        };
    }

    /// <summary>
    /// Collects file references from <see cref="MEAI.HostedCodeInterpreterTool.Inputs"/>
    /// and prepends them to the last user <see cref="Content"/>. Gemini expects files
    /// for code execution to be passed as <see cref="Part"/> entries in the conversation,
    /// not in the tool configuration.
    /// </summary>
    private static void InjectToolInputParts(MEAI.ChatOptions? options, List<Content> contents)
    {
        if (options?.Tools is null)
        {
            return;
        }

        List<Part>? toolInputParts = null;

        foreach (var tool in options.Tools)
        {
            if (tool is MEAI.HostedCodeInterpreterTool { Inputs: { Count: > 0 } inputs })
            {
                toolInputParts ??= [];
                foreach (var input in inputs)
                {
                    toolInputParts.Add(input switch
                    {
                        MEAI.HostedFileContent fc => CreateHostedFileDataPart(fc),
                        _ => ThrowUnsupportedToolInput(input),
                    });
                }
            }
        }

        if (toolInputParts is not { Count: > 0 })
        {
            return;
        }

        // Prepend the file parts to the last user content so the model sees
        // the files in context when executing code against them.
        for (var i = contents.Count - 1; i >= 0; i--)
        {
            if (contents[i].Role is ChatRoles.User)
            {
                var existing = contents[i].Parts ?? [];
                var combined = new List<Part>(toolInputParts.Count + existing.Count);
                combined.AddRange(toolInputParts);
                combined.AddRange(existing);
                contents[i] = contents[i] with { Parts = combined };
                return;
            }
        }

        // Tool input files were collected but there is no user content to
        // attach them to — fail loudly rather than silently dropping them.
        throw new InvalidOperationException(
            "Cannot inject tool input file parts: no user content was found in the conversation.");

        [DoesNotReturn]
        static Part ThrowUnsupportedToolInput(MEAI.AIContent content)
        {
            GeminiMappingException.Throw(
                fromPropertyName: $"{typeof(MEAI.HostedCodeInterpreterTool)}.{nameof(MEAI.HostedCodeInterpreterTool.Inputs)}",
                toPropertyName: $"{typeof(Part)}",
                reason: $"Unsupported tool input type: {content.GetType()}");

            return null!; // unreachable
        }
    }

    private static void AppendSystemInstructionParts(
        MEAI.ChatMessage message,
        List<Part> systemInstructionParts)
    {
        foreach (var content in message.Contents)
        {
            if (content is not MEAI.TextContent textContent)
            {
                GeminiMappingException.Throw(
                    fromPropertyName: $"{typeof(MEAI.ChatMessage)}.{nameof(MEAI.ChatMessage.Contents)}",
                    toPropertyName:
                    $"{typeof(GenerateContentRequest)}.{nameof(GenerateContentRequest.SystemInstruction)}",
                    reason:
                    $"Only {typeof(MEAI.TextContent)} is supported in system instructions because Gemini doesn't support non-text system instructions. Got {content.GetType()}");

                return; // unreachable
            }

            if (string.IsNullOrEmpty(textContent.Text))
            {
                continue;
            }

            systemInstructionParts.Add(new Part { Text = textContent.Text });
        }
    }

    /// <summary>
    /// Creates a batch embedding request where each input string becomes a separate
    /// <see cref="EmbedContentRequest"/>, ensuring one embedding per input value.
    /// </summary>
    /// <param name="model">The model identifier (e.g., "text-embedding-004").</param>
    /// <param name="values">The input strings to embed.</param>
    /// <param name="options">Optional embedding generation options.</param>
    /// <param name="clientOptions">The Gemini client options.</param>
    /// <param name="rawRepresentation">Optional raw representation to merge with the mapped request.</param>
    /// <returns>A <see cref="BatchEmbedContentsRequest"/> containing one request per input string.</returns>
    /// <remarks>
    /// The model name in each request is prefixed with "models/" as required by the
    /// BatchEmbedContents API (e.g., "models/text-embedding-004").
    /// </remarks>
    public static BatchEmbedContentsRequest CreateMappedBatchEmbeddingRequest(
        string model,
        IEnumerable<string> values,
        MEAI.EmbeddingGenerationOptions? options,
        IGeminiClientOptions clientOptions,
        BatchEmbedContentsRequest? rawRepresentation = null)
    {
        if (rawRepresentation is not null)
        {
            // Only one property to merge so, if provided, we return it directly.
            return rawRepresentation;
        }

        // The BatchEmbedContents API requires the full model path in each request
        var modelPath = model.StartsWith("models/", StringComparison.Ordinal) ? model : $"models/{model}";

        // The spec deprecates EmbedContentRequest.outputDimensionality in favour of
        // embedContentConfig.outputDimensionality, but the live v1beta API silently ignores the
        // replacement and returns full-size embeddings (verified 2026-08-18), so the deprecated
        // field remains the only one that takes effect.
#pragma warning disable CS0618 // Type or member is obsolete
        var requests = values.Select(value => new EmbedContentRequest
        {
            Model = modelPath,
            Content = new Content { Parts = [new Part { Text = value }] },
            OutputDimensionality = options?.Dimensions ?? clientOptions.DefaultEmbeddingDimensions,
        }).ToList();
#pragma warning restore CS0618 // Type or member is obsolete

        return new BatchEmbedContentsRequest { Requests = requests, };
    }
}

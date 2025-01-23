using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

using GeminiDotnet.ContentGeneration;
using GeminiDotnet.ContentGeneration.FunctionCalling;

namespace GeminiDotnet.Text.Json;

public sealed class ContentPartJsonConverter : JsonConverter<ContentPart>
{
    // case text
    // case inlineData
    // case fileData
    // case functionCall
    // case functionResponse
    // case executableCode
    // case codeExecutionResult

    private const string TextPropertyName = "text";
    private const string InlineDataPropertyName = "inlineData";
    private const string FileDataPropertyName = "fileData";
    private const string FunctionCallPropertyName = "functionCall";
    private const string FunctionResponsePropertyName = "functionResponse";
    private const string ExecutableCodePropertyName = "executableCode";
    private const string CodeExecutionResult = "codeExecutionResult";

    // TextChatMessagePart: `{ "text": "Great to meet you. What would you like to know?" }`
    // InlineDataChatMessagePart: `{ "inline_data": { "mime_type":"image/jpeg", "data": "base64-encoded-image" } }`
    // ExecutableCodeContentPart: `{ "executableCode": { "language": "PYTHON", "code": "print(\"Hello, World!\")" } }`

    public override ContentPart Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException(
                $"Expected a {nameof(JsonTokenType.StartObject)} token but got {reader.TokenType}.");
        }

        reader.Read();

        if (reader.TokenType != JsonTokenType.PropertyName)
        {
            throw new JsonException(
                $"Expected a {nameof(JsonTokenType.PropertyName)} token but got {reader.TokenType}.");
        }

        var propertyName = reader.GetString();

        ContentPart returnValue = propertyName switch
        {
            TextPropertyName => ParseTextChatMessagePart(ref reader),
            InlineDataPropertyName => ParseObjectPart<InlineDataContentPart>(ref reader, options),
            FileDataPropertyName => ParseObjectPart<FileDataContentPart>(ref reader, options),
            FunctionCallPropertyName => ParseObjectPart<FunctionCallContentPart>(ref reader, options),
            FunctionResponsePropertyName => ParseObjectPart<FunctionResponseContentPart>(ref reader, options),
            ExecutableCodePropertyName => ParseObjectPart<ExecutableCodeContentPart>(ref reader, options),
            CodeExecutionResult => ParseObjectPart<CodeExecutionResultContentPart>(ref reader, options),
            _ => throw new JsonException($"Unexpected property name: {propertyName}.")
        };

        reader.Read();

        if (reader.TokenType != JsonTokenType.EndObject)
        {
            throw new JsonException(
                $"Expected a {nameof(JsonTokenType.EndObject)} token but got {reader.TokenType}.");
        }

        return returnValue;
    }

    private static TPart ParseObjectPart<TPart>(ref Utf8JsonReader reader, JsonSerializerOptions options)
        where TPart : ContentPart
    {
        var typeInfo = (JsonTypeInfo<TPart>)options.GetTypeInfo(typeof(TPart));
        var data = JsonSerializer.Deserialize(ref reader, typeInfo);

        if (data is null)
        {
            throw new JsonException("Expected a non-null value.");
        }

        return data;
    }

    private static TextContentPart ParseTextChatMessagePart(ref Utf8JsonReader reader)
    {
        reader.Read();

        var text = reader.GetString();

        if (text is null)
        {
            throw new JsonException("Expected a string value for the text property but got null.");
        }

        return new TextContentPart { Text = text };
    }

    public override void Write(Utf8JsonWriter writer, ContentPart value, JsonSerializerOptions options)
    {
        switch (value)
        {
            case TextContentPart textChatMessagePart:
                SerializeTextContentPart(ref writer, textChatMessagePart);
                break;
            case InlineDataContentPart inlineDataChatMessagePart:
                SerializeObjectPart(ref writer, inlineDataChatMessagePart, options);
                break;
            case FileDataContentPart fileDataChatMessagePart:
                SerializeObjectPart(ref writer, fileDataChatMessagePart, options);
                break;
            case FunctionCallContentPart functionCallContentPart:
                SerializeObjectPart(ref writer, functionCallContentPart, options);
                break;
            case FunctionResponseContentPart functionResponseContentPart:
                SerializeObjectPart(ref writer, functionResponseContentPart, options);
                break;
            case ExecutableCodeContentPart executableCodeContentPart:
                SerializeObjectPart(ref writer, executableCodeContentPart, options);
                break;
            case CodeExecutionResultContentPart codeExecutionResult:
                SerializeObjectPart(ref writer, codeExecutionResult, options);
                break;
            default:
                throw new JsonException($"Unexpected type: {value.GetType().Name}.");
        }
    }

    private static void SerializeTextContentPart(ref Utf8JsonWriter writer, TextContentPart textChatMessagePart)
    {
        writer.WriteStartObject();
        writer.WriteString(TextPropertyName, textChatMessagePart.Text);
        writer.WriteEndObject();
    }

    private static void SerializeObjectPart<TPart>(ref Utf8JsonWriter writer, TPart part, JsonSerializerOptions options)
        where TPart : ContentPart
    {
        writer.WriteStartObject();

        // Write the type discriminator
        var propertyName = part switch
        {
            InlineDataContentPart => InlineDataPropertyName,
            FileDataContentPart => FileDataPropertyName,
            FunctionCallContentPart => FunctionCallPropertyName,
            FunctionResponseContentPart => FunctionResponsePropertyName,
            ExecutableCodeContentPart => ExecutableCodePropertyName,
            CodeExecutionResultContentPart => CodeExecutionResult,
            _ => throw new JsonException($"Unexpected type: {part.GetType().Name}.")
        };

        writer.WritePropertyName(propertyName);

        // Serialize the object
        var typeInfo = (JsonTypeInfo<TPart>)options.GetTypeInfo(typeof(TPart));
        JsonSerializer.Serialize(writer, part, typeInfo);

        // End the object
        writer.WriteEndObject();
    }
}
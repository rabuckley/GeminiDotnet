using System.Text.Json;
using System.Text.Json.Serialization;

namespace GeminiDotnet.Text.Json;

public sealed class JsonStringBase64Converter : JsonConverter<ReadOnlyMemory<byte>>
{
    public override ReadOnlyMemory<byte> Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        return reader.TokenType == JsonTokenType.String
            ? reader.GetBytesFromBase64()
            : throw new JsonException("Expected a Base64 string");
    }

    public override void Write(Utf8JsonWriter writer, ReadOnlyMemory<byte> value, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(writer);
        writer.WriteBase64StringValue(value.Span);
    }
}

using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GeminiDotnet.Text.Json;

public class JsonStringParsableFormattableConverter<T> : JsonConverter<T>
    where T : IParsable<T>, IFormattable
{
    public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.String)
        {
            throw new JsonException();
        }


        var value = reader.GetString();

        if (T.TryParse(value, CultureInfo.InvariantCulture, out var result))
        {
            return result;
        }

        throw new JsonException();
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString(null, CultureInfo.InvariantCulture));
    }
}
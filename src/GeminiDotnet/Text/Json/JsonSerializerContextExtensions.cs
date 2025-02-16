using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace GeminiDotnet.Text.Json;

internal static class JsonSerializerContextExtensions
{
    public static JsonTypeInfo<T> GetTypeInfo<T>(this JsonSerializerContext context)
    {
        return (JsonTypeInfo<T>)context.GetTypeInfo(typeof(T))!;
    }
}

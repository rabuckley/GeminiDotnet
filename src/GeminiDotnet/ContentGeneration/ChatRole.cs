using System.Text.Json.Serialization;

namespace GeminiDotnet.ContentGeneration;

[JsonConverter(typeof(JsonStringEnumConverter<ChatRole>))]
public enum ChatRole
{
    User,
    Model,
    System
}
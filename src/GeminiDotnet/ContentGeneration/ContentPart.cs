using System.Text.Json.Serialization;

using GeminiDotnet.Text.Json;

namespace GeminiDotnet.ContentGeneration;

/// <summary>
/// The abstract base class for all content parts.
/// </summary>
[JsonConverter(typeof(ContentPartJsonConverter))]
public abstract record ContentPart;
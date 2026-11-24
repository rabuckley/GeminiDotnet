using Microsoft.Extensions.AI;
using System.Text.Json;

namespace GeminiDotnet.Extensions.AI;

/// <summary>
/// Defines keys for Gemini-specific properties carried in Microsoft.Extensions.AI additional-property
/// dictionaries.
/// </summary>
public static class GeminiAdditionalProperties
{
    /// <summary>
    /// Key for the <see cref="ThinkingConfiguration"/> property that configures thinking/reasoning behavior.
    /// Read from <see cref="ChatOptions.AdditionalProperties"/>.
    /// </summary>
    [Obsolete("Use Microsoft.Extensions.AI.ReasoningEffort")]
    public const string ThinkingConfiguration = "thinkingConfig";

    /// <summary>
    /// Key for the response modalities property that specifies the desired output types (e.g., text, image).
    /// Read from <see cref="ChatOptions.AdditionalProperties"/>.
    /// </summary>
    public const string ResponseModalities = "responseModalities";

    /// <summary>
    /// Use a <see cref="V1.ImageConfiguration"/> value in <see cref="ChatOptions.AdditionalProperties"/>.
    /// </summary>
    public const string ImageConfiguration = "imageConfig";

    /// <summary>
    /// Set this key in the <see cref="HostedFileSearchTool"/>'s <see cref="AITool.AdditionalProperties"/>
    /// to filter documents and chunks. Use an <see href="https://google.aip.dev/160">AIP-160 filter</see>
    /// as a <see cref="string"/> or JSON string <see cref="JsonElement"/>.
    /// </summary>
    /// <remarks>A value of any other type throws <see cref="GeminiMappingException"/>.</remarks>
    /// <example>
    /// <see cref="AITool.AdditionalProperties"/> has no setter, so the dictionary must be passed to the
    /// constructor:
    /// <code>
    /// var tool = new HostedFileSearchTool(new Dictionary&lt;string, object?&gt;
    /// {
    ///     [GeminiAdditionalProperties.MetadataFilter] = "author = \"Robert Graves\"",
    /// })
    /// {
    ///     Inputs = [new HostedVectorStoreContent("fileSearchStores/poems")],
    /// };
    /// </code>
    /// </example>
    public const string MetadataFilter = "metadataFilter";
}

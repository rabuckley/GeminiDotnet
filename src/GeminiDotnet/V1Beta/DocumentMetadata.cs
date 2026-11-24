using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta;

/// <summary>
/// Metadata describes the input document content.
/// Set this in <c>Part.document_metadata</c> or
/// <c>FunctionResponsePart.document_metadata</c> when <c>inline_data</c> or <c>file_data</c>
/// contains a document to configure static document preprocessing.
/// </summary>
[global::System.Diagnostics.CodeAnalysis.Experimental("GEMINI001", UrlFormat = "https://github.com/rabuckley/GeminiDotnet#v1beta-stability")]
public sealed record DocumentMetadata
{
    /// <summary>
    /// Optional. Content layers to return.
    /// If empty, defaults to [PAGE_IMAGE] when DocumentMetadata is set. When
    /// DocumentMetadata is not set, default document processing is used (for
    /// example, page images for PDFs).
    /// </summary>
    [JsonPropertyName("contentLayers")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IReadOnlyList<DocumentMetadataContentLayers>? ContentLayers { get; init; }

    /// <summary>
    /// Optional. 1-indexed inclusive end page.
    /// </summary>
    [JsonPropertyName("endPage")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int? EndPage { get; init; }

    /// <summary>
    /// Optional. 1-indexed inclusive start page.
    /// </summary>
    [JsonPropertyName("startPage")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int? StartPage { get; init; }
}


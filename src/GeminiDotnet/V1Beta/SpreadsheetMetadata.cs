using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta;

/// <summary>
/// Metadata describes the input spreadsheet content.
/// Set this in <c>Part.spreadsheet_metadata</c> or
/// <c>FunctionResponsePart.spreadsheet_metadata</c> when <c>inline_data</c> or
/// <c>file_data</c> contains a spreadsheet to configure static spreadsheet
/// preprocessing.
/// </summary>
[global::System.Diagnostics.CodeAnalysis.Experimental("GEMINI001", UrlFormat = "https://github.com/rabuckley/GeminiDotnet#v1beta-stability")]
public sealed record SpreadsheetMetadata
{
    /// <summary>
    /// Optional. Content layers to return.
    /// If empty, defaults to [DATA, FORMULAS] when SpreadsheetMetadata is set.
    /// </summary>
    [JsonPropertyName("contentLayers")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IReadOnlyList<SpreadsheetMetadataContentLayers>? ContentLayers { get; init; }

    /// <summary>
    /// Optional. The 1-indexed index of the sheet to extract, in workbook order.
    /// If not set, all sheets are extracted.
    /// </summary>
    [JsonPropertyName("sheetIndex")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int? SheetIndex { get; init; }

    /// <summary>
    /// Optional. The range of cells to extract, in A1 notation (e.g. "A1:D10").
    /// If not set, the full occupied area of the sheet is used.
    /// </summary>
    [JsonPropertyName("visibleCells")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? VisibleCells { get; init; }
}


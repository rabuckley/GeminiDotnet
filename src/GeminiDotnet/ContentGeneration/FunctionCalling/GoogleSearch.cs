using System.Text.Json.Serialization;

namespace GeminiDotnet.ContentGeneration.FunctionCalling;

/// <summary>
/// GoogleSearch tool type. Tool to support Google Search in Model. Powered by Google.
/// </summary>
public sealed record GoogleSearch
{
    /// <summary>
    /// Optional. Filter search results to a specific time range. If customers set a start time, they must set an end
    /// time (and vice versa).
    /// </summary>
    [JsonPropertyName("timeRangeFilter")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Interval? TimeRangeFilter { get; init; }
}

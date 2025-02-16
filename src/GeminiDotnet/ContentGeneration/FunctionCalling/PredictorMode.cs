using System.Text.Json.Serialization;

namespace GeminiDotnet.ContentGeneration.FunctionCalling;

/// <summary>
/// The mode of the predictor to be used in dynamic retrieval.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter<PredictorMode>))]
public enum PredictorMode
{
    [JsonStringEnumMemberName("MODE_UNSPECIFIED")]
    Unspecified,

    [JsonStringEnumMemberName("MODE_DYNAMIC")]
    Dynamic
}

using System.Text.Json.Serialization;

namespace GeminiDotnet.ContentGeneration;

/// <summary>
/// Configuration options for model generation and outputs. Not all parameters are configurable for every model.
/// </summary>
public sealed record GenerationConfiguration
{
    /// <summary>
    ///  The set of character sequences (up to 5) that will stop output generation. If specified, the API will stop at
    /// the first appearance of a stop_sequence. The stop sequence will not be included as part of the response.
    /// </summary>
    [JsonPropertyName("stopSequences")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IEnumerable<string>? StopSequences { get; init; }

    /// <summary>
    /// MIME type of the generated candidate text.
    /// </summary>
    [JsonPropertyName("responseMimeType")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ResponseMimeType { get; init; }

    /// <summary>
    /// Output schema of the generated candidate text. Schemas must be a subset of the OpenAPI schema and can be
    /// objects, primitives or arrays. If set, a compatible <see cref="ResponseMimeType"/> must also be set.
    /// </summary>
    [JsonPropertyName("responseSchema")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Schema? ResponseSchema { get; init; }

    /// <summary>
    /// The requested modalities of the response. Represents the set of modalities that the model can return, and
    /// should be expected in the response. This is an exact match to the modalities of the response. A model may have
    /// multiple combinations of supported modalities. If the requested modalities do not match any of the supported
    /// combinations, an error will be returned. An empty list is equivalent to requesting only text.
    /// </summary>
    [JsonPropertyName("responseModalities")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IEnumerable<Modality>? ResponseModalities { get; init; }

    /// <summary>
    /// Number of generated responses to return.
    /// </summary>
    [JsonPropertyName("candidateCount")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? CandidateCount { get; init; }

    /// <summary>
    /// The maximum number of tokens to include in a response candidate.
    /// </summary>
    [JsonPropertyName("maxOutputTokens")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? MaxOutputTokens { get; init; }

    /// <summary>
    ///  Controls the randomness of the output. Values can range from <c>[0.0, 2.0]</c>.
    /// </summary>
    [JsonPropertyName("temperature")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public float? Temperature { get; init; }

    /// <summary>
    /// The maximum cumulative probability of tokens to consider when sampling.
    /// </summary>
    [JsonPropertyName("topP")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public float? TopP { get; init; }

    /// <summary>
    /// The maximum number of tokens to consider when sampling.
    /// </summary>
    [JsonPropertyName("topK")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? TopK { get; init; }

    /// <summary>
    /// Seed used in decoding. If not set, the request uses a randomly generated seed.
    /// </summary>
    [JsonPropertyName("seed")]
    public long? Seed { get; init; }

    /// <summary>
    /// Presence penalty applied to the next token's logprobs if the token has already been seen in the response.
    /// This penalty is binary on/off and not dependent on the number of times the token is used (after the first).
    /// Use <see cref="FrequencyPenalty"/> for a penalty that increases with each use.
    /// A positive penalty will discourage the use of tokens that have already been used in the response, increasing the vocabulary.
    /// A negative penalty will encourage the use of tokens that have already been used in the response, decreasing the vocabulary.
    /// </summary>
    [JsonPropertyName("presencePenalty")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public float? PresencePenalty { get; init; }

    /// <summary>
    /// Frequency penalty applied to the next token's logprobs, multiplied by the number of times each token has been
    /// seen in the response so far.
    /// 
    /// A positive penalty will discourage the use of tokens that have already been used, proportional to the number
    /// of times the token has been used: The more a token is used, the more difficult it is for the model to use that
    /// token again increasing the vocabulary of responses.
    /// <remarks>
    /// Caution: A negative penalty will encourage the model to reuse tokens proportional to the number of times the
    /// token has been used. Small negative values will reduce the vocabulary of a response. Larger negative values
    /// will cause the model to start repeating a common token until it hits the <see cref="MaxOutputTokens"/> limit.
    /// </remarks>
    /// </summary>
    [JsonPropertyName("frequencyPenalty")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public float? FrequencyPenalty { get; init; }

    /// <summary>
    /// If true, export the logprobs results in response.
    /// </summary>
    [JsonPropertyName("responseLogprobs")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? ResponseLogprobs { get; init; }

    /// <summary>
    /// Only valid if <see cref="ResponseLogprobs"/> is <see langword="true"/>. This sets the number of top logprobs to
    /// return at each decoding step in the <see cref="Candidate.LogprobsResult"/>.
    /// </summary>
    [JsonPropertyName("logprobs")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Logprobs { get; init; }

    /// <summary>
    /// Enables enhanced civic answers. It may not be available for all models.
    /// </summary>
    [JsonPropertyName("enableEnhancedCivicAnswers")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? EnableEnhancedCivicAnswers { get; init; }

    /// <summary>
    /// The speech generation configuration.
    /// </summary>
    [JsonPropertyName("speechConfig")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public SpeechConfiguration? SpeechConfiguration { get; init; }

    [JsonPropertyName("thinkingConfig")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ThinkingConfiguration? ThinkingConfiguration { get; init; }
}

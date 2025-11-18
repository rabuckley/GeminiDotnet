using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.Models;

[JsonConverter(typeof(JsonStringEnumConverter<TaskType>))]
public enum TaskType
{
    /// <summary>
    /// Unset value, which will default to one of the other enum values.
    /// </summary>
    [JsonStringEnumMemberName("TASK_TYPE_UNSPECIFIED")]
    Unspecified,

    /// <summary>
    /// Specifies the given text is a query in a search/retrieval setting.
    /// </summary>
    [JsonStringEnumMemberName("RETRIEVAL_QUERY")]
    RetrievalQuery,

    /// <summary>
    /// Specifies the given text is a document from the corpus being searched.
    /// </summary>
    [JsonStringEnumMemberName("RETRIEVAL_DOCUMENT")]
    RetrievalDocument,

    /// <summary>
    /// Specifies the given text will be used for STS.
    /// </summary>
    [JsonStringEnumMemberName("SEMANTIC_SIMILARITY")]
    SemanticSimilarity,

    /// <summary>
    /// Specifies that the given text will be classified.
    /// </summary>
    [JsonStringEnumMemberName("CLASSIFICATION")]
    Classification,

    /// <summary>
    /// Specifies that the embeddings will be used for clustering.
    /// </summary>
    [JsonStringEnumMemberName("CLUSTERING")]
    Clustering,

    /// <summary>
    /// Specifies that the given text will be used for question answering.
    /// </summary>
    [JsonStringEnumMemberName("QUESTION_ANSWERING")]
    QuestionAnswering,

    /// <summary>
    /// Specifies that the given text will be used for fact verification.
    /// </summary>
    [JsonStringEnumMemberName("FACT_VERIFICATION")]
    FactVerification,

    /// <summary>
    /// Specifies that the given text will be used for code retrieval.
    /// </summary>
    [JsonStringEnumMemberName("CODE_RETRIEVAL_QUERY")]
    CodeRetrievalQuery,
}


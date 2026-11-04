using System.Text.Json;
using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta;

/// <remarks>
/// Unknown API values are preserved.
/// </remarks>
[JsonConverter(typeof(TaskType.Converter))]
public readonly struct TaskType : IEquatable<TaskType>
{
    private readonly string? _value;

    /// <summary>
    /// Initializes a new instance of the <see cref="TaskType"/> structure.
    /// </summary>
    /// <param name="value">
    /// The wire value, including values not declared by this type.
    /// </param>
    /// <exception cref="global::System.ArgumentNullException">
    /// <paramref name="value"/> is <see langword="null"/>.
    /// </exception>
    public TaskType(string value)
    {
        global::System.ArgumentNullException.ThrowIfNull(value);
        _value = value;
    }

    /// <summary>
    /// Unset value, which will default to one of the other enum values.
    /// </summary>
    public static TaskType Unspecified { get; } = new("TASK_TYPE_UNSPECIFIED");

    /// <summary>
    /// Specifies the given text is a query in a search/retrieval setting.
    /// </summary>
    public static TaskType RetrievalQuery { get; } = new("RETRIEVAL_QUERY");

    /// <summary>
    /// Specifies the given text is a document from the corpus being searched.
    /// </summary>
    public static TaskType RetrievalDocument { get; } = new("RETRIEVAL_DOCUMENT");

    /// <summary>
    /// Specifies the given text will be used for STS.
    /// </summary>
    public static TaskType SemanticSimilarity { get; } = new("SEMANTIC_SIMILARITY");

    /// <summary>
    /// Specifies that the given text will be classified.
    /// </summary>
    public static TaskType Classification { get; } = new("CLASSIFICATION");

    /// <summary>
    /// Specifies that the embeddings will be used for clustering.
    /// </summary>
    public static TaskType Clustering { get; } = new("CLUSTERING");

    /// <summary>
    /// Specifies that the given text will be used for question answering.
    /// </summary>
    public static TaskType QuestionAnswering { get; } = new("QUESTION_ANSWERING");

    /// <summary>
    /// Specifies that the given text will be used for fact verification.
    /// </summary>
    public static TaskType FactVerification { get; } = new("FACT_VERIFICATION");

    /// <summary>
    /// Specifies that the given text will be used for code retrieval.
    /// </summary>
    public static TaskType CodeRetrievalQuery { get; } = new("CODE_RETRIEVAL_QUERY");

    /// <summary>
    /// Gets the declared values in schema order.
    /// </summary>
    public static IReadOnlyList<TaskType> KnownValues { get; } =
    [
        Unspecified,
        RetrievalQuery,
        RetrievalDocument,
        SemanticSimilarity,
        Classification,
        Clustering,
        QuestionAnswering,
        FactVerification,
        CodeRetrievalQuery,
    ];

    /// <summary>
    /// Gets the wire value.
    /// </summary>
    /// <value>
    /// The default instance has the value of <see cref="Unspecified"/>.
    /// </value>
    public string Value => _value ?? "TASK_TYPE_UNSPECIFIED";

    /// <summary>
    /// Compares wire values using ordinal equality.
    /// </summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    public static bool operator ==(TaskType left, TaskType right) => left.Equals(right);

    /// <summary>
    /// Compares wire values using ordinal inequality.
    /// </summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    public static bool operator !=(TaskType left, TaskType right) => !left.Equals(right);

    /// <inheritdoc/>
    public bool Equals(TaskType other) => string.Equals(Value, other.Value, global::System.StringComparison.Ordinal);

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is TaskType other && Equals(other);

    /// <inheritdoc/>
    public override int GetHashCode() => global::System.StringComparer.Ordinal.GetHashCode(Value);

    /// <summary>
    /// Returns <see cref="Value"/>.
    /// </summary>
    public override string ToString() => Value;

    [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
    public sealed class Converter : JsonConverter<TaskType>
    {
        /// <inheritdoc/>
        public override TaskType Read(ref Utf8JsonReader reader, global::System.Type typeToConvert, JsonSerializerOptions options)
            => new(reader.GetString() ?? throw new JsonException("Expected a string for TaskType."));

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, TaskType value, JsonSerializerOptions options)
        {
            global::System.ArgumentNullException.ThrowIfNull(writer);
            writer.WriteStringValue(value.Value);
        }
    }
}


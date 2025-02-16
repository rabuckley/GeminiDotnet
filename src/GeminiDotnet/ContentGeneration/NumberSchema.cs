namespace GeminiDotnet.ContentGeneration;

public sealed record NumberSchema : Schema
{
    internal static NumberSchema Create(SchemaInfo schemaInfo)
    {
        return new NumberSchema
        {
            Format = schemaInfo.Format, Description = schemaInfo.Description, Nullable = schemaInfo.Nullable,
        };
    }
}
namespace GeminiDotnet.ContentGeneration;

public sealed record BooleanSchema : Schema
{
    internal static BooleanSchema Create(SchemaInfo schemaInfo)
    {
        return new BooleanSchema
        {
            Format = schemaInfo.Format,
            Description = schemaInfo.Description,
            Nullable = schemaInfo.Nullable,
        };
    }
}

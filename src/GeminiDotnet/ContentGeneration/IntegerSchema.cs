namespace GeminiDotnet.ContentGeneration;

public sealed record IntegerSchema : Schema
{
    internal static IntegerSchema Create(SchemaInfo schemaInfo)
    {
        return new IntegerSchema
        {
            Format = schemaInfo.Format, Description = schemaInfo.Description, Nullable = schemaInfo.Nullable,
        };
    }
}
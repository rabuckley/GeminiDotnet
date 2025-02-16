using GeminiDotnet.ContentGeneration;
using System.Text.Json;
using System.Text.Json.Schema;

namespace GeminiDotnet;

public sealed class SchemaTests
{
    [Theory]
    [InlineData(typeof(int))]
    [InlineData(typeof(long))]
    public void FromJsonNode_WithIntegerModel_ShouldReturnIntegerSchema(Type type)
    {
        _ = FromJsonNodeTest<IntegerSchema>(type);
    }

    [Theory]
    [InlineData(typeof(float))]
    [InlineData(typeof(double))]
    public void FromJsonNode_WithNumberModel_ShouldReturnNumberSchema(Type type)
    {
        _ = FromJsonNodeTest<NumberSchema>(type);
    }

    [Fact]
    public void FromJsonNode_WithBooleanModel_ShouldReturnBooleanSchema()
    {
        _ = FromJsonNodeTest<BooleanSchema>(typeof(bool));
    }

    [Theory]
    [InlineData(typeof(int[]))]
    [InlineData(typeof(float[]))]
    [InlineData(typeof(TestObject[]))]
    public void FromJsonNode_WithArrayModel_ShouldReturnArraySchema(Type type)
    {
        _ = FromJsonNodeTest<ArraySchema>(type);
    }

    [Fact]
    public void FromJsonNode_WithClass_ShouldIncludeProperties()
    {
        // Arrange
        var node = JsonSerializerOptions.Default.GetJsonSchemaAsNode(typeof(TestObject));

        // Act
        var schema = Schema.FromJsonNode(node);

        // Assert
        var objectSchema = Assert.IsType<ObjectSchema>(schema);

        Assert.NotNull(objectSchema.Properties);
        Assert.Equal(3, objectSchema.Properties.Count);

        Assert.Collection(objectSchema.Properties,
            static prop =>
            {
                Assert.Equal("Id", prop.Key);
                Assert.IsType<IntegerSchema>(prop.Value);
            },
            static prop =>
            {
                Assert.Equal("Name", prop.Key);
                Assert.IsType<StringSchema>(prop.Value);
            },
            static prop =>
            {
                Assert.Equal("Description", prop.Key);
                var ss = Assert.IsType<StringSchema>(prop.Value);
                Assert.True(ss.Nullable);
            });
    }

    private static TSchema FromJsonNodeTest<TSchema>(Type type) where TSchema : Schema
    {
        // Arrange
        var node = JsonSerializerOptions.Default.GetJsonSchemaAsNode(type);

        // Act
        var schema = Schema.FromJsonNode(node);

        // Assert
        return Assert.IsType<TSchema>(schema);
    }

    private sealed record TestObject
    {
        public int Id { get; init; }

        public required string Name { get; init; }

        public string? Description { get; init; }
    }
}

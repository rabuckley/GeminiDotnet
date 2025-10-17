using GeminiDotnet.ContentGeneration;
using System.Text.Json.Serialization;
using Xunit.Sdk;

namespace GeminiDotnet.Extensions.AI;

public sealed class ResponseFormatTestCases : TheoryData<ResponseFormatTestCase>
{
    public ResponseFormatTestCases()
    {
        Add(new ResponseFormatTestCase("Integer", typeof(int), new IntegerSchema()));
        Add(new ResponseFormatTestCase("String", typeof(string), new StringSchema()));
        Add(new ResponseFormatTestCase("Boolean", typeof(bool), new BooleanSchema()));
        Add(new ResponseFormatTestCase("Double", typeof(double), new NumberSchema()));

        Add(new ResponseFormatTestCase("Array", typeof(string[]), new ArraySchema()
        {
            Items = new StringSchema(),
        }));

        Add(
            new ResponseFormatTestCase(
                "Object with nullable property",
                typeof(TestParentObjectWithNullableProperty),
                new ObjectSchema()
                {
                    Properties = new Dictionary<string, Schema>()
                    {
                        ["value"] = new IntegerSchema()
                        {
                            Nullable = true
                        },
                    }
                }
            )
        );

        Add(
            new ResponseFormatTestCase(
                "Object with non-nullable property",
                typeof(TestParentObjectWithNonNullableProperty),
                new ObjectSchema()
                {
                    Properties = new Dictionary<string, Schema>()
                    {
                        ["value"] = new IntegerSchema(),
                    },
                }
            )
        );

        Add(
            new ResponseFormatTestCase(
                "Nested Object",
                typeof(TestParentObjectWithChild),
                new ObjectSchema()
                {
                    Properties = new Dictionary<string, Schema>
                    {
                        ["child"] = new ObjectSchema()
                        {
                            Properties = new Dictionary<string, Schema>()
                            {
                                ["name"] = new StringSchema(),
                            },
                        },
                    },
                }
            )
        );

        Add(
            new ResponseFormatTestCase(
                "Object with multiple child refs",
                typeof(TestParentObjectWithChildren),
                new ObjectSchema()
                {
                    Properties = new Dictionary<string, Schema>()
                    {
                        ["child1"] = new ObjectSchema()
                        {
                            Properties = new Dictionary<string, Schema>
                            {
                                ["name"] = new StringSchema(),
                            },
                        },
                        ["child2"] = new ObjectSchema()
                        {
                            Properties = new Dictionary<string, Schema>()
                            {
                                ["name"] = new StringSchema(),
                            },
                        },
                    },
                }
            )
        );

        Add(
            new ResponseFormatTestCase(
                "Object with collection of child refs",
                typeof(TestParentObjectWithChildrenRefs),
                new ObjectSchema()
                {
                    Properties = new Dictionary<string, Schema>()
                    {
                        ["children"] = new ArraySchema()
                        {
                            Items = new ObjectSchema()
                            {
                                Properties = new Dictionary<string, Schema>()
                                {
                                    ["name"] = new StringSchema(),
                                },
                            },
                        },
                        ["stepChildren"] = new ArraySchema()
                        {
                            Items = new ObjectSchema()
                            {
                                Properties = new Dictionary<string, Schema>()
                                {
                                    ["name"] = new StringSchema(),
                                },
                            },
                        },
                    },
                }
            )
        );
    }

    private sealed class TestParentObjectWithNullableProperty
    {
        [JsonPropertyName("value")]
        public int? Value { get; init; }
    }

    private sealed class TestParentObjectWithNonNullableProperty
    {
        [JsonPropertyName("value")]
        public int Value { get; init; }
    }

    private sealed class TestParentObjectWithChild
    {
        [JsonPropertyName("child")]
        public TestChildObject Child { get; init; } = new TestChildObject();
    }

    private sealed class TestParentObjectWithChildren
    {
        [JsonPropertyName("child1")]
        public TestChildObject ChildOne { get; init; } = new TestChildObject();

        [JsonPropertyName("child2")]
        public TestChildObject ChildTwo { get; init; } = new TestChildObject();
    }

    private sealed class TestParentObjectWithChildrenRefs
    {
        [JsonPropertyName("children")]
        public TestChildObject[] Children { get; init; } = [];

        [JsonPropertyName("stepChildren")]
        public TestChildObject[] StepChildren { get; init; } = [];
    }

    private sealed class TestChildObject
    {
        [JsonPropertyName("name")]
        public string Name { get; init; } = string.Empty;
    }
}

public sealed record ResponseFormatTestCase : IXunitSerializable
{
    public string TestCaseName { get; private set; }
    public Type SchemaType { get; private set; }
    public Schema ExpectedSchema { get; private set; }

    public ResponseFormatTestCase()
    {
        TestCaseName = string.Empty;
        SchemaType = typeof(object);
        ExpectedSchema = new ObjectSchema();
    }

    public ResponseFormatTestCase(
        string testCaseName,
        Type schemaType,
        Schema expectedSchema
    )
    {
        TestCaseName = testCaseName;
        SchemaType = schemaType;
        ExpectedSchema = expectedSchema;
    }

    public void Deserialize(IXunitSerializationInfo info)
    {
        TestCaseName = info.GetValue<string>(nameof(TestCaseName)) ?? string.Empty;
        SchemaType = info.GetValue<Type>(nameof(SchemaType)) ?? typeof(object);
        ExpectedSchema = info.GetValue<Schema>(nameof(ExpectedSchema)) ?? new ObjectSchema();
    }

    public void Serialize(IXunitSerializationInfo info)
    {
        info.AddValue(nameof(TestCaseName), TestCaseName);
        info.AddValue(nameof(SchemaType), SchemaType);
        info.AddValue(nameof(ExpectedSchema), ExpectedSchema);
    }

    public override string ToString()
    {
        return TestCaseName;
    }
}

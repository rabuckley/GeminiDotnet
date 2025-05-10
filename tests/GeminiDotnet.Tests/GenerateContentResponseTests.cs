using GeminiDotnet.ContentGeneration;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace GeminiDotnet;

public sealed class GenerateContentResponseTests
{
    [Theory]
    [MemberData(nameof(ExampleResponses))]
    public void JsonRoundtrip(string json)
    {
        // Arrange
        var response = JsonSerializer.Deserialize<GenerateContentResponse>(json);

        // Act
        var serialized = JsonSerializer.Serialize(response);
        var deserialized = JsonSerializer.Deserialize<GenerateContentResponse>(serialized);

        // Assert
        Assert.Equivalent(response, deserialized);
    }

    public static IEnumerable<TheoryDataRow<string>> ExampleResponses()
    {
        yield return PythonCodeExecutionExampleResponse;
        yield return CodeExExample2;
        yield return JsonSchemaOutputExample;
        yield return VideoExampleResponse;
        yield return GoogleSearchExampleResponse;
    }

    [StringSyntax(StringSyntaxAttribute.Json)]
    private const string PythonCodeExecutionExampleResponse =
        """
        {
          "candidates": [
            {
              "content": {
                "parts": [
                  {
                    "text": "To print \"Hello, World!\" using Python, I will use the `print()` function.  This is a standard function in Python used for displaying output to the console.\\Here's the Python code:\\"
                  },
                  {
                    "executableCode": {
                      "language": "PYTHON",
                      "code": "print(\"Hello, World!\")"
                    }
                  },
                  {
                    "codeExecutionResult": {
                      "outcome": "OUTCOME_OK",
                      "output": "Hello, World!"
                    }
                  },
                  {
                    "text": "The code successfully prints \"Hello, World!\" to the console.  No further analysis or information gathering is needed."
                  }
                ],
                "role": "model"
              },
              "finishReason": "STOP",
              "avgLogprobs": -0.030912578841786324
            }
          ],
          "usageMetadata": {
            "promptTokenCount": 10,
            "candidatesTokenCount": 81,
            "totalTokenCount": 91
          },
          "modelVersion": "gemini-1.5-flash"
        }
        """;

    [StringSyntax(StringSyntaxAttribute.Json)]
    public const string CodeExExample2 =
        """
        {
          "candidates": [
            {
              "content": {
                "parts": [
                  {
                    "executableCode": {
                      "language": "PYTHON",
                      "code": "print(\"Hello, World!\")"
                    }
                  },
                  {
                    "codeExecutionResult": {
                      "outcome": "OUTCOME_OK",
                      "output": "Hello, World!"
                    }
                  },
                  {
                    "text": "Hello, World!"
                  }
                ],
                "role": "model"
              },
              "finishReason": "STOP"
            }
          ],
          "usageMetadata": {
            "promptTokenCount": 10,
            "candidatesTokenCount": 17,
            "totalTokenCount": 27,
            "promptTokensDetails": [
              {
                "modality": "TEXT",
                "tokenCount": 10
              }
            ],
            "candidatesTokensDetails": [
              {
                "modality": "TEXT",
                "tokenCount": 17
              }
            ]
          },
          "modelVersion": "gemini-2.0-flash"
        }
        """;

    [StringSyntax(StringSyntaxAttribute.Json)]
    private const string JsonSchemaOutputExample =
        """
        {
          "candidates": [
            {
              "content": {
                "parts": [
                  {
                    "text": "{ \"name\": \"Ryan\", \"age\": 42 }"
                  }
                ],
                "role": "model"
              },
              "finishReason": "MAX_TOKENS",
              "citationMetadata": {
                "citationSources": [
                  {
                    "startIndex": 257,
                    "endIndex": 23567
                  },
                  {
                    "startIndex": 23578,
                    "endIndex": 24592
                  }
                ]
              },
              "avgLogprobs": -0.022648460378868095
            }
          ],
          "usageMetadata": {
            "promptTokenCount": 12,
            "candidatesTokenCount": 8190,
            "totalTokenCount": 8202,
            "promptTokensDetails": [
              {
                "modality": "TEXT",
                "tokenCount": 12
              }
            ],
            "candidatesTokensDetails": [
              {
                "modality": "TEXT",
                "tokenCount": 8190
              }
            ]
          },
          "modelVersion": "gemini-2.0-flash"
        }
        """;

    [StringSyntax(StringSyntaxAttribute.Json)]
    public const string VideoExampleResponse =
        """
        {
          "candidates": [
            {
              "content": {
                "parts": [
                  {
                    "text": ":\\n\\nIn a talk from Lenny's Podcast between Lenny Ratchitsky and Michael Tru"
                  }
                ],
                "role": "model"
              }
            }
          ],
          "usageMetadata": {
            "promptTokenCount": 21,
            "totalTokenCount": 21,
            "promptTokensDetails": [
              {
                "modality": "TEXT",
                "tokenCount": 21
              },
              {
                "modality": "VIDEO"
              },
              {
                "modality": "AUDIO"
              }
            ]
          },
          "modelVersion": "gemini-2.0-flash"
        }
        """;

    [StringSyntax(StringSyntaxAttribute.Json)]
    public const string GoogleSearchExampleResponse =
        """
        {
          "candidates": [
            {
              "content": {
                "parts": [
                  {
                    "text": "The next total solar eclipse visible from the contiguous United States will be on August 23, 2044. However, the path of totality will only pass through a few northern states like Montana, North Dakota, and South Dakota.\n\nFollowing that, a total solar eclipse will occur on August 12, 2045, with a path spanning from California to Florida.\n"
                  }
                ],
                "role": "model"
              },
              "finishReason": "STOP",
              "groundingMetadata": {
                "searchEntryPoint": {
                  "renderedContent": "removed for brevity"
                },
                "groundingChunks": [
                  {
                    "web": {
                      "uri": "https://vertexaisearch.cloud.google.com/grounding-api-redirect/AbF9wXGoTY8RbwxkbUqVu8f2FbrRG_vbF5lDHQDFxZxfLv6XMQh45T4O-twXiZgD4O1tQEb6cWGJdtvLG0iRY0fs0Q2bQI2m3HKKc11pDW8Et53ybNwMAe_9VdlG-bN6kOnoFtDRXtTfOealkNf3H0M_MrhaW_Hq7S7qJ80vt4M=",
                      "title": "cbsnews.com"
                    }
                  },
                  {
                    "web": {
                      "uri": "https://vertexaisearch.cloud.google.com/grounding-api-redirect/AbF9wXG4o9PFe-Dy2zFb29Q-R1YWxiC1G73kbRrI7AssWEO1FP8knH6TKZMTqm3ikTXH61M9z1oqeZDIhGEXnp7QzfEobM5tRzi6fzlH1hQZcKpJ5k9vLFhYR-AW9DT-yrOfk9LRgwFiICXVb4Id2L_r8nU9lEHtPRWTOjrEiw==",
                      "title": "nasa.gov"
                    }
                  },
                  {
                    "web": {
                      "uri": "https://vertexaisearch.cloud.google.com/grounding-api-redirect/AbF9wXEI6bmS_EaMWBes0nAhlHrvbXvQSkzEa5fnyw_6v2M-4-3ihCwFA9RjdddXHIGW8u8N4npbDC_tfOLjjhACf_VF4Tg3UYlqecj-FMZdEhQC9Ab7vCoZ_d1CXLAAgSPzFzbM4AArWQ5LQ7byrqYNlsW7smr3l4vzPbHrU14C5pkVs5kWJaoilFkerQi3vsMCf5VZ6Vnr7nB_r0sIUAVj-6kLPdg=",
                      "title": "fox26houston.com"
                    }
                  }
                ],
                "groundingSupports": [
                  {
                    "segment": {
                      "endIndex": 97,
                      "text": "The next total solar eclipse visible from the contiguous United States will be on August 23, 2044"
                    },
                    "groundingChunkIndices": [
                      0,
                      1,
                      2
                    ]
                  },
                  {
                    "segment": {
                      "startIndex": 99,
                      "endIndex": 218,
                      "text": "However, the path of totality will only pass through a few northern states like Montana, North Dakota, and South Dakota"
                    },
                    "groundingChunkIndices": [
                      0,
                      2
                    ]
                  },
                  {
                    "segment": {
                      "startIndex": 221,
                      "endIndex": 337,
                      "text": "Following that, a total solar eclipse will occur on August 12, 2045, with a path spanning from California to Florida"
                    },
                    "groundingChunkIndices": [
                      0
                    ]
                  }
                ],
                "retrievalMetadata": {},
                "webSearchQueries": [
                  "next total solar eclipse United States"
                ]
              }
            }
          ],
          "usageMetadata": {
            "promptTokenCount": 12,
            "candidatesTokenCount": 81,
            "totalTokenCount": 93,
            "promptTokensDetails": [
              {
                "modality": "TEXT",
                "tokenCount": 12
              }
            ],
            "candidatesTokensDetails": [
              {
                "modality": "TEXT",
                "tokenCount": 81
              }
            ]
          },
          "modelVersion": "gemini-2.0-flash"
        }
        """;
}

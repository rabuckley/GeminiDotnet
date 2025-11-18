using GeminiDotnet.V1Beta;
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
        yield return UrlContextResponse;
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
          "modelVersion": "gemini-1.5-flash",
          "responseId": "bvq7aInSLPn9nsEP3MKX6A4"
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
          "modelVersion": "gemini-2.0-flash",
          "responseId": "bvq7aInSLPn9nsEP3MKX6A4"
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
          "modelVersion": "gemini-2.0-flash",
          "responseId": "bvq7aInSLPn9nsEP3MKX6A4"
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
          "modelVersion": "gemini-2.0-flash",
          "responseId": "bvq7aInSLPn9nsEP3MKX6A4"
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
          "modelVersion": "gemini-2.0-flash",
          "responseId": "bvq7aInSLPn9nsEP3MKX6A4"
        }
        """;

    [StringSyntax(StringSyntaxAttribute.Json)]
    private const string UrlContextResponse =
        """
        {
          "candidates": [
            {
              "content": {
                "parts": [
                  {
                    "text": "\nArtificial intelligence (AI) is a field of computer science focused on creating computational systems capable of performing tasks typically associated with human intelligence, such as learning, reasoning, problem-solving, perception, and decision-making. AI research and development also involve studying methods and software that enable machines to perceive their environment and use intelligence to achieve specific goals.\n\nHigh-profile applications of AI are common in modern life, including advanced web search engines, recommendation systems (used by platforms like YouTube and Netflix), virtual assistants (e.g., Google Assistant, Siri, Alexa), autonomous vehicles, generative tools for language and art, and superhuman performance in strategy games such as chess and Go. Many AI applications are so integrated into daily use that they are no longer explicitly recognized as AI.\n\nThe goals of AI research are broken down into subproblems, including reasoning and problem-solving, knowledge representation, planning and decision-making, learning, natural language processing, perception, and social intelligence. Techniques used to achieve these goals include search and mathematical optimization, formal logic, artificial neural networks, and methods based on statistics, operations research, and economics. AI also draws upon fields like psychology, linguistics, philosophy, and neuroscience. Some companies, such as OpenAI and Google DeepMind, are working towards artificial general intelligence (AGI), which aims for AI to perform virtually any cognitive task as well as a human.\n\nAI was established as an academic discipline in 1956, experiencing periods of optimism followed by \"AI winters\" where funding and interest declined. A significant resurgence occurred after 2012 with the adoption of graphics processing units (GPUs) for accelerating neural networks and the improved performance of deep learning. The \"AI boom\" in the 2020s, driven by generative AI and transformer architecture, has led to rapid progress but also raised ethical concerns about misinformation, bias, privacy, copyright, environmental impact, and potential existential risks. This has prompted discussions about regulatory policies to ensure the safety and benefits of AI technology."
                  }
                ],
                "role": "model"
              },
              "finishReason": "STOP",
              "index": 0,
              "groundingMetadata": {
                "groundingChunks": [
                  {
                    "web": {
                      "uri": "https://en.wikipedia.org/wiki/Artificial_intelligence",
                      "title": "Artificial intelligence - Wikipedia"
                    }
                  }
                ],
                "groundingSupports": [
                  {
                    "segment": {
                      "startIndex": 1,
                      "endIndex": 255,
                      "text": "Artificial intelligence (AI) is a field of computer science focused on creating computational systems capable of performing tasks typically associated with human intelligence, such as learning, reasoning, problem-solving, perception, and decision-making."
                    },
                    "groundingChunkIndices": [
                      0
                    ]
                  },
                  {
                    "segment": {
                      "startIndex": 256,
                      "endIndex": 425,
                      "text": "AI research and development also involve studying methods and software that enable machines to perceive their environment and use intelligence to achieve specific goals."
                    },
                    "groundingChunkIndices": [
                      0
                    ]
                  },
                  {
                    "segment": {
                      "startIndex": 427,
                      "endIndex": 778,
                      "text": "High-profile applications of AI are common in modern life, including advanced web search engines, recommendation systems (used by platforms like YouTube and Netflix), virtual assistants (e.g., Google Assistant, Siri, Alexa), autonomous vehicles, generative tools for language and art, and superhuman performance in strategy games such as chess and Go."
                    },
                    "groundingChunkIndices": [
                      0
                    ]
                  },
                  {
                    "segment": {
                      "startIndex": 779,
                      "endIndex": 885,
                      "text": "Many AI applications are so integrated into daily use that they are no longer explicitly recognized as AI."
                    },
                    "groundingChunkIndices": [
                      0
                    ]
                  },
                  {
                    "segment": {
                      "startIndex": 887,
                      "endIndex": 1118,
                      "text": "The goals of AI research are broken down into subproblems, including reasoning and problem-solving, knowledge representation, planning and decision-making, learning, natural language processing, perception, and social intelligence."
                    },
                    "groundingChunkIndices": [
                      0
                    ]
                  },
                  {
                    "segment": {
                      "startIndex": 1119,
                      "endIndex": 1314,
                      "text": "Techniques used to achieve these goals include search and mathematical optimization, formal logic, artificial neural networks, and methods based on statistics, operations research, and economics."
                    },
                    "groundingChunkIndices": [
                      0
                    ]
                  },
                  {
                    "segment": {
                      "startIndex": 1315,
                      "endIndex": 1400,
                      "text": "AI also draws upon fields like psychology, linguistics, philosophy, and neuroscience."
                    },
                    "groundingChunkIndices": [
                      0
                    ]
                  },
                  {
                    "segment": {
                      "startIndex": 1401,
                      "endIndex": 1589,
                      "text": "Some companies, such as OpenAI and Google DeepMind, are working towards artificial general intelligence (AGI), which aims for AI to perform virtually any cognitive task as well as a human."
                    },
                    "groundingChunkIndices": [
                      0
                    ]
                  },
                  {
                    "segment": {
                      "startIndex": 1591,
                      "endIndex": 1739,
                      "text": "AI was established as an academic discipline in 1956, experiencing periods of optimism followed by \"AI winters\" where funding and interest declined."
                    },
                    "groundingChunkIndices": [
                      0
                    ]
                  },
                  {
                    "segment": {
                      "startIndex": 1740,
                      "endIndex": 1918,
                      "text": "A significant resurgence occurred after 2012 with the adoption of graphics processing units (GPUs) for accelerating neural networks and the improved performance of deep learning."
                    },
                    "groundingChunkIndices": [
                      0
                    ]
                  },
                  {
                    "segment": {
                      "startIndex": 1919,
                      "endIndex": 2162,
                      "text": "The \"AI boom\" in the 2020s, driven by generative AI and transformer architecture, has led to rapid progress but also raised ethical concerns about misinformation, bias, privacy, copyright, environmental impact, and potential existential risks."
                    },
                    "groundingChunkIndices": [
                      0
                    ]
                  },
                  {
                    "segment": {
                      "startIndex": 2163,
                      "endIndex": 2270,
                      "text": "This has prompted discussions about regulatory policies to ensure the safety and benefits of AI technology."
                    },
                    "groundingChunkIndices": [
                      0
                    ]
                  }
                ]
              },
              "urlContextMetadata": {
                "urlMetadata": [
                  {
                    "retrievedUrl": "https://en.wikipedia.org/wiki/Artificial_intelligence",
                    "urlRetrievalStatus": "URL_RETRIEVAL_STATUS_SUCCESS"
                  }
                ]
              }
            }
          ],
          "usageMetadata": {
            "promptTokenCount": 22,
            "candidatesTokenCount": 426,
            "totalTokenCount": 59880,
            "promptTokensDetails": [
              {
                "modality": "TEXT",
                "tokenCount": 22
              }
            ],
            "toolUsePromptTokenCount": 59392,
            "toolUsePromptTokensDetails": [
              {
                "modality": "TEXT",
                "tokenCount": 59392
              }
            ],
            "thoughtsTokenCount": 40
          },
          "modelVersion": "gemini-2.5-flash",
          "responseId": "bvq7aInSLPn9nsEP3MKX6A4"
        }
        """;
}

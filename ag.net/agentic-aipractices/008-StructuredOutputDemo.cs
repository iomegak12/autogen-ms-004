using System.Text.Json;
using AutoGen.Core;
using AutoGen.OpenAI;
using AutoGen.OpenAI.Extension;
using Json.Schema;
using Json.Schema.Generation;

namespace AgenticAIPractices;

public static class StructuredOutputDemo
{
    public static async Task RunAsync()
    {
        var deploymentName = LLMConfiguration.GetDeploymentName();
        var azureOpenAIClient = LLMConfiguration.GetAzureOpenAIClient();
        var systemPrompt = @"
            You're a helpful assistant that generates structured output based on user input.
            The output should be in JSON format, adhering to the schema provided.
        ";

        var schemaBuilder = new JsonSchemaBuilder().FromType<People>();
        var schema = schemaBuilder.Build();

        var agent = new OpenAIChatAgent(
            chatClient: azureOpenAIClient.GetChatClient(deploymentName),
            name: "StructuredOutputAgent",
            systemMessage: systemPrompt,
            temperature: 0.2f
        )
            .RegisterMessageConnector()
            .RegisterPrintMessage();

        var promptMessage = new TextMessage(
            Role.User,
            @"
                My name is John Doe, I am 30 years old, my email is jd@email-info.com,
                I live in New York, I work as a Software Engineer, and my hobbies include reading, hiking, and coding.

                My friend name is Eric Lippert, he is 35 years old, his email is eric@email-info.com,
                he lives in Seattle, he works as a Software Architect, and his hobbies include writing, gaming, and traveling.

                Both of use like to play chess and enjoy outdoor activities.

                Please generate a JSON output that adheres to the provided schema.
            "
        );

        var reply = await agent.GenerateReplyAsync(
            messages: [promptMessage],
            options: new GenerateReplyOptions
            {
                OutputSchema = schema,
            }
        );

        var people = JsonSerializer.Deserialize<People>(reply.GetContent());

        foreach (var person in people?.Persons ?? Enumerable.Empty<Person>())
        {
            Console.WriteLine(person);
        }

        Console.WriteLine("Structured output generated successfully.");
    }
}
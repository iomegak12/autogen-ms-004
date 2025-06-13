using AutoGen.Core;
using AutoGen.OpenAI;
using AutoGen.OpenAI.Extension;
using FluentAssertions;

namespace AgenticAIPractices;

public static class ConnectWithAzureOpenAIEval
{
    public static async Task RunAsync()
    {
        var client = LLMConfiguration.GetAzureOpenAIClient();
        var deploymentName = LLMConfiguration.GetDeploymentName();

        var agent = new OpenAIChatAgent(
            chatClient: client.GetChatClient(deploymentName),
            name: "AgenticAIPracticesAgent",
            systemMessage:
                @"You are an AI agent designed that converts what user said into Capitalized text."
        )
            .RegisterMessageConnector()
            .RegisterPrintMessage();

        var userMessage = new TextMessage(
            role: Role.User,
            content: "Hello Microsoft!"
        );

        var response = await agent.SendAsync(userMessage);

        if (response is TextMessage textResponse)
        {
            Console.WriteLine($"Agent Response: {textResponse.GetContent()}");
        }
        else
        {
            Console.WriteLine("Agent did not return a text response.");
        }

        response.Should().BeOfType<TextMessage>();
        response.Should().NotBeNull();
        response.GetContent().Should().Be("HELLO MICROSOFT!");

        Console.WriteLine("Processing completed.");
    }
}
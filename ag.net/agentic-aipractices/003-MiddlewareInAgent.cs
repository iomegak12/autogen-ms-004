using AutoGen.Core;
using AutoGen.OpenAI;
using AutoGen.OpenAI.Extension;
using FluentAssertions;
using OpenAI.Chat;

namespace AgenticAIPractices;

public static class MiddlewareAgentDemo
{
    public static async Task RunAsync()
    {
        var client = LLMConfiguration.GetAzureOpenAIClient();
        var deploymentName = LLMConfiguration.GetDeploymentName();
        var totalTokenCount = 0;

        var agent = new OpenAIChatAgent(
            chatClient: client.GetChatClient(deploymentName),
            name: "AgenticAIPracticesAgent",
            systemMessage:
                @"You are a helpful assistant that provides information and answers questions. 
                You can also tell jokes and engage in casual conversation."
        )
            .RegisterMiddleware(
                async (messages, option, agent, cancellationToken) =>
                {
                    var reply = await agent.GenerateReplyAsync(messages, option, cancellationToken);

                    if (reply is MessageEnvelope<ChatCompletion> chatCompletion)
                    {
                        var tokenCount = chatCompletion.Content.Usage.TotalTokenCount;

                        totalTokenCount += tokenCount;
                        Console.WriteLine($"Total token count: {totalTokenCount}");
                    }

                    return reply;
                }
            )
            .RegisterMiddleware(new OpenAIChatRequestMessageConnector());

        var reply = await agent.SendAsync("Tell me a joke about Batman Joker");

        reply.Should().NotBeNull();
        reply.GetContent().Should().NotBeNull();

        totalTokenCount.Should().BeGreaterThan(0);

        Console.WriteLine($"Final reply: {reply.GetContent()}");
        Console.WriteLine("Program completed successfully.");
    }
}
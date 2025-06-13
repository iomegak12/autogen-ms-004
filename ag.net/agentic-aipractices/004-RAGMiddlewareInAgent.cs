using AutoGen.Core;
using AutoGen.OpenAI;
using AutoGen.OpenAI.Extension;
using FluentAssertions;

namespace AgenticAIPractices;

public static class RAGMiddlewareInAgent
{
    public static async Task RunAsync()
    {
        Console.WriteLine("Running RAG Middleware in Agent...");

        var deploymentName = Utils.GetConfigurationValue("AzureOpenAI:DeploymentName");
        var client = LLMConfiguration.GetAzureOpenAIClient();

        var agent = new OpenAIChatAgent(
            chatClient: client.GetChatClient(deploymentName),
            name: "RAGAgent",
            systemMessage: @"You are a helpful agent that uses RAG (Retrieval-Augmented Generation) to answer questions. 
            You will use the provided context to generate accurate and relevant responses."
        )
            .RegisterMessageConnector()
            .RegisterMiddleware(
                async (messages, option, agent, cancellationToken) =>
                {
                    var today = DateTime.UtcNow.ToString("yyyy-MM-dd");
                    var todayMessage = new TextMessage(
                        role: Role.System,
                        content: @$"Today's date is {today}. 
                            Please use this information to provide context-aware responses."
                    );

                    messages = messages.Concat([todayMessage]);

                    return await agent.GenerateReplyAsync(messages, option, cancellationToken);
                }
            )
            .RegisterPrintMessage();

        var reply = await agent.SendAsync("what is todays date?");

        Console.WriteLine($"Agent Reply: {reply.GetContent()}");

        reply.Should().BeOfType<TextMessage>();

        Console.WriteLine("RAG Middleware in Agent completed successfully.");
    }
}
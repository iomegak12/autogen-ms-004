using AutoGen.Core;
using AutoGen.OpenAI;
using AutoGen.OpenAI.Extension;

namespace AgenticAIPractices;

public static class ConnectWithAzureOpenAI
{
    public static async Task RunAsync()
    {
        var client = LLMConfiguration.GetAzureOpenAIClient();
        var deploymentName = LLMConfiguration.GetDeploymentName();

        var agent = new OpenAIChatAgent(
            chatClient: client.GetChatClient(deploymentName),
            name: "AgenticAIPracticesAgent",
            systemMessage:
                @"You are an AI agent designed to assist with various tasks related to Azure OpenAI and Agentic AI practices. Your goal is to provide accurate and helpful responses based on the user's queries. You can access configuration settings and interact with Azure OpenAI services as needed.
                Please ensure that your responses are clear, concise, and relevant to the user's request."
        )
            .RegisterMessageConnector()
            .RegisterPrintMessage();

        var userMessage = new TextMessage(
            role: Role.User,
            content: "Can you generate a C# code that returns and displays 100th Fibonacci number?"
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

        Console.WriteLine("Processing completed.");
    }
}
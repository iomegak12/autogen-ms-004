using AutoGen.Core;
using AutoGen.SemanticKernel;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;

namespace AgenticAIPractices;

public static class SimpleSKIntegrationWithAGDemo
{
    public static async Task RunAsync()
    {
        var kernel = Kernel.CreateBuilder()
            .AddAzureOpenAIChatCompletion(
                deploymentName: LLMConfiguration.GetDeploymentName(),
                endpoint: Utils.GetConfigurationValue("AzureOpenAI:Endpoint"),
                apiKey: Utils.GetConfigurationValue("AzureOpenAI:ApiKey"))
            .Build();

        var chatAgent = new ChatCompletionAgent
        {
            Kernel = kernel,
            Name = "ChatAgent",
            Description = "A simple chat agent that responds to user queries using Azure OpenAI."
        };

        var skAgent = new SemanticKernelChatCompletionAgent(chatAgent)
            .RegisterMiddleware(new SemanticKernelChatMessageContentConnector())
            .RegisterPrintMessage();

        var reply = await skAgent.SendAsync(
            @"How do you deploy Azure AI Search in Azure?");

        Console.WriteLine("Agent Response:");
        Console.WriteLine(reply.GetContent());

        Console.WriteLine("Application completed. Press any key to exit.");
    }
}
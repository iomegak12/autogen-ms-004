using AutoGen.Anthropic.DTO;
using AutoGen.Core;
using AutoGen.OpenAI;
using AutoGen.OpenAI.Extension;

namespace AgenticAIPractices;

public static class ToolsMiddlewareInAgentEx
{
    public static async Task RunAsync()
    {
        var deploymentName = Utils.GetConfigurationValue("AzureOpenAI:DeploymentName");
        var client = LLMConfiguration.GetAzureOpenAIClient();
        var tools = new Tools();
        var toolsCallMiddleware = new FunctionCallMiddleware(
            functions:
            [
                tools.UpperCaseFunctionContract,
                tools.ConcatFunctionContract,
                tools.CalculateTaxFunctionContract
            ]
        );

        var systemPrompt = @"
            You are a helpful assistant that can use tools to perform tasks.
            You can use the following tools:
            - UpperCase: Converts a given string to uppercase.
            - Concat: Concatenates an array of strings with a specified separator.
            - CalculateTax: Calculates the tax on a given amount based on a specified tax rate.
            NOTE:
            - Apply user specific instructions to the tools output.
        ";

        var agent = new OpenAIChatAgent(
            chatClient: client.GetChatClient(deploymentName),
            name: "ToolsMiddlewareAgent",
            temperature: 0.7f,
            maxTokens: 1000,
            systemMessage: systemPrompt
        )
            .RegisterMessageConnector()
            .RegisterStreamingMiddleware(toolsCallMiddleware)
            .RegisterPrintMessage();

        Console.WriteLine("Running ToolsMiddlewareInAgent Features ...");

        Console.WriteLine("Testing #4");
        var reply4 = await agent.SendAsync(
            @"can you calculate the tax on 1000 with a tax rate of 0.2
                also calculate the tax on 500 with a tax rate of 0.15
                and return the result in a formatted string, but this time use the tools directly");
        Console.WriteLine($"Response: {reply4.GetContent()}");
        var toolCalls = reply4.GetToolCalls();

        foreach (var toolCall in toolCalls!)
        {
            // Process each tool call
            var toolName = toolCall.FunctionName;
            var toolArgs = toolCall.FunctionArguments;

            // Here you can implement the logic to handle the tool calls
            // For example, you can call the corresponding function based on the tool name
            Console.WriteLine($"Processing tool call: {toolName} with arguments: {toolArgs}");
        }

        Console.WriteLine("***************");


        Console.WriteLine("End of ToolsMiddlewareInAgent Features");
    }
}
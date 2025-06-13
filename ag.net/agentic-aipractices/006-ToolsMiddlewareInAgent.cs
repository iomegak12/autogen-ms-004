using AutoGen.Anthropic.DTO;
using AutoGen.Core;
using AutoGen.OpenAI;
using AutoGen.OpenAI.Extension;

namespace AgenticAIPractices;

public static class ToolsMiddlewareInAgent
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
            ],
            functionMap: new Dictionary<string, Func<string, Task<string>>>
            {
                { nameof(tools.UpperCase), tools.UpperCaseWrapper },
                { nameof(tools.Concat), tools.ConcatWrapper },
                { nameof(tools.CalculateTax), tools.CalculateTaxWrapper }
            }
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
            .RegisterMiddleware(toolsCallMiddleware)
            .RegisterPrintMessage();

        Console.WriteLine("Running ToolsMiddlewareInAgent Features ...");

        Console.WriteLine("Testing #1");

        var reply = await agent.SendAsync("can you capitalize the following text: hello world");
        Console.WriteLine($"Response: {reply.GetContent()}");
        Console.WriteLine("***************");

        Console.WriteLine("Testing #2");

        var reply2 = await agent.SendAsync(
            @"can you concatenate these strings: 'hello', 'world', 'from', 'agent'
            with a : as a separator");

        Console.WriteLine($"Response: {reply2.GetContent()}");
        Console.WriteLine("***************");

        Console.WriteLine("Testing #3");
        var reply3 = await agent.SendAsync(
            @"can you calculate the tax on 1000 with a tax rate of 0.2
            and return the result in a formatted string");
        Console.WriteLine($"Response: {reply3.GetContent()}");
        Console.WriteLine("***************");

        Console.WriteLine("Testing #4");
        var reply4 = await agent.SendAsync(
            @"can you calculate the tax on 1000 with a tax rate of 0.2
                also calculate the tax on 500 with a tax rate of 0.15
                and return the result in a formatted string, but this time use the tools directly");
        Console.WriteLine($"Response: {reply4.GetContent()}");
        Console.WriteLine("***************");


        Console.WriteLine("End of ToolsMiddlewareInAgent Features");
    }
}
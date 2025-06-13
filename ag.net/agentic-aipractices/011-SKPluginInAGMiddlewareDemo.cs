using AutoGen.Core;
using AutoGen.OpenAI;
using AutoGen.OpenAI.Extension;
using AutoGen.SemanticKernel;
using Microsoft.SemanticKernel;

namespace AgenticAIPractices;

public static class SKPluginInAGMiddlewareDemo
{
    public static async Task RunAsync()
    {
        var kernel = Kernel.CreateBuilder()
            .AddAzureOpenAIChatCompletion(
                deploymentName: LLMConfiguration.GetDeploymentName(),
                endpoint: Utils.GetConfigurationValue("AzureOpenAI:Endpoint"),
                apiKey: Utils.GetConfigurationValue("AzureOpenAI:ApiKey"))
            .Build();

        var getWeatherFunction = KernelFunctionFactory.CreateFromMethod(
            method: (string location) =>
            {
                return $"The weather in {location} is sunny with a high of 75°F.";
            },
            functionName: "GetWeather",
            description: "Get the current weather for a given location."
        );

        var getWeatherPlugin = kernel.CreatePluginFromFunctions("my_plugin", [getWeatherFunction]);
        var kernelPluginMiddleware = new KernelPluginMiddleware(kernel, getWeatherPlugin);

        var deploymentName = LLMConfiguration.GetDeploymentName();
        var azureOpenAIClient = LLMConfiguration.GetAzureOpenAIClient();

        var systemPrompt = @"
            You are a helpful assistant that can answer questions about the weather.
            You can use the GetWeather function to get the current weather for a location.";

        var agent = new OpenAIChatAgent(
            chatClient: azureOpenAIClient.GetChatClient(deploymentName),
            systemMessage: systemPrompt,
            name: "WeatherAgent",
            temperature: 0.7f,
            maxTokens: 1000
        )
            .RegisterMessageConnector()
            .RegisterMiddleware(kernelPluginMiddleware)
            .RegisterPrintMessage();

        var response = await agent.SendAsync(
            new TextMessage(Role.User, "What is the weather like in New York?"));

        Console.WriteLine("Agent Response:");
        Console.WriteLine(response.GetContent());

        Console.WriteLine("Application completed. Press any key to exit.");
    }
}
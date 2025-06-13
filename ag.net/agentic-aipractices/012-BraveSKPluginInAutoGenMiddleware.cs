using AutoGen.Core;
using AutoGen.SemanticKernel;
using AutoGen.SemanticKernel.Extension;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Plugins.Web;

namespace AgenticAIPractices;

#pragma warning disable SKEXP0050 

public static class BraveSKPluginInAutoGenMiddleware
{
    public static async Task RunAsync()
    {
        var configuration = Configuration.ConfigureAppSettings();
        var openAISettings = new OpenAIOptions();
        var pluginConfguration = new PluginOptions();

        configuration.GetSection("OpenAI").Bind(openAISettings);
        configuration.GetSection("PluginConfig").Bind(pluginConfguration);

        var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.SetMinimumLevel(LogLevel.Warning);
            builder.AddConfiguration(configuration);
            builder.AddConsole();
        });

        var builder = Kernel.CreateBuilder();

        builder.Services.AddSingleton(loggerFactory);
        builder.AddChatCompletionService(openAISettings);
        builder.AddBraveConnector(pluginConfguration, ApiLoggingLevel.ResponseAndRequest);
        builder.Plugins.AddFromType<WebSearchEnginePlugin>();

        var systemPrompt = @"
                You are a helpful agent that can search the web for information using the Brave Search Plugin.
                You can answer questions about current events, find information on specific topics, and provide summaries of web content.
                You can also provide links to the sources of the information you find.
            ";
        var kernel = builder.Build();
        var skAgent = new SemanticKernelAgent(
            kernel: kernel,
            name: "BraveSKPluginInAutoGenMiddleware",
            systemMessage: systemPrompt
        )
            .RegisterMessageConnector()
            .RegisterPrintMessage();

        var response = await skAgent.SendAsync(
            @"Tell me about the ***latest advancements in Agrovolaics Solar Technology in 2025***.
            Summarize the key points and provide links to the sources.");

        Console.WriteLine("Response:");
        Console.WriteLine(response.GetContent());
        Console.WriteLine("End of the Application!");
    }
}
using AutoGen;
using AutoGen.Core;
using AutoGen.OpenAI;
using AutoGen.OpenAI.Extension;
using AutoGen.SemanticKernel;
using AutoGen.SemanticKernel.Extension;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Plugins.Web;

namespace AgenticAIPractices;

#pragma warning disable SKEXP0050 

public static class RoundRobinGroupChatDemo
{
    public static async Task<IAgent> CreateBraveSearchAgentAsync()
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

        var kernel = builder.Build();
        var systemPrompt = @"
                You are a helpful agent that can search the web for information using the Brave Search Plugin.
                You can answer questions about current events, find information on specific topics, and provide summaries of web content.
                You can also provide links to the sources of the information you find.
            ";

        var searchAgent = new SemanticKernelAgent(
            kernel: kernel,
            name: "BraveSearchAgent",
            systemMessage: systemPrompt
        )
            .RegisterMessageConnector()
            .RegisterPrintMessage();

        return searchAgent;
    }

    public static async Task<IAgent> CreateSummarizeAgentAsync()
    {
        var deploymentName = LLMConfiguration.GetDeploymentName();
        var azureOpenAIClient = LLMConfiguration.GetAzureOpenAIClient();
        var systemPrompt = @"
            You're a helpful assistant that can summarize web content.
            Your summarize the contents of the brave web search results and provide concise summaries.
            You can provide concise summaries of articles, reports, and other documents.";

        var summarizeAgent = new OpenAIChatAgent(
            chatClient: azureOpenAIClient.GetChatClient(deploymentName),
            name: "SummarizeAgent",
            systemMessage: systemPrompt,
            temperature: 0.7f,
            maxTokens: 2000
        )
            .RegisterMessageConnector()
            .RegisterPrintMessage();


        return summarizeAgent;
    }

    public static async Task RunAsync()
    {
        var userProxyAgent = new UserProxyAgent(
            name: "UserProxyAgent",
            humanInputMode: HumanInputMode.ALWAYS
        )
            .RegisterPrintMessage();

        var braveSearchAgent = await CreateBraveSearchAgentAsync();
        var summarizeAgent = await CreateSummarizeAgentAsync();
        var agents = new List<IAgent>
        {
            userProxyAgent,
            braveSearchAgent,
            summarizeAgent
        };

        var groupChat = new RoundRobinGroupChat(agents);
        var groupChatManager = new GroupChatManager(groupChat);

        var conversationHistory = await userProxyAgent.InitiateChatAsync(
            receiver: groupChatManager,
            message: @"Tell me about the ***latest advancements in Agrovolaics Solar Technology in 2025***. 
                Summarize the key points and provide links to the sources.",
            maxRound: 10
        );

        Console.WriteLine("Conversation History:");

        foreach (var message in conversationHistory)
        {
            Console.WriteLine($"{message.From}: {message.GetContent()}");
        }

        Console.WriteLine("End of the Application!");
    }
}
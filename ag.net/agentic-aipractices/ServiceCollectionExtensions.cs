#pragma warning disable IDE0073
// The code that's violating the rule is on this line.
// #pragma warning restore IDE0073

#pragma warning disable SKEXP0050
// The code that's violating the rule is on this line.
// #pragma warning restore SKEXP0050


using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Plugins.Web;
using Microsoft.SemanticKernel.Plugins.Web.Brave;
using System.Text.Json;

public static class ServiceCollectionExtensions
{
    internal static IServiceCollection AddChatCompletionService(this IServiceCollection serviceCollection, OpenAIOptions openAIOptions)
    {
        switch (openAIOptions.Source)
        {
            // case "AzureOpenAI":
            //     serviceCollection = serviceCollection.AddAzureOpenAIChatCompletion(openAIOptions.ChatDeploymentName, endpoint: openAIOptions.Endpoint,
            //         apiKey: openAIOptions.ApiKey, serviceId: openAIOptions.ChatModelId);
            //     break;

            case "OpenAI":
                serviceCollection = serviceCollection.AddOpenAIChatCompletion(modelId: openAIOptions.ChatModelId, apiKey: openAIOptions.ApiKey);
                break;

            default:
                throw new ArgumentException($"Invalid source: {openAIOptions.Source}");
        }

        return serviceCollection;
    }
}
public enum ApiLoggingLevel
{
    None = 0,
    RequestOnly = 1,
    ResponseAndRequest = 2,
}

public static class IKernelBuilderExtensions
{
    public static IKernelBuilder AddBraveConnector(this IKernelBuilder kernelBuilder, PluginOptions pluginOptions, ApiLoggingLevel apiLoggingLevel = ApiLoggingLevel.None)
    {
        if (apiLoggingLevel == ApiLoggingLevel.None)
        {
            kernelBuilder.Services.AddSingleton<IWebSearchEngineConnector>(new BraveConnector(pluginOptions.BraveApiKey));
        }
        else
        {
            var client = CreateHttpClient(apiLoggingLevel);
            kernelBuilder.Services.AddSingleton<IWebSearchEngineConnector>(new BraveConnector(pluginOptions.BraveApiKey, client));
        }

        return kernelBuilder;
    }

    public static IKernelBuilder AddChatCompletionService(this IKernelBuilder kernelBuilder, OpenAIOptions openAIOptions, ApiLoggingLevel apiLoggingLevel = ApiLoggingLevel.None)
    {
        switch (openAIOptions.Source)
        {
            // case "AzureOpenAI":
            //     {
            //         if (apiLoggingLevel == ApiLoggingLevel.None)
            //         {
            //             kernelBuilder = kernelBuilder.AddAzureOpenAIChatCompletion(openAIOptions.ChatDeploymentName, endpoint: openAIOptions.Endpoint,
            //                 apiKey: openAIOptions.ApiKey, serviceId: openAIOptions.ChatModelId);
            //         }
            //         else
            //         {
            //             var client = CreateHttpClient(apiLoggingLevel);
            //             kernelBuilder.AddAzureOpenAIChatCompletion(openAIOptions.ChatDeploymentName, openAIOptions.Endpoint, openAIOptions.ApiKey, null, null, client);
            //         }
            //         break;
            //     }
            case "OpenAI":
                {
                    if (apiLoggingLevel == ApiLoggingLevel.None)
                    {
                        kernelBuilder = kernelBuilder.AddOpenAIChatCompletion(modelId: openAIOptions.ChatModelId, apiKey: openAIOptions.ApiKey);
                        break;
                    }
                    else
                    {
                        var client = CreateHttpClient(apiLoggingLevel);
                        kernelBuilder.AddOpenAIChatCompletion(openAIOptions.ChatModelId, openAIOptions.ApiKey, null, null, client);
                    }
                    break;
                }
            default:
                throw new ArgumentException($"Invalid source: {openAIOptions.Source}");
        }

        return kernelBuilder;
    }

    public static HttpClient CreateHttpClient(ApiLoggingLevel apiLoggingLevel)
    {
        HttpClientHandler httpClientHandler;
        if (apiLoggingLevel == ApiLoggingLevel.RequestOnly)
        {
            httpClientHandler = new RequestLoggingHttpClientHandler();
        }
        else
        {
            httpClientHandler = new RequestAndResponseLoggingHttpClientHandler();
        }
        var client = new HttpClient(httpClientHandler);
        return client;
    }
}

// Found most of this implementation via: https://github.com/microsoft/semantic-kernel/issues/5107
public class RequestAndResponseLoggingHttpClientHandler : HttpClientHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (request.Content is not null)
        {
            var content = await request.Content.ReadAsStringAsync(cancellationToken);
            var json = JsonSerializer.Serialize(JsonSerializer.Deserialize<JsonDocument>(content),
                new JsonSerializerOptions { WriteIndented = true });
            System.Console.WriteLine("***********************************************");
            System.Console.WriteLine("Request:");
            System.Console.WriteLine(json);
        }
        else
        {
            System.Console.WriteLine("***********************************************");
            System.Console.WriteLine($"Request: {request.Method} {request.RequestUri}");
        }

        var result = await base.SendAsync(request, cancellationToken);

        if (result.Content is not null)
        {
            var content = await result.Content.ReadAsStringAsync(cancellationToken);
            var json = JsonSerializer.Serialize(JsonSerializer.Deserialize<JsonDocument>(content),
                new JsonSerializerOptions { WriteIndented = true });
            System.Console.WriteLine("***********************************************");
            System.Console.WriteLine("Response:");
            System.Console.WriteLine(json);
        }

        return result;
    }
}
public class RequestLoggingHttpClientHandler : HttpClientHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (request.Content is not null)
        {
            var content = await request.Content.ReadAsStringAsync(cancellationToken);
            var json = JsonSerializer.Serialize(JsonSerializer.Deserialize<JsonDocument>(content),
                new JsonSerializerOptions { WriteIndented = true });
            System.Console.WriteLine("***********************************************");
            System.Console.WriteLine("Request:");
            System.Console.WriteLine(json);
        }
        else
        {
            System.Console.WriteLine("***********************************************");
            System.Console.WriteLine($"Request: {request.Method} {request.RequestUri}");
        }
        return await base.SendAsync(request, cancellationToken);
    }
}

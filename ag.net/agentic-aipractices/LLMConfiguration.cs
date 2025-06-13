using Azure.AI.OpenAI;

namespace AgenticAIPractices;

public static class LLMConfiguration
{
    public static AzureOpenAIClient GetAzureOpenAIClient()
    {
        var apiKey = Utils.GetConfigurationValue("AzureOpenAI:ApiKey");
        var endpoint = Utils.GetConfigurationValue("AzureOpenAI:Endpoint");

        if (string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(endpoint))
        {
            throw new InvalidOperationException("Azure OpenAI API Key or Endpoint is not configured.");
        }

        var client = new AzureOpenAIClient(
            new Uri(endpoint),
            new System.ClientModel.ApiKeyCredential(apiKey)
        );

        return client;
    }

    public static string GetDeploymentName()
    {
        return Utils.GetConfigurationValue("AzureOpenAI:DeploymentName");
    }
}
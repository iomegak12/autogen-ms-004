using AutoGen.Core;
using AutoGen.OpenAI;
using AutoGen.OpenAI.Extension;

namespace AgenticAIPractices;

public static class MultiModalityModelsDemo
{
    public static async Task RunAsync(string fileName = "")
    {
        var deploymentName = LLMConfiguration.GetDeploymentName();
        var azureOpenAIClient = LLMConfiguration.GetAzureOpenAIClient();
        var systemPrompt = @"
            You are a helpful assistant that can process both text and images.
            You're an expert in analyzing images and knows how to apply deep insights 
                and providing detailed descriptions.
            When given an image, describe its content in detail.
            When given text, provide a concise summary or answer the question asked.
        ";

        var agent = new OpenAIChatAgent(
            chatClient: azureOpenAIClient.GetChatClient(deploymentName),
            systemMessage: systemPrompt,
            name: "MultiModalityAgent",
            temperature: 0.7f,
            maxTokens: 1500
        )
            .RegisterMessageConnector()
            .RegisterPrintMessage();

        var imageFileName = string.IsNullOrEmpty(fileName)
            ? "background.png"
            : fileName;

        var imagePath = Path.Combine(
            Environment.CurrentDirectory, "Resources", "Images", imageFileName);

        Console.WriteLine("Processing image: " + imagePath);

        var imageBytes = await File.ReadAllBytesAsync(imagePath);
        var imageMessage = new ImageMessage(
            Role.User, BinaryData.FromBytes(imageBytes, "image/png"));
        var textMessage = new TextMessage(
            Role.User, "What is in this image? Please describe it in detail.");

        var multiModalMessage = new MultiModalMessage(Role.User, [
            imageMessage,
            textMessage
        ]);

        var response = await agent.SendAsync(multiModalMessage);

        Console.WriteLine("Response from MultiModalityAgent:");
        Console.WriteLine(response.GetContent());

        Console.WriteLine("Application complete. Press any key to exit.");
    }
}
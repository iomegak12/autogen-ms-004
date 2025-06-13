using AutoGen.Core;
using AutoGen.OpenAI;
using AutoGen.OpenAI.Extension;
using FluentAssertions;

namespace AgenticAIPractices;

public static class MultiAgentsCommunicationDemo
{
    public static async Task RunAsync()
    {
        var deploymentName = Utils.GetConfigurationValue("AzureOpenAI:DeploymentName");
        var client = LLMConfiguration.GetAzureOpenAIClient();
        var systemPrompt = @"
            You're a teacher who create grad-school random math and slightly complex questions for students and checks answers.
            If the answer is correct, you will say Correct and you stop the conversations by saying COMPLETE.
            If the answer is wrong, you will say Incorrect and you will ask the student to fix it.
            After 3 incorrect answers, you will say that the student is not able to answer the question and you will stop the conversation by saying COMPLETE.";
        var teacherAgent = new OpenAIChatAgent(
            chatClient: client.GetChatClient(deploymentName),
            name: "SuperTeacher",
            systemMessage: systemPrompt,
            temperature: 0.7f
        )
            .RegisterMessageConnector()
            .RegisterMiddleware(
                async (messages, option, agent, cancellationToken) =>
                {
                    var reply = await agent.GenerateReplyAsync(messages, option);

                    if (reply.GetContent()?.ToLower().Contains("complete") == true)
                    {
                        Console.WriteLine("Teacher has completed the conversation.");

                        return new TextMessage(
                            Role.Assistant,
                            GroupChatExtension.TERMINATE,
                            from: reply.From
                        );
                    }

                    return reply;
                }
            )
            .RegisterPrintMessage();

        var studentAgent = new OpenAIChatAgent(
            chatClient: client.GetChatClient(deploymentName),
            name: "SuperStudent",
            systemMessage: @"
                You are an outstanding mathematics student who answers math questions from the teacher.
                If the teacher says your answer is correct, you will say COMPLETE."
        )
            .RegisterMessageConnector()
            .RegisterPrintMessage();

        var conversation = await studentAgent.InitiateChatAsync(
            receiver: teacherAgent,
            message: @"Hello Teacher!, can you give me a random math question?",
            maxRound: 10
        );

        foreach (var message in conversation)
        {
            Console.WriteLine($"{message.From}: {message.GetContent()}");
        }

        conversation.Should().NotBeNull();
        conversation.Count().Should().BeLessThan(12);
        conversation.Last().IsGroupChatTerminateMessage().Should().BeTrue();

        Console.WriteLine("Conversation completed successfully.");
    }
}
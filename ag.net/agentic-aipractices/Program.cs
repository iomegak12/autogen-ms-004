using System;
using System.Threading.Tasks;

namespace AgenticAIPractices
{
    class Program
    {
        static async Task Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Welcome to Agentic AI Practices!");
                Console.WriteLine("Select an option:");
                Console.WriteLine("1. Run ConnectWithAzureOpenAI");
                Console.WriteLine("2. Run ConnectWithAzureOpenAIEval");
                Console.WriteLine("3. Run MiddlewareAgentDemo");
                Console.WriteLine("4. Run RAGMiddlewareInAgent");
                Console.WriteLine("5. Run MultiAgentsCommunicationDemo");
                Console.WriteLine("6. Run ToolsMiddlewareInAgent");
                Console.WriteLine("7. Run ToolsMiddlewareInAgentEx");
                Console.WriteLine("8. Run ToolsMiddlewareInAgentNoInvoke");
                Console.WriteLine("9. Run StructuredOutputDemo");
                Console.WriteLine("10. Run MultiModalityModelsDemo (Background Image)");
                Console.WriteLine("11. Run MultiModalityModelsDemo (US Mortgage Rates Analysis)");
                Console.WriteLine("12. Run SimpleSKIntegrationWithAGDemo");
                Console.WriteLine("13. Run SKPluginInAGMiddlewareDemo");
                Console.WriteLine("14. Run BraveSKPluginInAutoGenMiddleware");
                Console.WriteLine("15. Run RoundRobinGroupChatDemo");

                Console.WriteLine("0. Exit");
                Console.Write("Enter your choice: ");

                var input = Console.ReadLine();

                if (input == "0")
                {
                    Console.WriteLine("Exiting...");
                    break;
                }
                else if (input == "1")
                {
                    Console.WriteLine("Running ConnectWithAzureOpenAI...");
                    await ConnectWithAzureOpenAI.RunAsync();
                }
                else if (input == "2")
                {
                    Console.WriteLine("Running ConnectWithAzureOpenAIEval...");
                    await ConnectWithAzureOpenAIEval.RunAsync();
                }
                else if (input == "3")
                {
                    Console.WriteLine("Running MiddlewareAgentDemo...");
                    await MiddlewareAgentDemo.RunAsync();
                }
                else if (input == "4")
                {
                    Console.WriteLine("Running RAGMiddlewareInAgent...");
                    await RAGMiddlewareInAgent.RunAsync();
                }
                else if (input == "5")
                {
                    Console.WriteLine("Running MultiAgentsCommunicationDemo...");
                    await MultiAgentsCommunicationDemo.RunAsync();
                }
                else if (input == "6")
                {
                    Console.WriteLine("Running ToolsMiddlewareInAgent...");
                    await ToolsMiddlewareInAgent.RunAsync();
                }
                else if (input == "7")
                {
                    Console.WriteLine("Running ToolsMiddlewareInAgentEx...");
                    await ToolsMiddlewareInAgentEx.RunAsync();
                }
                else if (input == "8")
                {
                    Console.WriteLine("Running ToolsMiddlewareInAgentNoInvoke...");
                    await ToolsMiddlewareInAgentNoInvoke.RunAsync();
                }
                else if (input == "9")
                {
                    Console.WriteLine("Running StructuredOutputDemo...");
                    await StructuredOutputDemo.RunAsync();
                }
                else if (input == "10")
                {
                    Console.WriteLine("Running MultiModalityModelsDemo...");
                    await MultiModalityModelsDemo.RunAsync("Background.png");
                }
                else if (input == "11")
                {
                    Console.WriteLine("Running MultiModalityModelsDemo with US Mortgage Rates Analysis...");
                    await MultiModalityModelsDemo.RunAsync("USMortgageRate.png");
                }
                else if (input == "12")
                {
                    Console.WriteLine("Running SimpleSKIntegrationWithAGDemo...");
                    await SimpleSKIntegrationWithAGDemo.RunAsync();
                }
                else if (input == "13")
                {
                    Console.WriteLine("Running SKPluginInAGMiddlewareDemo...");
                    await SKPluginInAGMiddlewareDemo.RunAsync();
                }
                else if (input == "14")
                {
                    Console.WriteLine("Running BraveSKPluginInAutoGenMiddleware...");
                    await BraveSKPluginInAutoGenMiddleware.RunAsync();
                }
                else if (input == "15")
                {
                    Console.WriteLine("Running RoundRobinGroupChatDemo...");
                    await RoundRobinGroupChatDemo.RunAsync();
                }
                else
                {
                    Console.WriteLine("Invalid option. Please try again.");
                }

                Console.WriteLine();
            }
        }
    }
}
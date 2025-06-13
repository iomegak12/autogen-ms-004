import asyncio
import os

from autogen_agentchat.agents import AssistantAgent
from autogen_agentchat.conditions import MaxMessageTermination
from autogen_agentchat.ui import Console
from autogen_agentchat.messages import AgentEvent, ChatMessage, TextMessage
from autogen_ext.models.openai import AzureOpenAIChatCompletionClient
from autogen_core import CancellationToken
from autogen_core.memory import ListMemory, MemoryContent, MemoryMimeType
from autogen_ext.tools.mcp import SseMcpToolAdapter, SseServerParams
from dotenv import load_dotenv

load_dotenv(override=True)

azure_openai_api_key = os.getenv("AZURE_OPENAI_API_KEY")
azure_openai_endpoint = os.getenv("AZURE_OPENAI_ENDPOINT")
azure_openai_deployment_name = os.getenv("AZURE_OPENAI_DEPLOYMENT_NAME")
azure_openai_api_version = os.getenv("AZURE_OPENAI_API_VERSION")

if not all([azure_openai_api_key, azure_openai_endpoint, azure_openai_deployment_name, azure_openai_api_version]):
    raise ValueError("""
        Please set all required environment variables: 
            AZURE_OPENAI_API_KEY, 
            AZURE_OPENAI_ENDPOINT, 
            AZURE_OPENAI_DEPLOYMENT_NAME, 
            AZURE_OPENAI_API_VERSION
        """)

model_client = AzureOpenAIChatCompletionClient(
    azure_deployment=azure_openai_deployment_name,
    model=azure_openai_deployment_name,
    api_key=azure_openai_api_key,
    azure_endpoint=azure_openai_endpoint,
    api_version=azure_openai_api_version,
)

async def main() -> None:
    server_params = SseServerParams(
        url="http://localhost:8004/sse",
        headers={"Content-Type" : "application/json"},
        timeout=60,
    )
    
    adapter =await SseMcpToolAdapter.from_server_params(server_params=server_params,
                                                   tool_name="write_file")
    
    agent = AssistantAgent(
        name="FileWriter",
        model_client=model_client,
        tools=[adapter],
        description="An agent that writes files to a remote server using MCP.",
    )
    
    await Console(
        agent.run_stream(task = """
                         Translate the statement 'Welcome To Autogen!' 
                         into French and write it to a file named '/greeting.txt'.
                         """,
                         cancellation_token=CancellationToken(),
        )
    )
    

if __name__ == "__main__":
    try:
        asyncio.run(main())
    except KeyboardInterrupt:
        print("Process interrupted by user.")
    except Exception as e:
        print(f"An error occurred: {e}")
    finally:
        print("Exiting the program.")
import os
from dotenv import load_dotenv
from autogen_ext.models.openai import AzureOpenAIChatCompletionClient

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


def get_model_client():
    """Initialize and return the Azure OpenAI client."""
    return model_client

from azure.ai.projects import AIProjectClient
from azure.identity import DefaultAzureCredential
from azure.ai.agents.models import ListSortOrder

project = AIProjectClient(
    credential=DefaultAzureCredential(),
    endpoint="https://ramkumaraifoundryv10.services.ai.azure.com/api/projects/ramkumaraifoundryprojectv10")

agent = project.agents.get_agent("asst_83yfPxJRvF3WThYNcQQXLcv9")

thread = project.agents.threads.get("thread_JCPbCtnbkW709ZawR2fh09Of")

message = project.agents.messages.create(
    thread_id=thread.id,
    role="user",
    content="what's the microsoft annual leave policy, and highlight citations with sources"
)

run = project.agents.runs.create_and_process(
    thread_id=thread.id,
    agent_id=agent.id)

if run.status == "failed":
    print(f"Run failed: {run.last_error}")
else:
    messages = project.agents.messages.list(thread_id=thread.id, order=ListSortOrder.ASCENDING)

    for message in messages:
        if message.text_messages:
            print(f"{message.role}: {message.text_messages[-1].text.value}")
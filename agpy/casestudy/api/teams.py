from autogen_agentchat.conditions import TextMentionTermination, MaxMessageTermination
from autogen_agentchat.teams import SelectorGroupChat
from autogen_agentchat.messages import AgentEvent, ChatMessage


from typing import Sequence

from models import get_model_client
from agents import get_planning_agent, get_response_agent, get_order_placement_agent, \
    get_order_status_agent, get_complaint_registration_agent, get_product_inquiry_agent


def custom_selector_func(messages: Sequence[AgentEvent | ChatMessage]) -> str:
    planning_agent = get_planning_agent()

    if messages[-1].source != planning_agent.name:
        return planning_agent.name

    return None


def get_termination_conditions():
    """Return the termination conditions for the team chat."""

    text_termination = TextMentionTermination("TERMINATE")
    max_messages_termination = MaxMessageTermination(10)

    combined_conditions = text_termination | max_messages_termination

    return combined_conditions


planner = get_planning_agent()
reponder = get_response_agent()
order_placement_provider = get_order_placement_agent()
order_status_provider = get_order_status_agent()
complaint_registrar = get_complaint_registration_agent()
product_inquiry_provider = get_product_inquiry_agent()

customer_support_team = SelectorGroupChat(
    [
        planner,
        reponder,
        order_placement_provider,
        complaint_registrar,
        order_status_provider,
        product_inquiry_provider,
    ],
    selector_func=custom_selector_func,
    termination_condition=get_termination_conditions(),
    model_client=get_model_client(),
    allow_repeated_speaker=True,
)


def get_customer_support_team() -> SelectorGroupChat:
    """Return the customer support team chat."""
    return customer_support_team

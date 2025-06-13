from autogen_agentchat.agents import AssistantAgent

from models import get_model_client
from services import product_inquiry_tool, \
    order_placement_tool, order_status_tool, complaint_registration_tool
from prompts import get_planning_agent_prompt, \
    get_response_agent_prompt, get_order_placement_agent_prompt, \
    get_order_status_agent_prompt, get_complaint_registration_agent_prompt, \
    get_product_inquiry_agent_prompt

planning_agent = AssistantAgent(
    name="PlanningAgent",
    model_client=get_model_client(),
    system_message=get_planning_agent_prompt(),
    description="Planning agent to coordinate tasks among other agents.",
)

response_agent = AssistantAgent(
    name="ResponseAgent",
    model_client=get_model_client(),
    system_message=get_response_agent_prompt(),
    description="Response agent to handle user queries and provide responses.",
)

order_placement_agent = AssistantAgent(
    name="OrderPlacementAgent",
    model_client=get_model_client(),
    system_message=get_order_placement_agent_prompt(),
    description="Order placement agent to handle product orders.",
    tools=[order_placement_tool],
)

order_status_agent = AssistantAgent(
    name="OrderStatusAgent",
    model_client=get_model_client(),
    system_message=get_order_status_agent_prompt(),
    description="Order status agent to check order statuses.",
    tools=[order_status_tool],
)

complaint_registration_agent = AssistantAgent(
    name="ComplaintRegistrationAgent",
    model_client=get_model_client(),
    system_message=get_complaint_registration_agent_prompt(),
    description="Complaint registration agent to handle customer complaints.",
    tools=[complaint_registration_tool],
)

product_inquiry_agent = AssistantAgent(
    name="ProductInquiryAgent",
    model_client=get_model_client(),
    system_message=get_product_inquiry_agent_prompt(),
    description="Product inquiry agent to handle product inquiries.",
    tools=[product_inquiry_tool],
)


def get_planning_agent() -> AssistantAgent:
    """Return the planning agent."""
    return planning_agent


def get_response_agent() -> AssistantAgent:
    """Return the response agent."""
    return response_agent


def get_order_placement_agent() -> AssistantAgent:
    """Return the order placement agent."""
    return order_placement_agent


def get_order_status_agent() -> AssistantAgent:
    """Return the order status agent."""
    return order_status_agent


def get_complaint_registration_agent() -> AssistantAgent:
    """
    Retrieves the complaint registration agent instance.

    Returns:
        AssistantAgent: The agent responsible for handling complaint registrations.
    """
    """Return the complaint registration agent."""
    return complaint_registration_agent


def get_product_inquiry_agent() -> AssistantAgent:
    """Return the product inquiry agent."""
    return product_inquiry_agent

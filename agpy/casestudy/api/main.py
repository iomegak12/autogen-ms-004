import asyncio

from teams import get_customer_support_team


async def main(query: str):
    """
    Handles a customer support query asynchronously by delegating the task to the customer support team,
    retrieving the latest response, and processing the result. If the response ends with "TERMINATE",
    the suffix is removed and the result is stripped of leading/trailing whitespace.
    Args:
        query (str): The customer support query to be processed.
    Returns:
        str: The processed response from the customer support team.
    """
    """Main function to handle customer support queries."""
    customer_support_team = get_customer_support_team()
    response = await customer_support_team.run(task=query)
    result = response.messages[-1].content

    if result.endswith("TERMINATE"):
        result = result.removesuffix("TERMINATE").strip()

    return result


if __name__ == "__main__":
    query = "What is the price of Apple iPhone 15 Pro?"
    response = asyncio.run(main(query))

    print(response)

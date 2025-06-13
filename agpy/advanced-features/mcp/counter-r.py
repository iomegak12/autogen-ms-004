import time
import signal
import sys

from mcp.server.fastmcp  import FastMCP

def signal_handler(signum, frame):
    print(f"Signal {signum} received, exiting gracefully...")
    sys.exit(0)
    
signal.signal(signal.SIGINT, signal_handler)

mcp = FastMCP(
    name="counter-r",
    description="Counter with reset",
    timeout=30
)

@mcp.tool()
def counter_r(word: str) -> int:
    """
    Count the number of 'r' characters in the input word.
    """
    
    try:
        if not isinstance(word, str):
            return 0
        
        count = word.lower().count('r')
        
        return count
    except Exception as e:
        print(f"Error processing word: {e}")
        return 0

if __name__ == "__main__":
    print("Starting FastMCP server...")
    mcp.run(transport="stdio")
    print("FastMCP server stopped.")
    

import uvicorn

from fastapi import FastAPI
from pydantic import BaseModel
from main import main as handle_query

app = FastAPI()


class QueryRequest(BaseModel):
    query: str


@app.post("/chat")
async def chat(request: QueryRequest):
    """Handle customer support queries."""
    response = await handle_query(request.query)

    return {"response": response}


if __name__ == "__main__":
    uvicorn.run(app, host="0.0.0.0", port=8000,
                log_level="info", reload=True, workers=1)

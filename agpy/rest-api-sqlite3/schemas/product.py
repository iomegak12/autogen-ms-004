from pydantic import BaseModel

class ProductCreate(BaseModel):
    Title: str
    Description: str
    QuantityAvailable: int
    UnitPrice: float

class ProductRead(BaseModel):
    ProductID: int
    Title: str
    Description: str
    QuantityAvailable: int
    UnitPrice: float

    class Config:
        orm_mode = True

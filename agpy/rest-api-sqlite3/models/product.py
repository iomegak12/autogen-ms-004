from sqlalchemy import Column, Integer, String, Float
from sqlalchemy.ext.declarative import declarative_base

Base = declarative_base()

class Product(Base):
    __tablename__ = 'products'
    ProductID = Column(Integer, primary_key=True, autoincrement=True)
    Title = Column(String, nullable=False)
    Description = Column(String)
    QuantityAvailable = Column(Integer, nullable=False)
    UnitPrice = Column(Float, nullable=False)

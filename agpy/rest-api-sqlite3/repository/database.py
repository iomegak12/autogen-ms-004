from sqlalchemy import create_engine
from sqlalchemy.orm import sessionmaker
from models.product import Base

DATABASE_URL = "sqlite:///products.db"

engine = create_engine(DATABASE_URL, connect_args={"check_same_thread": False})
SessionLocal = sessionmaker(autocommit=False, autoflush=False, bind=engine)

# Create tables
Base.metadata.create_all(bind=engine)

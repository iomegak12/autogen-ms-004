from sqlalchemy.orm import Session
from models.product import Product
from schemas.product import ProductCreate

class ProductRepository:
    def __init__(self, db: Session):
        self.db = db

    def add_product(self, product: ProductCreate):
        db_product = Product(**product.dict())
        self.db.add(db_product)
        self.db.commit()
        self.db.refresh(db_product)
        return db_product

    def get_product_by_id(self, product_id: int):
        return self.db.query(Product).filter(Product.ProductID == product_id).first()

    def get_products_by_name(self, name: str):
        return self.db.query(Product).filter(Product.Title.ilike(f"%{name}%")).all()

from repository.database import SessionLocal
from repository.product_repository import ProductRepository
from schemas.product import ProductCreate

# Create a new database session
session = SessionLocal()

# Initialize the repository
repo = ProductRepository(session)

# Add a sample product
sample_product = ProductCreate(
    Title="Sample Widget",
    Description="A sample widget for demonstration purposes.",
    QuantityAvailable=100,
    UnitPrice=9.99
)
added_product = repo.add_product(sample_product)
print(f"Added Product: {added_product.ProductID}, {added_product.Title}")

# Retrieve the product by partial name
products = repo.get_products_by_name("Widget")
for product in products:
    print(f"Found Product: {product.ProductID}, {product.Title}, {product.Description}, {product.QuantityAvailable}, {product.UnitPrice}")

# Close the session
session.close()

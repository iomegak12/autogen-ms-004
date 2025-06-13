using System;
using System.Collections.Generic;
using System.Linq;

namespace ProductLibrary
{
    public class InMemoryProductService : IProductService
    {
        private readonly List<Product> _products;

        public InMemoryProductService()
        {
            _products = new List<Product>();
            var random = new Random();
            for (int i = 1; i <= 100; i++)
            {
                _products.Add(new Product
                {
                    ProductId = i,
                    Title = $"Medical Product {i}",
                    QuantityAvailable = random.Next(1, 500),
                    Description = $"Description for Medical Product {i}",
                    UnitPrice = Math.Round((decimal)(random.NextDouble() * 100 + 1), 2)
                });
            }
        }

        public IEnumerable<Product> GetProductsByName(string name)
        {
            return _products.Where(p => p.Title.Contains(name, StringComparison.OrdinalIgnoreCase));
        }

        public Product GetProductDetails(int productId)
        {
            return _products.FirstOrDefault(p => p.ProductId == productId);
        }
    }
}

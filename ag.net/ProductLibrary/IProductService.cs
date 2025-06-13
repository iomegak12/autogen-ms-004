using System.Collections.Generic;

namespace ProductLibrary
{
    public interface IProductService
    {
        IEnumerable<Product> GetProductsByName(string name);
        Product GetProductDetails(int productId);
    }
}

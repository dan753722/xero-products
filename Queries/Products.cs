using System;
using Products.Models;

namespace Products.Queries
{
  public class ProductsQuery
  {
    public Models.Products GetProducts()
    {
      return new Models.Products();
    }

    public Product GetProduct(Guid id)
    {
      var product = new Product(id);
      if (product.IsNew) {
        return null;
      }
      return product;
    }
  }
}
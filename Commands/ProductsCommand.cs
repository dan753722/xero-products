using System;
using Products.Models;

namespace Products.Commands
{
  public class ProductsCommand
  {
    public void CreateProduct(Product product)
    {
      product.Save();
    }

    public void UpdateProduct(Guid id, Product product)
    {
      var orig = new Product(id)
      {
        Name = product.Name,
        Description = product.Description,
        Price = product.Price,
        DeliveryPrice = product.DeliveryPrice
      };

      orig.Save();
    }

    public void DeleteProduct(Guid id)
    {
      var product = new Product(id);
      product.Delete();
    }
  }
}

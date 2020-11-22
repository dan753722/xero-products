using System;
using Products.Models;

namespace Products.Commands
{
  public interface IProductsCommand
  {
    void CreateProduct(Product product);
    void UpdateProduct(Guid id, Product product);
    void DeleteProduct(Guid id);
  }
}
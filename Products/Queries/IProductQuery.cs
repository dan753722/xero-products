using System;
using Products.Models;

namespace Products.Queries
{
  public interface IProductsQuery
  {
    Models.Products GetProducts();
    Product GetProduct(Guid id);
  }
}
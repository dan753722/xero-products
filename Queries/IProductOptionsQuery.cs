using System;
using Products.Models;

namespace Products.Queries
{
  public interface IProductOptionsQuery
  {
    ProductOptions GetOptions(Guid productId);
    ProductOption GetOption(Guid productId, Guid id);
  }
}
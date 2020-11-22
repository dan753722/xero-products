using System;
using Products.Models;

namespace Products.Queries
{
  public class ProductOptionsQuery : IProductOptionsQuery
  {
    public ProductOptions GetOptions(Guid productId)
    {
      return new ProductOptions(productId);
    }

    public ProductOption GetOption(Guid productId, Guid id)
    {
      var option = new ProductOption(id);
      if (option.IsNew) {
        return null;
      }

      return option;
    }
  }
}
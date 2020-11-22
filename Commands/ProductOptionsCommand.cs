using System;
using Products.Models;

namespace Products.Commands
{
  public class ProductOptionsCommand
  {
    public void CreateOption(Guid productId, ProductOption option)
    {
      option.ProductId = productId;
      option.Save();
    }

    public void UpdateOption(Guid id, ProductOption option)
    {
      var orig = new ProductOption(id)
      {
        Name = option.Name,
        Description = option.Description
      };

      orig.Save();
    }

    public void DeleteOption(Guid id)
    {
      var opt = new ProductOption(id);
      opt.Delete();
    }
  }
}
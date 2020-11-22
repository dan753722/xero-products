using System;
using Products.Models;

namespace Products.Commands
{
  public interface IProductOptionsCommand
  {
    void CreateOption(Guid productId, ProductOption option);
    void UpdateOption(Guid id, ProductOption option);
    void DeleteOption(Guid id);
  }
}
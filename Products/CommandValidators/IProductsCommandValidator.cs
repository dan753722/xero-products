using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Products.Models;

namespace Products.CommandValidators
{
  public interface IProductsCommandValidator
  {
    public List<Error> ValidateUpdateProduct(Guid id, Product product);
    
  }
}
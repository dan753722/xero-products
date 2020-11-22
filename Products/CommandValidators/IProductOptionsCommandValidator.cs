using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Products.Models;

namespace Products.CommandValidators
{
  public interface IProductOptionsCommandValidator
  {
    List<Error> ValidateCreateOption(Guid productId, ProductOption option);

    List<Error> ValidateUpdateOption(Guid id, ProductOption option);
  }
}
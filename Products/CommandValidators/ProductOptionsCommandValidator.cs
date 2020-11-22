using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Products.Models;

namespace Products.CommandValidators
{
  public class ProductOptionsCommandValidator: IProductOptionsCommandValidator
  {
    public List<Error> ValidateCreateOption(Guid productId, ProductOption option)
    {
      var errors = new List<Error>();
      var product = new Product(productId);
      if (product.IsNew)
      {
        errors.Add(new Error
        {
          Status = StatusCodes.Status404NotFound,
          Message = "Product not found",
        });
        return errors;
      }
      return errors;
    }

    public List<Error> ValidateUpdateOption(Guid id, ProductOption option)
    {
      var errors = new List<Error>();
      var orig = new ProductOption(id);
      if (orig.IsNew) {
        errors.Add(new Error
        {
          Status = StatusCodes.Status404NotFound,
          Message = "Option not found",
        });
        return errors;
      }
      return errors;
    }
  }
}
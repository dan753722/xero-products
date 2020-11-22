using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Products.Models;

namespace Products.CommandValidators
{
  public class ProductsCommandValidator
  {
    public List<Error> ValidateUpdateProduct(Guid id, Product product)
    {
      var errors = new List<Error>();
      var orig = new Product(id)
      {
        Name = product.Name,
        Description = product.Description,
        Price = product.Price,
        DeliveryPrice = product.DeliveryPrice
      };
      if (orig.IsNew) {
        errors.Add(new Error
        {
          Status = StatusCodes.Status404NotFound,
          Message = "Product not found",
        });
      }
      return errors;
    }
  }
}
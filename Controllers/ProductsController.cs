using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Products.Models;
using Products.Queries;
using Products.Commands;
using Products.CommandValidators;
using System.Linq;
using Newtonsoft.Json;

namespace Products.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private ProductsQuery productsQuery;
        private ProductsQuery ProductsQuery
        {
            get
            {
                if (productsQuery == null) {
                    productsQuery = new ProductsQuery();
                }
                return productsQuery;
            }
        }

        private ProductOptionsQuery productOptionsQuery;
        private ProductOptionsQuery ProductOptionsQuery
        {
            get
            {
                if (productOptionsQuery == null) {
                    productOptionsQuery = new ProductOptionsQuery();
                }
                return productOptionsQuery;
            }
        }

        private ProductsCommand productsCommand;
        private ProductsCommand ProductsCommand
        {
            get
            {
                if (productsCommand == null) {
                    productsCommand = new ProductsCommand();
                }
                return productsCommand;
            }
        }

        private ProductsCommandValidator productsCommandValidator;
        private ProductsCommandValidator ProductsCommandValidator
        {
            get
            {
                if (productsCommandValidator == null) {
                    productsCommandValidator = new ProductsCommandValidator();
                }
                return productsCommandValidator;
            }
        }

        private ProductOptionsCommand productOptionsCommand;
        private ProductOptionsCommand ProductOptionsCommand
        {
            get
            {
                if (productOptionsCommand == null) {
                    productOptionsCommand = new ProductOptionsCommand();
                }
                return productOptionsCommand;
            }
        }

        private ProductOptionsCommandValidator productOptionsCommandValidator;
        private ProductOptionsCommandValidator ProductOptionsCommandValidator
        {
            get
            {
                if (productOptionsCommandValidator == null) {
                    productOptionsCommandValidator = new ProductOptionsCommandValidator();
                }
                return productOptionsCommandValidator;
            }
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult Get()
        {
            return Ok(ProductsQuery.GetProducts());
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Get(Guid id)
        {
            var product = ProductsQuery.GetProduct(id);
            if (product == null)
                return NotFound();

            return Ok(product);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult Post(Product product)
        {
            ProductsCommand.CreateProduct(product);
            return NoContent();
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult Update(Guid id, Product product)
        {
            var errors = ProductsCommandValidator.ValidateUpdateProduct(id, product);
            if (errors.Count == 0) {
                ProductsCommand.UpdateProduct(id, product);
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult Delete(Guid id)
        {
            ProductsCommand.DeleteProduct(id);
            return NoContent();
        }

        [HttpGet("{productId}/options")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetOptions(Guid productId)
        {
            return Ok(ProductOptionsQuery.GetOptions(productId));
        }

        [HttpGet("{productId}/options/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetOption(Guid productId, Guid id)
        {
            var option = ProductOptionsQuery.GetOption(productId, id);
            if (option == null)
                return NotFound();

            return Ok(option);
        }

        [HttpPost("{productId}/options")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult CreateOption(Guid productId, ProductOption option)
        {
            var results = ProductOptionsCommandValidator.ValidateCreateOption(productId, option);
            if (results.Count != 0) {
                var error = results.First();
                if (error.Status == StatusCodes.Status404NotFound)
                {
                    return NotFound();
                }
                throw new Exception(JsonConvert.SerializeObject(error));
            }

            ProductOptionsCommand.CreateOption(productId, option);
            return NoContent();
        }

        [HttpPut("{productId}/options/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateOption(Guid id, ProductOption option)
        {

            var results = ProductOptionsCommandValidator.ValidateUpdateOption(id, option);
            if (results.Count != 0) {
                var error = results.First();
                if (error.Status == StatusCodes.Status404NotFound)
                {
                    return NotFound();
                }
                throw new Exception(JsonConvert.SerializeObject(error));
            }

            ProductOptionsCommand.UpdateOption(id, option);
            return NoContent();
        }

        [HttpDelete("{productId}/options/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]

        public IActionResult DeleteOption(Guid id)
        {
            ProductOptionsCommand.DeleteOption(id);
            return NoContent();
        }
    }
}
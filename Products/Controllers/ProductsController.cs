using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Products.Models;
using Products.Queries;
using Products.Commands;
using Products.CommandValidators;
using System.Linq;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;

namespace Products.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        public ProductsController(
            IProductsQuery productsQuery,
            IProductOptionsQuery productOptionsQuery,
            IProductsCommand productsCommand,
            IProductsCommandValidator productsCommandValidator,
            IProductOptionsCommand productOptionsCommand,
            IProductOptionsCommandValidator productOptionsCommandValidator
        )
        {
            _productsQuery = productsQuery;
            _productOptionsQuery = productOptionsQuery;
            _productsCommand = productsCommand;
            _productOptionsCommand = productOptionsCommand;
            _productOptionsCommandValidator = productOptionsCommandValidator;
            _productsCommandValidator = productsCommandValidator;
        }

        private readonly IProductsQuery _productsQuery;

        private readonly IProductOptionsQuery _productOptionsQuery;

        private readonly IProductsCommand _productsCommand;

        private readonly IProductsCommandValidator _productsCommandValidator;

        private readonly IProductOptionsCommand _productOptionsCommand;

        private readonly IProductOptionsCommandValidator _productOptionsCommandValidator;

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult Get()
        {
            return Ok(_productsQuery.GetProducts());
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Get(Guid id)
        {
            var product = _productsQuery.GetProduct(id);
            if (product == null)
                return NotFound();

            return Ok(product);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult Post(Product product)
        {
            _productsCommand.CreateProduct(product);
            return NoContent();
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult Update(Guid id, Product product)
        {
            var errors = _productsCommandValidator.ValidateUpdateProduct(id, product);
            if (errors.Count == 0) {
                _productsCommand.UpdateProduct(id, product);
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult Delete(Guid id)
        {
            _productsCommand.DeleteProduct(id);
            return NoContent();
        }

        [HttpGet("{productId}/options")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetOptions(Guid productId)
        {
            return Ok(_productOptionsQuery.GetOptions(productId));
        }

        [HttpGet("{productId}/options/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetOption(Guid productId, Guid id)
        {
            var option = _productOptionsQuery.GetOption(productId, id);
            if (option == null)
                return NotFound();

            return Ok(option);
        }

        [HttpPost("{productId}/options")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult CreateOption(Guid productId, ProductOption option)
        {
            var results = _productOptionsCommandValidator.ValidateCreateOption(productId, option);
            if (results.Count != 0) {
                var error = results.First();
                if (error.Status == StatusCodes.Status404NotFound)
                {
                    return NotFound();
                }
                throw new Exception(JsonConvert.SerializeObject(error));
            }

            _productOptionsCommand.CreateOption(productId, option);
            return NoContent();
        }

        [HttpPut("{productId}/options/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateOption(Guid id, ProductOption option)
        {

            var results = _productOptionsCommandValidator.ValidateUpdateOption(id, option);
            if (results.Count != 0) {
                var error = results.First();
                if (error.Status == StatusCodes.Status404NotFound)
                {
                    return NotFound();
                }
                throw new Exception(JsonConvert.SerializeObject(error));
            }

            _productOptionsCommand.UpdateOption(id, option);
            return NoContent();
        }

        [HttpDelete("{productId}/options/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]

        public IActionResult DeleteOption(Guid id)
        {
            _productOptionsCommand.DeleteOption(id);
            return NoContent();
        }
    }
}
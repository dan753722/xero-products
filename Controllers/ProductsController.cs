using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Products.Models;
using Products.Queries;

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
        public void Post(Product product)
        {
            product.Save();
        }

        [HttpPut("{id}")]
        public void Update(Guid id, Product product)
        {
            var orig = new Product(id)
            {
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                DeliveryPrice = product.DeliveryPrice
            };

            if (!orig.IsNew)
                orig.Save();
        }

        [HttpDelete("{id}")]
        public void Delete(Guid id)
        {
            var product = new Product(id);
            product.Delete();
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
        public void CreateOption(Guid productId, ProductOption option)
        {
            option.ProductId = productId;
            option.Save();
        }

        [HttpPut("{productId}/options/{id}")]
        public void UpdateOption(Guid id, ProductOption option)
        {
            var orig = new ProductOption(id)
            {
                Name = option.Name,
                Description = option.Description
            };

            if (!orig.IsNew)
                orig.Save();
        }

        [HttpDelete("{productId}/options/{id}")]
        public void DeleteOption(Guid id)
        {
            var opt = new ProductOption(id);
            opt.Delete();
        }
    }
}
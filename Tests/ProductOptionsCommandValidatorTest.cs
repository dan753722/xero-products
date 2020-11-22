using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Products.CommandValidators;
using Products.Models;
using Products.Queries;

namespace Tests
{
    [TestClass]
    public class ProductOptionsCommandValidatorTest
    {
        private IProductOptionsQuery productOptionsQuery = new ProductOptionsQuery();
        private IProductsQuery productsQuery = new ProductsQuery();
        [TestMethod]
        public void TestShouldCreateOption()
        {
            var products = productsQuery.GetProducts();
            var product = products.Items.First();

            var productOptionsCommandValidator = new ProductOptionsCommandValidator();
            var results = productOptionsCommandValidator.ValidateCreateOption(product.Id, new ProductOption
            {
                Name = "Navi",
                Description = "Navi colour",
            });

            Assert.IsTrue(results.Count == 0);
        }

        [TestMethod]
        public void TestShouldNotCreateOption()
        {
            var products = productsQuery.GetProducts();
            var product = products.Items.First();

            var productOptionsCommandValidator = new ProductOptionsCommandValidator();
            var results = productOptionsCommandValidator.ValidateCreateOption(new Guid(), new ProductOption
            {
                Name = "Navi",
                Description = "Navi colour",
            });

            Assert.IsTrue(results.Count == 1);
            var result = results.First();
            Assert.AreEqual(result.Status, 404);
        }

        [TestMethod]
        public void TestShouldUpdateOption()
        {
            var products = productsQuery.GetProducts();
            var product = products.Items.First();
            var productOptions = productOptionsQuery.GetOptions(product.Id);

            var productOptionsCommandValidator = new ProductOptionsCommandValidator();
            var results = productOptionsCommandValidator.ValidateUpdateOption(productOptions.Items.First().Id, new ProductOption
            {
                Name = "Navi",
                Description = "Navi colour",
            });

            Assert.IsTrue(results.Count == 0);
        }

        [TestMethod]
        public void TestShouldNotUpdateOption()
        {
            var products = productsQuery.GetProducts();
            var product = products.Items.First();
            var productOptions = productOptionsQuery.GetOptions(product.Id);

            var productOptionsCommandValidator = new ProductOptionsCommandValidator();
            var results = productOptionsCommandValidator.ValidateUpdateOption(product.Id, new ProductOption
            {
                Name = "Navi",
                Description = "Navi colour",
            });

            Assert.IsTrue(results.Count == 1);
            var result = results.First();
            Assert.AreEqual(result.Status, 404);
        }

    }
}

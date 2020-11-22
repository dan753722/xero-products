using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Products.CommandValidators;
using Products.Models;
using Products.Queries;

namespace Tests
{
    [TestClass]
    public class ProductsCommandValidatorTest
    {
        private IProductsQuery productsQuery = new ProductsQuery();
        [TestMethod]
        public void TestShouldUpdateProduct()
        {
            var products = productsQuery.GetProducts();
            var product = products.Items.First();

            var productsCommandValidator = new ProductsCommandValidator();
            var results = productsCommandValidator.ValidateUpdateProduct(product.Id, product);

            Assert.IsTrue(results.Count == 0);
        }

        [TestMethod]
        public void TestShouldNotUpdateProduct()
        {
            var products = productsQuery.GetProducts();
            var product = products.Items.First();

            var productsCommandValidator = new ProductsCommandValidator();
            var results = productsCommandValidator.ValidateUpdateProduct(new Guid(), product);

            Assert.IsTrue(results.Count == 1);
            var result = results.First();
            Assert.AreEqual(result.Status, 404);
        }
    }
}

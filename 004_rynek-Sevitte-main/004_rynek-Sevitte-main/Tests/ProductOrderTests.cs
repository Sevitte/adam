using ClassLibrary2;
using ClassLibrary2.Products;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using NSubstitute;

namespace Tests
{
    [TestFixture]
    class ProductOrderTests
    {
        [TestCase(1.0, 0.3, ExpectedResult = 1.3)]
        //[TestCase(656_443.45, 0.3, ExpectedResult = 85_337_648.5)]
        public decimal TotalValueIsCorrect(double productValue, double profitMargin)
        {
            //var product = new MockProduct((decimal)productValue);
            var product = Substitute.For<IProduct>();
                product.Value.Returns((decimal)productValue);
            var productOrder = new ProductOrder(product, (decimal)profitMargin);
            return productOrder.TotalValue;
        }

        [Test]
        public void ConstructorThrowsWhenProfitMarginIsNegative()
        {

            var product = new MockProduct(656443.45m);
            Assert.That(() => new ProductOrder(product, -0.3m),
                Throws.ArgumentException.And.Property("ParamName").EqualTo("profitMargin"));
        }

        [Test]
        public void ConstructorThrowsWhenProductIsNull()
        {
            Assert.That(() => new ProductOrder(null, 0.3m),
                Throws.ArgumentNullException.And.Property("ParamName").EqualTo("product"));
        }

        class MockProduct : Product
        {
            public MockProduct(decimal value) : base(Guid.NewGuid(), "<MOCKED PRODUCT>", value)
            {
            }
        }
    }
}

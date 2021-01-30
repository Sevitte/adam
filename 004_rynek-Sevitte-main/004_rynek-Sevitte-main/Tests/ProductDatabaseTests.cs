using ClassLibrary2;
using ClassLibrary2.Data;
using ClassLibrary2.Products;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tests
{
    [TestFixture]
    class ProductDatabaseTests
    {
        [Test]
        public void AddSimpleProductOrder()
        {
            var db = new ProductDatabase();
            var productOrder = new ProductOrder(new MockProduct(3), 0.3m);
            var productId = productOrder.Product.Id;
            db.Insert(productOrder);

            Assert.That(db.TryGetProduct(productId, out _), Is.True);
            Assert.That(db.GetCurrentPrice(productId), Is.EqualTo(3.9m));
        }

        [Test]
        public void SubscribersAreNotifiedAboutPriceChange()
        {
            var db = new ProductDatabase();
            // Add first subscriber
            var subscriber1 = new MockSubscriber("Subscriber1");
                subscriber1.Subscribe(db);

            // 0 - Add first price ever
            var product = new MockProduct(3);
            var productId = product.Id;
            var productOrder = new ProductOrder(product, 0.3m);
            db.Insert(productOrder);
            Assert.That(db.GetCurrentPrice(productId), Is.EqualTo(3.9m));
            Assert.That(subscriber1.ChangesReceived.Count, Is.Zero);

            // 1 - Change the price
            productOrder = new ProductOrder(product, 0.1m);
            db.Insert(productOrder);
            Assert.That(db.GetCurrentPrice(productId), Is.EqualTo(3.3m));
            Assert.That(subscriber1.ChangesReceived.Count, Is.EqualTo(1));
            Assert.That(subscriber1.ChangesReceived[^1].NewPrice, Is.EqualTo(3.3m));

            // 2 - Add new subscriber and change the price
            var subscriber2 = new MockSubscriber("Subscriber2");
                subscriber2.Subscribe(db);
            productOrder = new ProductOrder(product, 1m);
            db.Insert(productOrder);
            Assert.That(db.GetCurrentPrice(productId), Is.EqualTo(6m));
            Assert.That(subscriber1.ChangesReceived.Count, Is.EqualTo(2));
            Assert.That(subscriber1.ChangesReceived[^1].NewPrice, Is.EqualTo(6m));
            Assert.That(subscriber2.ChangesReceived.Count, Is.EqualTo(1));
        }

        class MockSubscriber : IObserver<ProductPriceChangeData>
        {
            public string Name { get; }
            public List<ProductPriceChangeData> ChangesReceived { get; } = new List<ProductPriceChangeData>();
            public MockSubscriber(string subscriberName)
            {
                Name = subscriberName;
            }

            public void OnCompleted() => throw new NotImplementedException();
            public void OnError(Exception error) => throw new NotImplementedException();
            public void OnNext(ProductPriceChangeData value)
            {
                ChangesReceived.Add(value);
                var text = $"{Name}: PreviousPrice={value.PreviousPrice}, NewPrice={value.NewPrice}, Product id={value.ProductId}";
                Console.WriteLine(text);
            }

            public void Subscribe(ProductDatabase db)
            {
                db.Subscribe(this);
                Console.WriteLine($"{Name} has subscribed!");
            }
        }

        class MockProduct : Product
        {
            public MockProduct(decimal value) : base(Guid.NewGuid(), "<MOCKED PRODUCT>", value)
            {
            }
        }
    }
}

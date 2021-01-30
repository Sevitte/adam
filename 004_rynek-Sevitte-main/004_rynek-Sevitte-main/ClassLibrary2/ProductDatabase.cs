using ClassLibrary2.Data;
using ClassLibrary2.Products;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClassLibrary2
{
    public class ProductDatabase : IObservable<ProductPriceChangeData>
    {
        private readonly List<IObserver<ProductPriceChangeData>> _priceChangeObservers = new List<IObserver<ProductPriceChangeData>>();

        private readonly Dictionary<Guid, List<ProductPriceChangeData>> _priceHistory = new Dictionary<Guid, List<ProductPriceChangeData>>();

        private readonly Dictionary<Guid, IProduct> _products = new Dictionary<Guid, IProduct>();

        // Method for Removing product order.
        
        public void Remove(ProductOrder productOrder)
        {
            var product = productOrder.Product;
            if (_products.ContainsKey(product.Id))
                _products.Remove(product.Id);
            else
                throw new ArgumentOutOfRangeException("There is no such product");
        }
        public void Insert(ProductOrder productOrder)
        {
            var totalValue = productOrder.TotalValue;
            var product = productOrder.Product;
            AddOrUpdateProduct(product);

            // Get price history for this product
            if (!_priceHistory.TryGetValue(product.Id, out var priceHistory))
            {
                priceHistory = new List<ProductPriceChangeData>();
                _priceHistory.Add(product.Id, priceHistory);
            }
            // Retrieve previous and new price
            bool sendNotification = priceHistory.Count > 0;
            var currentPrice = priceHistory.Count > 0 ? priceHistory[^1].NewPrice : default;
            var newPrice = productOrder.TotalValue;
            var changeData = new ProductPriceChangeData
            {
                ProductId = product.Id,
                PreviousPrice = currentPrice,
                NewPrice = newPrice,
            };

            //Do not send notification, nor add to history, if price hasn't changed.
            priceHistory.Add(changeData);
            if (sendNotification && changeData.NewPrice != changeData.PreviousPrice)
                NotifySubscribers(changeData);
        }

        public decimal GetCurrentPrice(Guid productId) => _priceHistory.TryGetValue(productId, out var priceHistory) ? priceHistory[^1].NewPrice : default;

        private void AddOrUpdateProduct(IProduct product)
        {
            _products[product.Id] = product;
        }

        public bool Contains(Guid productId) => _products.ContainsKey(productId);
        public bool Contains(IProduct product) => _products.ContainsValue(product);

        public bool TryGetProduct(Guid productId, out IProduct product) => _products.TryGetValue(productId, out product);
        public IDisposable Subscribe(IObserver<ProductPriceChangeData> observer)
        {
            if (!_priceChangeObservers.Contains(observer)) 
            {
                _priceChangeObservers.Add(observer);
            }

            return new UnSubscriber<ProductPriceChangeData>(_priceChangeObservers, observer);
        }

        public void NotifySubscribers(ProductPriceChangeData data)
        {
            foreach (var observer in _priceChangeObservers)
            {
                observer.OnNext(data);
            }
        }
    }
}

using ClassLibrary2.Data;
using ClassLibrary2.Products;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClassLibrary2
{
    class Client : IObserver<InflationChangeData>, IObserver<ProductPriceChangeData>
    {
        private IDisposable _unsubscriber;

        public void OnCompleted() => throw new NotImplementedException();
        public void OnError(Exception error) => throw new NotImplementedException();
        public void OnNext(InflationChangeData value) => throw new NotImplementedException();
        public void OnNext(ProductPriceChangeData value) => throw new NotImplementedException();

        public void Subscribe<TData>(IObservable<TData> provider)
        {
            if (_unsubscriber == null)
            {
                _unsubscriber = provider.Subscribe((IObserver<TData>)this);
            }
        }
    }
}

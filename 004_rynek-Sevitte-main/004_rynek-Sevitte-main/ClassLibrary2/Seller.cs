using ClassLibrary2.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClassLibrary2
{
    class Seller : IObserver<InflationChangeData>
    {
        public void OnCompleted() => throw new NotImplementedException();
        public void OnError(Exception error) => throw new NotImplementedException();
        public void OnNext(InflationChangeData value) => throw new NotImplementedException();
    }
}

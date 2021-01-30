using ClassLibrary2.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClassLibrary2
{
    class CentralBank : IObservable<InflationChangeData>
    {
        private readonly List<IObserver<InflationChangeData>> _observers;

        public IDisposable Subscribe(IObserver<InflationChangeData> observer)
        {
            if (!_observers.Contains(observer)) 
            {
                _observers.Add(observer);
            }

            return new UnSubscriber<InflationChangeData>(_observers, observer);
        }
    }
}

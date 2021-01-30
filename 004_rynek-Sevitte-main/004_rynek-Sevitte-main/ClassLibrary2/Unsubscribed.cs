using System;
using System.Collections.Generic;
using System.Text;
using ClassLibrary2.Data;
using ClassLibrary2.Products;

namespace ClassLibrary2
{
	internal class UnSubscriber<T> : IDisposable
	{
		private List<IObserver<T>> lstObservers;
		private IObserver<T> observer;

		internal UnSubscriber(List<IObserver<T>> observersCollection, IObserver<T> observer)
		{
			this.lstObservers = observersCollection;
			this.observer = observer;
		}

		public void Dispose()
		{
			if (this.observer != null)
			{
				lstObservers.Remove(this.observer);
			}
		}
	}
}

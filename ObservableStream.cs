using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SignalRFilterPlay
{
    public class ObservableStream<T>
    {
        private readonly List<IObserver<T>> _observers = new();
        private readonly Timer _timer;
        private readonly Random _random = new();

        public ObservableStream()
        {
            _timer = new Timer(GenerateAndEmitTrade, null, TimeSpan.Zero, TimeSpan.FromSeconds(10));
        }

        private void GenerateAndEmitTrade(object? state)
        {
            if (typeof(T) != typeof(EpexTrade)) return;

            var trade = new EpexTrade
            {
                ContractId = _random.Next(1000, 9999).ToString(),
                ParentMarketArea = GetRandomArea(),
                BuyArea = GetRandomArea(),
                SellArea = GetRandomArea(),
                Price = (decimal)(_random.NextDouble() * 1000),
                Volume = _random.Next(1, 1000),
                TradeTime = DateTime.UtcNow
            };

            foreach (var observer in _observers)
            {
                observer.OnNext((T)(object)trade);
            }
        }

        private string GetRandomArea()
        {
            var areas = new[] { "AT", "DE", "FR", "APG", "TNG", "RTE" };
            return areas[_random.Next(areas.Length)];
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            if (!_observers.Contains(observer))
                _observers.Add(observer);
            
            return new Unsubscriber<T>(_observers, observer);
        }

        private class Unsubscriber<TItem> : IDisposable
        {
            private readonly List<IObserver<TItem>> _observers;
            private readonly IObserver<TItem> _observer;

            public Unsubscriber(List<IObserver<TItem>> observers, IObserver<TItem> observer)
            {
                _observers = observers;
                _observer = observer;
            }

            public void Dispose()
            {
                if (_observer != null && _observers.Contains(_observer))
                    _observers.Remove(_observer);
            }
        }
    }
}

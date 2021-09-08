using System.Collections.Generic;

namespace AI
{
    public class ObservableStat
    {
        private string _statName;
        private int _statValue;
        private StatType _statType;

        private List<ISubscriber> _subscribers = new List<ISubscriber>();

        public ObservableStat( StatType statType, string statName = "")
        {
            _statType = statType;
            _statName = statName == "" ? statType.ToString() : statName;
        }

        public void Supscribe(ISubscriber subscriber)
        {
            _subscribers.Add(subscriber);
        }

        public void Unsubscribe(ISubscriber subscriber)
        {
            _subscribers.Remove(subscriber);
        }

        protected void Notify(StatType dataType)
        {
            foreach (var subscriber in _subscribers)
                subscriber.OnStatUpdate(this, dataType);
        }

        public string StatName => _statName;

        public int StatValue {
            get => _statValue;
            set
            {
                if (_statValue != value)
                {
                    _statValue = value;
                    Notify(_statType);
                }
            }
        }
    }
}
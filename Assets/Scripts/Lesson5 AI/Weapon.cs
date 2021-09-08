using UnityEngine;

namespace AI
{
    public class Weapon : IPublisher
    {
        readonly int _minDamage = 1600;
        readonly int _maxDamage = 3200;

        readonly int _minAccuracy = 25;
        readonly int _maxAccuracy = 55;

        readonly int _minCrit = 10;
        readonly int _maxCrit = 25;

        ObservableStat _damage = new ObservableStat(StatType.Power);
        ObservableStat _accuracy = new ObservableStat(StatType.Accuracy);
        ObservableStat _crit = new ObservableStat(StatType.Crit);

        public void SupscribeToStats(ISubscriber subscriber)
        {
            _accuracy.Supscribe(subscriber);
            _damage.Supscribe(subscriber);
            _crit.Supscribe(subscriber);
        }

        public void GenerateStats()
        {
            _damage.StatValue = Random.Range(_minDamage, _maxDamage + 1);
            _accuracy.StatValue = Random.Range(_minAccuracy, _maxAccuracy + 1);
            _crit.StatValue = Random.Range(_minCrit, _maxCrit + 1);
        }

        public int Damage => _damage.StatValue;
        public int Accuracy => _accuracy.StatValue;
        public int Crit => _crit.StatValue;
    }
}
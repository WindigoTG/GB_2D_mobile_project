using UnityEngine;

namespace AI
{
    public class Mech : IPublisher
    {
        readonly int _minTargeting = 80;
        readonly int _maxTargeting = 120;

        readonly int _minMobility = 80;
        readonly int _maxMobility = 120;

        readonly int _minArmor = 900;
        readonly int _maxArmor = 1600;

        ObservableStat _targeting = new ObservableStat(StatType.Targeting);
        ObservableStat _armor = new ObservableStat(StatType.Armor);
        ObservableStat _mobility = new ObservableStat(StatType.Mobility);

        public void SupscribeToStats(ISubscriber subscriber)
        {
            _targeting.Supscribe(subscriber);
            _armor.Supscribe(subscriber);
            _mobility.Supscribe(subscriber);
        }

        public void GenerateStats()
        {
            _targeting.StatValue = Random.Range(_minTargeting, _maxTargeting + 1);
            _armor.StatValue = Random.Range(_minArmor, _maxArmor + 1);
            _mobility.StatValue = Random.Range(_minMobility, _maxMobility + 1);
        }

        public int Targeting => _targeting.StatValue;
        public int Armor => _armor.StatValue;
        public int Mobility => _mobility.StatValue;
    }
}
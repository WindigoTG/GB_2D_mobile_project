using System.Collections.Generic;

namespace AI
{
    public abstract class PlayerStat
    {
        private string _statName;
        protected int _statValue;

        private List<IEnemy> _enemies = new List<IEnemy>();

        protected PlayerStat(string statName)
        {
            _statName = statName;
        }

        public void Supscribe(IEnemy enemy)
        {
            _enemies.Add(enemy);
        }

        public void Unsubscribe(IEnemy enemy)
        {
            _enemies.Remove(enemy);
        }

        protected void Notify(StatType dataType)
        {
            foreach (var subscriber in _enemies)
                subscriber.UpdateStats(this, dataType);
        }

        public string StatName => _statName;

        public abstract int StatValue { get; set; }
    }
}
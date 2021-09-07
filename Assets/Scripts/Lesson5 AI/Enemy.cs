using UnityEngine;

namespace AI
{
    public class Enemy : IEnemy
    {
        private const int KCoins = 5;
        private const float KPower = 1.5f;
        private const int MaxPlayerHealth = 20;

        private string _name;
        private int _playerMoney;
        private int _playerHealth;
        private int _playerPower;

        public Enemy(string name)
        {
            _name = name;
        }

        public void UpdateStats(PlayerStat playerStat, StatType dataType)
        {
            switch (dataType)
            {
                case StatType.Money:
                    _playerMoney = playerStat.StatValue;
                    break;

                case StatType.Health:
                    _playerHealth = playerStat.StatValue;
                    break;

                case StatType.Power:
                    _playerPower = playerStat.StatValue;
                    break;
            }

            Debug.Log($"Notified {_name} change to {playerStat.StatName}");
        }

        public int Power
        {
            get
            {
                var kHealth = _playerHealth > MaxPlayerHealth ? 100 : 5;
                var power = (int)(_playerMoney / KCoins + kHealth + _playerPower / KPower);

                return power;
            }
        }
    }

}
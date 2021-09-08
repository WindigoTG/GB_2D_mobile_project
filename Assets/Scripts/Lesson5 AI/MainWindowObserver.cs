using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AI
{
    public class MainWindowObserver : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _moneyCountText;

        [SerializeField]
        private TMP_Text _healthCountText;

        [SerializeField]
        private TMP_Text _powerCountText;

        [SerializeField]
        private TMP_Text _enemyPowerCountText;


        [SerializeField]
        private Button _addMoneyButton;

        [SerializeField]
        private Button _subtractMoneyButton;


        [SerializeField]
        private Button _addHealthButton;

        [SerializeField]
        private Button _subtractHealthButton;


        [SerializeField]
        private Button _addPowerButton;

        [SerializeField]
        private Button _subtractPowerButton;

        [SerializeField]
        private Button _fightButton;

        private Money _money;
        private Health _heath;
        private Power _power;

        private Enemy _enemy;

        private void Start()
        {
            _enemy = new Enemy("Enemy");

            _money = new Money(nameof(Money));
            _money.Supscribe(_enemy);

            _heath = new Health(nameof(Health));
            _heath.Supscribe(_enemy);

            _power = new Power(nameof(Power));
            _power.Supscribe(_enemy);

            _addMoneyButton.onClick.AddListener(() => ChangeValue(StatType.Money, true));
            _subtractMoneyButton.onClick.AddListener(() => ChangeValue(StatType.Money, false));

            _addHealthButton.onClick.AddListener(() => ChangeValue(StatType.Health, true));
            _subtractHealthButton.onClick.AddListener(() => ChangeValue(StatType.Health, false));

            _addPowerButton.onClick.AddListener(() => ChangeValue(StatType.Power, true));
            _subtractPowerButton.onClick.AddListener(() => ChangeValue(StatType.Power, false));

            _fightButton.onClick.AddListener(Fight);
        }

        private void OnDestroy()
        {
            _addMoneyButton.onClick.RemoveAllListeners();
            _subtractMoneyButton.onClick.RemoveAllListeners();

            _addHealthButton.onClick.RemoveAllListeners();
            _subtractHealthButton.onClick.RemoveAllListeners();

            _addPowerButton.onClick.RemoveAllListeners();
            _subtractPowerButton.onClick.RemoveAllListeners();

            _fightButton.onClick.RemoveAllListeners();

            _money.Unsubscribe(_enemy);
            _heath.Unsubscribe(_enemy);
            _power.Unsubscribe(_enemy);
        }

        private void ChangeValue(StatType dataType, bool doIncrease)
        {
            PlayerStat stat;

            switch (dataType)
            {
                case StatType.Health:
                    {
                        stat = _heath;
                        break;
                    }
                case StatType.Power:
                    {
                        stat = _power;
                        break;
                    }
                default:
                    {
                        stat = _money;
                        break;
                    }
            }

            if (doIncrease)
                stat.StatValue++;
            else
                stat.StatValue--;

            ChangeDataWindow(stat.StatValue, dataType);
        }

        private void Fight()
        {
            Debug.Log(_power.StatValue >= _enemy.Power
               ? "<color=#07FF00>Win!!!</color>"
               : "<color=#FF0000>Lose!!!</color>");
        }

        private void ChangeDataWindow(int valueToDisplay, StatType dataType)
        {
            switch (dataType)
            {
                case StatType.Money:
                    _moneyCountText.text = $"Player Money {valueToDisplay.ToString()}";
                    break;

                case StatType.Health:
                    _healthCountText.text = $"Player Health {valueToDisplay.ToString()}";
                    break;

                case StatType.Power:
                    _powerCountText.text = $"Player Power {valueToDisplay.ToString()}";
                    break;
            }

            _enemyPowerCountText.text = $"Enemy Power {_enemy.Power}";
        }
    }

}
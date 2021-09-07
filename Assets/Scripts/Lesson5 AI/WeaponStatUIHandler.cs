using System;
using UnityEngine;
using TMPro;

namespace AI
{
    [Serializable]
    public class WeaponStatUIHandler : ISubscriber
    {
        [SerializeField] TMP_Text _damageText;
        [SerializeField] TMP_Text _accuracyText;
        [SerializeField] TMP_Text _critText;

        public void OnStatUpdate(ObservableStat stat, StatType dataType)
        {
            switch (dataType)
            {
                case StatType.Accuracy:
                    _accuracyText.text = $"+{stat.StatValue}";
                    break;

                case StatType.Crit:
                    _critText.text = $"{stat.StatValue}%";
                    break;

                case StatType.Power:
                    _damageText.text = $"{stat.StatValue}";
                    break;

                default:
                    break;
            }
        }
    }
}
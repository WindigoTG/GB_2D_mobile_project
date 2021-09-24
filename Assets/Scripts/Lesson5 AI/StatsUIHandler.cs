using System;
using UnityEngine;
using TMPro;

namespace AI
{
    [Serializable]
    public class StatsUIHandler : ISubscriber
    {
        [SerializeField] TMP_Text _healthText;
        [Header("Pilot")]
        [SerializeField] TMP_Text _meleeStatText;
        [SerializeField] TMP_Text _rangedStatText;
        [SerializeField] TMP_Text _accuracyStatText;
        [SerializeField] TMP_Text _evasionStatText;
        [SerializeField] TMP_Text _defenceStatText;
        [SerializeField] TMP_Text _skillStatText;
        [SerializeField] TMP_Text _moraleStatText;
        [Space]
        [Header("Mech")]
        [SerializeField] TMP_Text _targetingStatText;
        [SerializeField] TMP_Text _armorStatText;
        [SerializeField] TMP_Text _mobilityStatText;
        [Space]
        [Header("Weapon")]
        [SerializeField] WeaponStatUIHandler _meleeWeapon;
        [SerializeField] WeaponStatUIHandler _rangedWeapon;

        public void OnStatUpdate(ObservableStat stat, StatType dataType)
        {
            switch (dataType)
            {
                case StatType.Accuracy:
                    _accuracyStatText.text = $"{stat.StatValue}";
                    break;

                case StatType.Armor:
                    _armorStatText.text = $"{stat.StatValue}";
                    break;

                case StatType.Defence:
                    _defenceStatText.text = $"{stat.StatValue}";
                    break;

                case StatType.Evasion:
                    _evasionStatText.text = $"{stat.StatValue}";
                    break;

                case StatType.Melee:
                    _meleeStatText.text = $"{stat.StatValue}";
                    break;

                case StatType.Mobility:
                    _mobilityStatText.text = $"{stat.StatValue}";
                    break;

                case StatType.Morale:
                    _moraleStatText.text = $"{stat.StatValue}";
                    break;

                case StatType.Ranged:
                    _rangedStatText.text = $"{stat.StatValue}";
                    break;

                case StatType.Skill:
                    _skillStatText.text = $"{stat.StatValue}";
                    break;

                case StatType.Targeting:
                    _targetingStatText.text = $"{stat.StatValue}";
                    break;

                case StatType.Health:
                    _healthText.text = $"{stat.StatValue}";
                    break;

                default:
                    break;
            }
        }

        public WeaponStatUIHandler MeleeWeapon => _meleeWeapon;

        public WeaponStatUIHandler RangedWeapon => _rangedWeapon;
    }
}
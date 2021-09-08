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
                    _accuracyStatText.text = $"Accuracy: {stat.StatValue}";
                    break;

                case StatType.Armor:
                    _armorStatText.text = $"Armor: {stat.StatValue}";
                    break;

                case StatType.Defence:
                    _defenceStatText.text = $"Defence: {stat.StatValue}";
                    break;

                case StatType.Evasion:
                    _evasionStatText.text = $"Evasion: {stat.StatValue}";
                    break;

                case StatType.Melee:
                    _meleeStatText.text = $"Melee: {stat.StatValue}";
                    break;

                case StatType.Mobility:
                    _mobilityStatText.text = $"Mobility: {stat.StatValue}";
                    break;

                case StatType.Morale:
                    _moraleStatText.text = $"Morale: {stat.StatValue}";
                    break;

                case StatType.Ranged:
                    _rangedStatText.text = $"Ranged: {stat.StatValue}";
                    break;

                case StatType.Skill:
                    _skillStatText.text = $"Skill: {stat.StatValue}";
                    break;

                case StatType.Targeting:
                    _targetingStatText.text = $"Targeting: {stat.StatValue}";
                    break;

                case StatType.Health:
                    _healthText.text = $"Targeting: {stat.StatValue}";
                    break;

                default:
                    break;
            }
        }

        public WeaponStatUIHandler MeleeWeapon => _meleeWeapon;

        public WeaponStatUIHandler RangedWeapon => _rangedWeapon;
    }
}
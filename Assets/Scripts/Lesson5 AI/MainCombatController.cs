using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    public class MainCombatController : MonoBehaviour
    {
        [SerializeField] StatsUIHandler _playerStatsDisplay;
        [SerializeField] StatsUIHandler _enemyStatsDisplay;

        [SerializeField] GamePhase _statAssignPhaseWindow;
        [SerializeField] GamePhase _playerPhaseWindow;
        [SerializeField] GamePhase _enemyPhaseWindow;

        Queue<GamePhase> _turnOrder = new Queue<GamePhase>();

        Combatant _player = new PlayerCombatant("Player");
        Combatant _enemy = new EnemyCombatant("Enemy");

        readonly int _abilitiCoeff = 15;
        readonly int _basicPowerCoeff = 200;
        readonly int _basicAccuracyCoeff = 2;
        readonly int _basicCritCoeff = 2;
        readonly int _minDamage = 10;
        readonly float _critMultiplier = 1.25f;

        public bool IsSetupDone { get; private set; }

        private void Awake()
        {
            SubscribeForObservation();

            _statAssignPhaseWindow.Init(_player, _enemy, this);
            _playerPhaseWindow.Init(_player, _enemy, this);
            _enemyPhaseWindow.Init(_player, _enemy, this);

            _player.SetUp();
            _enemy.SetUp();
        }

        private void Start()
        {
            IsSetupDone = true;
        }

        private void SubscribeForObservation()
        {
            _player.Pilot.SupscribeToStats(_playerStatsDisplay);
            _player.Mech.SupscribeToStats(_playerStatsDisplay);
            _player.MeleeWeapon.SupscribeToStats(_playerStatsDisplay.MeleeWeapon);
            _player.RangedWeapon.SupscribeToStats(_playerStatsDisplay.RangedWeapon);
            _player.HitPoints.Supscribe(_playerStatsDisplay);

            _enemy.Pilot.SupscribeToStats(_enemyStatsDisplay);
            _enemy.Mech.SupscribeToStats(_enemyStatsDisplay);
            _enemy.MeleeWeapon.SupscribeToStats(_enemyStatsDisplay.MeleeWeapon);
            _enemy.RangedWeapon.SupscribeToStats(_enemyStatsDisplay.RangedWeapon);
            _enemy.HitPoints.Supscribe(_enemyStatsDisplay);
        }

        public void StartGame()
        {
            _turnOrder.Enqueue(_playerPhaseWindow);
            _turnOrder.Enqueue(_enemyPhaseWindow);
            ShuffleTurnOrder();
            NextPhase();
        }

        private void ShuffleTurnOrder()
        {
            int iterations = Random.Range(0, 20);

            for (int i = 0; i < iterations; i++)
                _turnOrder.Enqueue(_turnOrder.Dequeue());
        }

        private void NextPhase()
        {
            _turnOrder.Enqueue(_turnOrder.Dequeue());
            _turnOrder.Peek().BeginPhase();
        }

        public void ResolveCombatPhase((Combatant combatant, CombatAction action) attacker, (Combatant combatant, CombatAction action) defender)
        {
            CalculateAttack(attacker, defender);

            if (defender.combatant.HitPoints.StatValue <= 0)
            {
                EndGame(attacker, defender);
                return;
            }

            if (defender.action == CombatAction.Melee || defender.action == CombatAction.Ranged)
            {
                Debug.Log($"<color=#FF9900>{defender.combatant.Name} is counter-attacking</color>");

                CalculateAttack(defender, attacker);
            }

            if (attacker.combatant.HitPoints.StatValue <= 0)
            {
                EndGame(defender, attacker);
                return;
            }

            NextPhase();
        }

        private void CalculateAttack((Combatant combatant, CombatAction action) attacker, (Combatant combatant, CombatAction action) defender)
        {
            int accuracy = CalculateAccuracy(attacker, defender);

            if (!CheckIfSuccessful(accuracy))
            {
                Debug.Log($"{defender.combatant.Name} evaded attack");
                attacker.combatant.GainMoraleForAttack(false);
                defender.combatant.GainMoraleForDodge(true);

                return;
            }

            CalculateHitResult(attacker, defender);
        }

        private void CalculateHitResult((Combatant combatant, CombatAction action) attacker, (Combatant combatant, CombatAction action) defender)
        {
            int baseDamage = CalculateBaseDamage(attacker, defender);

            int critChance = CalculateCritChance(attacker, defender);

            bool isCritical = CheckIfSuccessful(critChance);

            int damage = isCritical ? (int)(baseDamage * _critMultiplier) : baseDamage;

            if (isCritical)
                Debug.Log($"<color=#FFFF00>Critical hit!</color>");

            int finalDamage = defender.action == CombatAction.Defence ? damage / 2 : damage;

            Debug.Log($"{defender.combatant.Name} got hit for {finalDamage}");
            defender.combatant.GetDamage(finalDamage);

            attacker.combatant.GainMoraleForAttack(true);
            defender.combatant.GainMoraleForDodge(false);
        }

        private void EndGame((Combatant combatant, CombatAction action) attacker, (Combatant combatant, CombatAction action) defender)
        {
            Debug.Log($"<color=#FF9900>{defender.combatant.Name} was destroyed!</color>");

            string color = defender.combatant == _enemy ? "00FF00" : "FF0000";

            Debug.Log($"<color=#{color}>{attacker.combatant.Name} has won!</color>");

            _turnOrder.Peek().EndPhase();
        }

        private int CalculateBaseDamage((Combatant combatant, CombatAction action) attacker, (Combatant combatant, CombatAction action) defender)
        {
            int damage = CalculateBasicAttackPower(attacker) - CalculateBasicDefencePower(defender);

            return damage > _minDamage ? damage : _minDamage;
        }

        private int CalculateBasicAttackPower((Combatant combatant, CombatAction action) attacker)
        {
            int weaponDamage = attacker.action == CombatAction.Melee ?
                attacker.combatant.MeleeWeapon.Damage :
                attacker.combatant.RangedWeapon.Damage;

            int pilotAbility = attacker.action == CombatAction.Melee ?
                attacker.combatant.Pilot.Melee :
                attacker.combatant.Pilot.Ranged;

            int BAP = weaponDamage * (attacker.combatant.Pilot.Morale + pilotAbility * _abilitiCoeff) / _basicPowerCoeff;

            return BAP;
        }

        private int CalculateBasicDefencePower((Combatant combatant, CombatAction action) defender)
        {
            int BDP = defender.combatant.Mech.Armor *
                (defender.combatant.Pilot.Morale + defender.combatant.Pilot.Defence * _abilitiCoeff) / _basicPowerCoeff;

            return BDP;
        }

        private int CalculateAccuracy((Combatant combatant, CombatAction action) attacker, (Combatant combatant, CombatAction action) defender)
        {
            int accuracy = CalculateBasicAccuracy(attacker) - CalculateBasicEvasion(defender);

            return defender.action == CombatAction.Evasion ?
                accuracy / 2 : accuracy;
        }

        private int CalculateBasicAccuracy((Combatant combatant, CombatAction action) attacker)
        {
            int pilotAbility = attacker.combatant.Pilot.Accuracy * _abilitiCoeff / _basicAccuracyCoeff;

            int weaponAcc = attacker.action == CombatAction.Melee ?
                attacker.combatant.MeleeWeapon.Accuracy :
                attacker.combatant.RangedWeapon.Accuracy;

            int weaponAccBonus = weaponAcc * attacker.combatant.Pilot.Accuracy / _basicAccuracyCoeff;

            int BA = pilotAbility + attacker.combatant.Mech.Targeting + weaponAccBonus;

            return BA;
        }

        private int CalculateBasicEvasion((Combatant combatant, CombatAction action) defender)
        {
            int pilotAbility = defender.combatant.Pilot.Evasion * _abilitiCoeff / _basicAccuracyCoeff;

            int BE = pilotAbility + defender.combatant.Mech.Mobility;

            return BE;
        }

        private int CalculateCritChance((Combatant combatant, CombatAction action) attacker, (Combatant combatant, CombatAction action) defender)
        {
            int attackerAbility = attacker.combatant.Pilot.Skill * _abilitiCoeff / _basicCritCoeff;
            int defenderAbility = defender.combatant.Pilot.Skill * _abilitiCoeff / _basicCritCoeff;
            int weaponCrit = attacker.action == CombatAction.Melee ?
                attacker.combatant.MeleeWeapon.Crit :
                attacker.combatant.RangedWeapon.Crit;

            return attackerAbility - defenderAbility + weaponCrit;
        }

        private bool CheckIfSuccessful(int chanceValue)
        {
            int random = Random.Range(0, 100);

            return random <= chanceValue;
        }
    }
}
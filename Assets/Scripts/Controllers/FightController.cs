using UnityEngine;
using AI;
using Profile;
using System.Collections.Generic;
using UnityEngine.Localization.Settings;

public class FightController : BaseController
{
    private FightView _fightView;
    private PlayerProfile _playerProfile;

    private Queue<GamePhase> _turnOrder = new Queue<GamePhase>();

    private Combatant _player = new PlayerCombatant("Player");
    private Combatant _enemy = new EnemyCombatant("Enemy");

    private readonly int _abilitiCoeff = 15;
    private readonly int _basicPowerCoeff = 200;
    private readonly int _basicAccuracyCoeff = 2;
    private readonly int _basicCritCoeff = 2;
    private readonly int _minDamage = 10;
    private readonly float _critMultiplier = 1.25f;

    public bool IsSetupDone { get; private set; }

    private int _currentLocaleIndex = 0;

    public FightController (AddressableUIWindowsContainer uiPrefabsContainer, PlayerProfile playerProfile)
    {
        CreateAddressablesPrefab<FightView>(uiPrefabsContainer.FightWindowPrefab, uiPrefabsContainer.PlaceForUi, InitializeView);

        _playerProfile = playerProfile;
    }

    private void InitializeView(FightView view)
    {
        _fightView = view;

        _fightView.ButtonLeaveFight.onClick.AddListener(LeaveFight);
        _fightView.ButtonSwitchLocale.onClick.AddListener(SwitchLocale);

        Setup();
    }

   private void Setup()
    {
        SubscribeForObservation();

        _fightView.StatAssignPhaseWindow.Init(_player, _enemy, this);
        _fightView.PlayerPhaseWindow.Init(_player, _enemy, this);
        _fightView.EnemyPhaseWindow.Init(_player, _enemy, this);

        _player.SetUp(_fightView.PlayerViewImage);
        _enemy.SetUp(_fightView.EnemyViewImage);

        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[_currentLocaleIndex];

        IsSetupDone = true;
    }

    private void SubscribeForObservation()
    {
        _player.Pilot.SupscribeToStats(_fightView.PlayerStatsDisplay);
        _player.Mech.SupscribeToStats(_fightView.PlayerStatsDisplay);
        _player.MeleeWeapon.SupscribeToStats(_fightView.PlayerStatsDisplay.MeleeWeapon);
        _player.RangedWeapon.SupscribeToStats(_fightView.PlayerStatsDisplay.RangedWeapon);
        _player.HitPoints.Supscribe(_fightView.PlayerStatsDisplay);

        _enemy.Pilot.SupscribeToStats(_fightView.EnemyStatsDisplay);
        _enemy.Mech.SupscribeToStats(_fightView.EnemyStatsDisplay);
        _enemy.MeleeWeapon.SupscribeToStats(_fightView.EnemyStatsDisplay.MeleeWeapon);
        _enemy.RangedWeapon.SupscribeToStats(_fightView.EnemyStatsDisplay.RangedWeapon);
        _enemy.HitPoints.Supscribe(_fightView.EnemyStatsDisplay);
    }

    public void StartGame()
    {
        _turnOrder.Enqueue(_fightView.PlayerPhaseWindow);
        _turnOrder.Enqueue(_fightView.EnemyPhaseWindow);
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

    #region Combat logic
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
        bool isDefending = defender.action == CombatAction.Defence ? true : false;

        Debug.Log($"{defender.combatant.Name} got hit for {finalDamage}");
        defender.combatant.GetDamage(finalDamage, isDefending);

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

    #endregion

    private void LeaveFight()
    {
        _playerProfile.CurrentState.Value = GameState.Game;
    }

    private void SwitchLocale()
    {
        _currentLocaleIndex = _currentLocaleIndex > 0 ? _currentLocaleIndex - 2 : _currentLocaleIndex + 2;

        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[_currentLocaleIndex];
    }

    protected override void OnDispose()
    {
        base.OnDispose();
        _fightView.ButtonLeaveFight.onClick.RemoveAllListeners();
        _fightView.ButtonSwitchLocale.onClick.RemoveAllListeners();
    }
}



namespace AI
{
    public class PlayerCombatant : Combatant
    {
        public PlayerCombatant(string name) : base(name) { }

        public override void ChangePilotAbilityValue(bool doIncrease, StatType statType)
        {
            _pilot.IncrementOrDecrementStatValue(doIncrease, statType);
        }
    }
}
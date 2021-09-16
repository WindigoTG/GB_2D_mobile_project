using UnityEngine;

namespace AI
{
    public abstract class GamePhase : MonoBehaviour
    {
        protected FightView _combatController;
        protected Combatant _player;
        protected Combatant _enemy;
        protected (Combatant combatant, CombatAction action) _attacker;
        protected (Combatant combatant, CombatAction action) _defender;

        public abstract void Init(Combatant player, Combatant enemy, FightView combatController);
        public abstract void BeginPhase();
        public abstract void EndPhase();
    }
}
using UnityEngine;
using UnityEngine.UI;

namespace AI
{
    public class PlayerPhase : GamePhase
    {
        [SerializeField] Button _meleeAttackButton;
        [SerializeField] Button _rangedAttackButton;

        public override void BeginPhase()
        {
            gameObject.SetActive(true);
            Debug.Log("<color=#FF9900>Player phase</color>");
        }

        public override void EndPhase()
        {
            gameObject.SetActive(false);
        }

        public override void Init(Combatant player, Combatant enemy, FightController fightController)
        {
            _fightController = fightController;

            _player = player;
            _enemy = enemy;

            _meleeAttackButton.onClick.AddListener(() => PerformAttack(CombatAction.Melee));
            _rangedAttackButton.onClick.AddListener(() => PerformAttack(CombatAction.Ranged));
        }

        private void PerformAttack(CombatAction action)
        {
            _attacker = (_player, action);

            _defender = (_enemy, (_enemy as EnemyCombatant).GetDefensiveAction());

            Debug.Log($"{_defender.combatant.Name} uses {_defender.action.ToString()}");

            _fightController.ResolveCombatPhase(_attacker, _defender);

            EndPhase();
        }
    }
}
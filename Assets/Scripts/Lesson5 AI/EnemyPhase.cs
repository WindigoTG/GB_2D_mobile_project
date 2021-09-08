using UnityEngine;
using UnityEngine.UI;

namespace AI
{
    public class EnemyPhase : GamePhase
    {
        [SerializeField] Button _meleeCounterButton;
        [SerializeField] Button _rangedCounterButton;
        [SerializeField] Button _defendButton;
        [SerializeField] Button _evadeButton;

        public override void BeginPhase()
        {
            gameObject.SetActive(true);
            Debug.Log("<color=#FF9900>Enemy phase</color>");
            _attacker = (_enemy, (_enemy as EnemyCombatant).GetAttackType());
            Debug.Log($"{_attacker.combatant.Name} uses {_attacker.action.ToString()}");
        }

        public override void EndPhase()
        {
            gameObject.SetActive(false);
        }

        public override void Init(Combatant player, Combatant enemy, MainCombatController combatController)
        {
            _combatController = combatController;

            _player = player;
            _enemy = enemy;

            _meleeCounterButton.onClick.AddListener(() => PerformPlayerAction(CombatAction.Melee));
            _rangedCounterButton.onClick.AddListener(() => PerformPlayerAction(CombatAction.Ranged));
            _defendButton.onClick.AddListener(() => PerformPlayerAction(CombatAction.Defence));
            _evadeButton.onClick.AddListener(() => PerformPlayerAction(CombatAction.Evasion));
        }

        private void PerformPlayerAction(CombatAction action)
        {
            _defender = (_player, action);

            _combatController.ResolveCombatPhase(_attacker, _defender);

            EndPhase();
        }
    }
}
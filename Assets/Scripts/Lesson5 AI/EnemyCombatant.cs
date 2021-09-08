using System.Collections.Generic;

namespace AI
{
    public class EnemyCombatant : Combatant
    {
        Stack<StatType> _statAssignHistory = new Stack<StatType>();

        readonly int _evadeOnlyTreshold = 25;
        readonly int _defenciveActionsOnlyTreshold = 50;

        public EnemyCombatant(string name) : base(name) { }

        public override void ChangePilotAbilityValue(bool doIncrease, StatType statType)
        {
            if (doIncrease)
            {
                int randomStat = UnityEngine.Random.Range(0, 6);
                StatType type;

                switch (randomStat)
                {
                    case 1:
                        {
                            type = StatType.Defence;
                            break;
                        }
                    case 2:
                        {
                            type = StatType.Evasion;
                            break;
                        }
                    case 3:
                        {
                            type = StatType.Melee;
                            break;
                        }
                    case 4:
                        {
                            type = StatType.Ranged;
                            break;
                        }
                    case 5:
                        {
                            type = StatType.Skill;
                            break;
                        }
                    default:
                        {
                            type = StatType.Accuracy;
                            break;
                        }
                }

                _pilot.IncrementOrDecrementStatValue(true, type);
                _statAssignHistory.Push(type);
            }
            else
                _pilot.IncrementOrDecrementStatValue(false, _statAssignHistory.Pop());
        }

        public CombatAction GetDefensiveAction()
        {
            if (_hitPoints.StatValue * 100 / _maxHitPoints < _evadeOnlyTreshold)
                return CombatAction.Evasion;

            Queue<CombatAction> availableActions = new Queue<CombatAction>();

            availableActions.Enqueue(CombatAction.Defence);
            availableActions.Enqueue(CombatAction.Evasion);

            if (_hitPoints.StatValue * 100 / _maxHitPoints > _defenciveActionsOnlyTreshold)
                availableActions.Enqueue(CombatAction.Attack);

            Shuffle(ref availableActions);

            if (availableActions.Peek() != CombatAction.Attack)
                return availableActions.Dequeue();

            return GetAttackType();
        }

        private void Shuffle(ref Queue<CombatAction> queue)
        {
            int random = UnityEngine.Random.Range(0, 100);

            for (int i = 0; i < random; i++)
            {
                queue.Enqueue(queue.Dequeue());
            }
        }

        public CombatAction GetAttackType()
        {
            bool isMeleeDMGpreferable = _meleeWeapon.Damage > _rangedWeapon.Damage;
            bool isMeleeAbilityPreferable = _pilot.Melee > _pilot.Ranged;
            bool isMeleeAccuracyPreferable = _meleeWeapon.Accuracy > _rangedWeapon.Accuracy;

            if (isMeleeAbilityPreferable && isMeleeAccuracyPreferable && isMeleeDMGpreferable)
                return CombatAction.Melee;

            if (!isMeleeAbilityPreferable && !isMeleeAccuracyPreferable && !isMeleeDMGpreferable)
                return CombatAction.Ranged;

            Queue<CombatAction> availableAttacks = new Queue<CombatAction>();
            CombatAction attack = isMeleeDMGpreferable ? CombatAction.Melee : CombatAction.Ranged;
            availableAttacks.Enqueue(attack);
            attack = isMeleeAbilityPreferable ? CombatAction.Melee : CombatAction.Ranged;
            availableAttacks.Enqueue(attack);
            attack = isMeleeAccuracyPreferable ? CombatAction.Melee : CombatAction.Ranged;
            availableAttacks.Enqueue(attack);

            Shuffle(ref availableAttacks);

            return availableAttacks.Dequeue();
        }
    }
}
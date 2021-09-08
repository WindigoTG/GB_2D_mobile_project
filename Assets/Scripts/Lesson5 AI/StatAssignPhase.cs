using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace AI
{
    public class StatAssignPhase : GamePhase, ISubscriber
    {
        [SerializeField] TMP_Text _pointsLeftText;
        [SerializeField] Button _addMeleeButton;
        [SerializeField] Button _subtractMeleeButton;
        [SerializeField] Button _addRangedButton;
        [SerializeField] Button _subtractRangedButton;
        [SerializeField] Button _addAccuracyButton;
        [SerializeField] Button _subtractAccuracyButton;
        [SerializeField] Button _addEvasionButton;
        [SerializeField] Button _subtractEvasionButton;
        [SerializeField] Button _addDefenceButton;
        [SerializeField] Button _subtractDefenceButton;
        [SerializeField] Button _addSkillButton;
        [SerializeField] Button _subtractSkillButton;
        [SerializeField] Button _startButton;

        int _pointsLeft;

        public override void BeginPhase()
        {
            gameObject.SetActive(true);
        }

        public override void EndPhase()
        {
            gameObject.SetActive(false);
            _combatController.StartGame();
        }

        public override void Init(Combatant player, Combatant enemy, MainCombatController combatController)
        {
            _combatController = combatController;
            _player = player;
            _enemy = enemy;

            _player.Pilot.SubscribeToAbilityPoints(this);

            _addMeleeButton.onClick.AddListener(() => _player.ChangePilotAbilityValue(true, StatType.Melee));
            _subtractMeleeButton.onClick.AddListener(() => _player.ChangePilotAbilityValue(false, StatType.Melee));

            _addRangedButton.onClick.AddListener(() => _player.ChangePilotAbilityValue(true, StatType.Ranged));
            _subtractRangedButton.onClick.AddListener(() => _player.ChangePilotAbilityValue(false, StatType.Ranged));

            _addAccuracyButton.onClick.AddListener(() => _player.ChangePilotAbilityValue(true, StatType.Accuracy));
            _subtractAccuracyButton.onClick.AddListener(() => _player.ChangePilotAbilityValue(false, StatType.Accuracy));

            _addEvasionButton.onClick.AddListener(() => _player.ChangePilotAbilityValue(true, StatType.Evasion));
            _subtractEvasionButton.onClick.AddListener(() => _player.ChangePilotAbilityValue(false, StatType.Evasion));

            _addDefenceButton.onClick.AddListener(() => _player.ChangePilotAbilityValue(true, StatType.Defence));
            _subtractDefenceButton.onClick.AddListener(() => _player.ChangePilotAbilityValue(false, StatType.Defence));

            _addSkillButton.onClick.AddListener(() => _player.ChangePilotAbilityValue(true, StatType.Skill));
            _subtractSkillButton.onClick.AddListener(() => _player.ChangePilotAbilityValue(false, StatType.Skill));

            _startButton.onClick.AddListener(StartGame);

            BeginPhase();
        }

        public void OnStatUpdate(ObservableStat stat, StatType dataType)
        {
            bool isAssigningPoint = stat.StatValue < _pointsLeft;
            _pointsLeft = stat.StatValue;
            _pointsLeftText.text = $"Points left: {_pointsLeft}";

            if (_combatController.IsSetupDone)
                _enemy.ChangePilotAbilityValue(isAssigningPoint, StatType.NonSpecified);
        }

        private void StartGame()
        {
            if (_pointsLeft > 0)
                Debug.Log("<color=#FF0000>Assign all ability points to begin</color>");
            else

                EndPhase();
        }

        private void OnDestroy()
        {
            _addMeleeButton.onClick.RemoveAllListeners();
            _subtractMeleeButton.onClick.RemoveAllListeners();

            _addRangedButton.onClick.RemoveAllListeners();
            _subtractRangedButton.onClick.RemoveAllListeners();

            _addAccuracyButton.onClick.RemoveAllListeners();
            _subtractAccuracyButton.onClick.RemoveAllListeners();

            _addEvasionButton.onClick.RemoveAllListeners();
            _subtractEvasionButton.onClick.RemoveAllListeners();

            _addDefenceButton.onClick.RemoveAllListeners();
            _subtractDefenceButton.onClick.RemoveAllListeners();

            _addSkillButton.onClick.RemoveAllListeners();
            _subtractSkillButton.onClick.RemoveAllListeners();

            _startButton.onClick.RemoveAllListeners();
        }
    }
}
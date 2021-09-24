using UnityEngine;
using UnityEngine.UI;

namespace AI
{
    public class FightView : MonoBehaviour
    {
        [SerializeField]
        private StatsUIHandler _playerStatsDisplay;
        [SerializeField]
        private StatsUIHandler _enemyStatsDisplay;

        [SerializeField]
        private GamePhase _statAssignPhaseWindow;
        [SerializeField]
        private GamePhase _playerPhaseWindow;
        [SerializeField]
        private GamePhase _enemyPhaseWindow;

        [SerializeField]
        private Image _playerViewImage;
        [SerializeField]
        private Image _enemyViewImage;

        [SerializeField]
        private Button _buttonLeaveFight;

        [SerializeField]
        private Button _buttonSwitchLocale;

        public StatsUIHandler PlayerStatsDisplay => _playerStatsDisplay;
        public StatsUIHandler EnemyStatsDisplay => _enemyStatsDisplay;
        public GamePhase StatAssignPhaseWindow => _statAssignPhaseWindow;
        public GamePhase PlayerPhaseWindow => _playerPhaseWindow;
        public GamePhase EnemyPhaseWindow => _enemyPhaseWindow;
        public Image PlayerViewImage => _playerViewImage;
        public Image EnemyViewImage => _enemyViewImage;
        public Button ButtonLeaveFight => _buttonLeaveFight;
        public Button ButtonSwitchLocale => _buttonSwitchLocale; 
    }
}
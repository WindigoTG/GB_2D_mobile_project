using UnityEngine;
using UnityEngine.UI;

namespace Rewards
{
    public class InstallView : MonoBehaviour
    {
        [SerializeField]
        private RewardView _dailyRewardView;
        [SerializeField]
        private RewardView _weeklyRewardView;
        [SerializeField]
        private Button _resetButton;

        private RewardController _rewardController;

        private void Awake()
        {
            //_rewardController = new RewardController(_dailyRewardView, _weeklyRewardView, _resetButton);
        }

        private void Start()
        {
            _rewardController.RefreshView();
        }
    }
}
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Rewards
{
    public class ContainerSlotRewardView : MonoBehaviour
    {
        [SerializeField]
        private Image _selectedBackground;

        [SerializeField]
        private Image _rewardIcon;

        [SerializeField]
        private TMP_Text _rewardDayText;

        [SerializeField]
        private TMP_Text _rewardedAmmountText;

        public void SetData(Reward reward, int rewardDay, bool isSelected)
        {
            _rewardIcon.sprite = reward.RewardIcon;
            _rewardDayText.text = $"Day {rewardDay}";
            _rewardedAmmountText.text = reward.Ammount.ToString();
            _selectedBackground.gameObject.SetActive(isSelected);
        }
    }
}
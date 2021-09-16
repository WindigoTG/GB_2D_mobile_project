using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Rewards
{
    public class RewardView : MonoBehaviour
    {
        private string CurrentActiveSlotKey;
        private string LastRewardTimeKey;

        private const string CurrentActiveSlotKeyPrefix = nameof(CurrentActiveSlotKey);
        private const string LastRewardTimeKeyPrefix = nameof(LastRewardTimeKey);

        [SerializeField] private RewardPeriod _rewardPeriod;

        [Header("Reward timers")]
        [SerializeField]
        private float _rewardCooldownTime = 86400;

        [SerializeField]
        private float _rewardChainResetTime = 172800;

        [Header("Rewards")]
        [SerializeField]
        private List<Reward> _rewards;

        [Header("UI Elements")]
        [SerializeField]
        private TMP_Text _nextRewardTimeText;

        [SerializeField]
        private Transform _slotLayout;

        [SerializeField]
        private ContainerSlotRewardView _containerSlotRewardView;

        [SerializeField]
        private Button _claimRewardButton;

        [SerializeField]
        private Image _progressBar;

        public float RewardCooldownTime => _rewardCooldownTime;

        public float RewardChainResetTime => _rewardChainResetTime;

        public List<Reward> Rewards => _rewards;

        public TMP_Text NextRewardTimeText => _nextRewardTimeText;

        public Transform SlotLayout => _slotLayout;

        public ContainerSlotRewardView ContainerSlotRewardView => _containerSlotRewardView;

        public Button ClaimRewardButton => _claimRewardButton;

        public Image ProgressBar => _progressBar;

        public int CurrentDailyActiveSlot
        {
            get => PlayerPrefs.GetInt(CurrentActiveSlotKey, 0);
            set => PlayerPrefs.SetInt(CurrentActiveSlotKey, value);
        }

        public DateTime? LastRewardClaimTime
        {
            get
            {
                var data = PlayerPrefs.GetString(LastRewardTimeKey, null);

                if (!string.IsNullOrEmpty(data))
                    return DateTime.Parse(data);

                return null;
            }
            set
            {
                if (value != null)
                    PlayerPrefs.SetString(LastRewardTimeKey, value.ToString());
                else
                    PlayerPrefs.DeleteKey(LastRewardTimeKey);
            }
        }

        private void Awake()
        {
            CurrentActiveSlotKey = CurrentActiveSlotKeyPrefix + _rewardPeriod.ToString();
            LastRewardTimeKey = LastRewardTimeKeyPrefix + _rewardPeriod.ToString();
        }

        private void OnDestroy()
        {
            _claimRewardButton.onClick.RemoveAllListeners();
        }
    }
}
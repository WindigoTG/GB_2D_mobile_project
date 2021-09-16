using Profile;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Rewards
{
    public class RewardController : BaseController
    {
        private RewardView _dailyRewardView;
        private RewardView _weeklyRewardView;
        private CurrencyView _currencyView;
        private PlayerProfile _playerProfile;
        private List<ContainerSlotRewardView> _slotsDaily;
        private List<ContainerSlotRewardView> _slotsWeekly;

        private bool _isDailyRewardReadyToClaim;
        private bool _isWeeklyRewardReadyToClaim;

        private float _progressDaily;
        private float _progressWeekly;

        public RewardController(Transform placeForUi, PlayerProfile playerProfile)
        {
            _weeklyRewardView = ResourceLoader.LoadAndInstantiateObject<RewardView>(References.WEEKLY_REWARD_VIEW_PREFAB_PATH, placeForUi, false);
            AddGameObject(_weeklyRewardView.gameObject);
            _dailyRewardView = ResourceLoader.LoadAndInstantiateObject<RewardView>(References.DAILY_REWARD_VIEW_PREFAB_PATH, placeForUi, false);
            AddGameObject(_dailyRewardView.gameObject);
            _currencyView = ResourceLoader.LoadAndInstantiateObject<CurrencyView>(References.CURRENCY_VIEW_PREFAB_PATH, placeForUi, false);
            AddGameObject(_currencyView.gameObject);

            _playerProfile = playerProfile;

            RefreshView();
        }

        public void RefreshView()
        {
            InitSlots();

            _dailyRewardView.StartCoroutine(RewardsStateUpdater());

            RefreshUi();
            SubscribeToButtons();
        }

        private void InitSlots()
        {
            _slotsDaily = new List<ContainerSlotRewardView>();
            _slotsWeekly = new List<ContainerSlotRewardView>();

            FillSlots(_dailyRewardView, ref _slotsDaily);
            FillSlots(_weeklyRewardView, ref _slotsWeekly);
        }

        private void FillSlots(RewardView rewardView, ref List<ContainerSlotRewardView> slots)
        {
            for (var i = 0; i < rewardView.Rewards.Count; i++)
            {
                var instanceSlot = GameObject.Instantiate(rewardView.ContainerSlotRewardView,
                    rewardView.SlotLayout, false);

                slots.Add(instanceSlot);
            }
        }

        private IEnumerator RewardsStateUpdater()
        {
            while (true)
            {
                RefreshRewardsState();
                yield return new WaitForSeconds(1);
            }
        }

        private void RefreshRewardsState()
        {
            CheckIfRewardisReadyAndCalculateProgress(_dailyRewardView, out _isDailyRewardReadyToClaim, out _progressDaily);
            CheckIfRewardisReadyAndCalculateProgress(_weeklyRewardView, out _isWeeklyRewardReadyToClaim, out _progressWeekly);

            RefreshUi();
        }

        private void CheckIfRewardisReadyAndCalculateProgress(RewardView rewardView, out bool isRewardReady, out float rewardProgress)
        {
            isRewardReady = true;
            rewardProgress = 1;

            if (rewardView.LastRewardClaimTime.HasValue)
            {
                var timeSinceLastClaim = DateTime.UtcNow - rewardView.LastRewardClaimTime.Value;

                if (timeSinceLastClaim.Seconds > rewardView.RewardChainResetTime)
                {
                    rewardView.LastRewardClaimTime = null;
                    rewardView.CurrentDailyActiveSlot = 0;
                }
                else if (timeSinceLastClaim.Seconds < rewardView.RewardCooldownTime)
                {
                    isRewardReady = false;
                }

                var progress = timeSinceLastClaim.Seconds / rewardView.RewardCooldownTime;

                rewardProgress = progress <= 1 ? progress : 1;
            }
        }

        private void RefreshUi()
        {
            RefreshUIPortion(RewardPeriod.Daily);
            RefreshUIPortion(RewardPeriod.Weekly);
        }

        private void RefreshUIPortion(RewardPeriod rewardPeriod)
        {
            RewardView rewardView;
            bool isRewardReady;
            List<ContainerSlotRewardView> slots;
            float progress;

            switch (rewardPeriod)
            {
                case RewardPeriod.Weekly:
                    {
                        rewardView = _weeklyRewardView;
                        isRewardReady = _isWeeklyRewardReadyToClaim;
                        slots = _slotsWeekly;
                        progress = _progressWeekly;
                        break;
                    }
                default:
                    {
                        rewardView = _dailyRewardView;
                        isRewardReady = _isDailyRewardReadyToClaim;
                        slots = _slotsDaily;
                        progress = _progressDaily;
                        break;
                    }
            }


            rewardView.ClaimRewardButton.interactable = isRewardReady;

            rewardView.ProgressBar.transform.localScale = rewardView.ProgressBar.transform.localScale.Change(x: progress);

            if (isRewardReady)
            {
                rewardView.NextRewardTimeText.text = $"Claim {rewardPeriod.ToString()} reward now!";
            }
            else
            {
                if (rewardView.LastRewardClaimTime != null)

                    rewardView.NextRewardTimeText.text = $"Next reward in: {CalculateRemainingTime(rewardView)}";
            }

            for (var i = 0; i < slots.Count; i++)
                slots[i].SetData(rewardView.Rewards[i], i + 1, i == rewardView.CurrentDailyActiveSlot);
        }

        private string CalculateRemainingTime(RewardView rewardView)
        {
            var nextClaimTime = rewardView.LastRewardClaimTime.Value.AddSeconds(rewardView.RewardCooldownTime);
            var currentClaimCooldown = nextClaimTime - DateTime.UtcNow;
            return $"{currentClaimCooldown.Days:D2}:{currentClaimCooldown.Hours:D2}:{currentClaimCooldown.Minutes:D2}:{currentClaimCooldown.Seconds:D2}";
        }

        private void SubscribeToButtons()
        {
            _dailyRewardView.ClaimRewardButton.onClick.AddListener(() => ClaimReward(RewardPeriod.Daily));
            _weeklyRewardView.ClaimRewardButton.onClick.AddListener(() => ClaimReward(RewardPeriod.Weekly));
            _currencyView.ResetButton.onClick.AddListener(ResetTimer);
            _currencyView.BackButton.onClick.AddListener(CloseWindow);
        }

        private void ClaimReward(RewardPeriod rewardPeriod)
        {
            if ((rewardPeriod == RewardPeriod.Daily && !_isDailyRewardReadyToClaim) ||
                (rewardPeriod == RewardPeriod.Weekly && !_isWeeklyRewardReadyToClaim))
                return;

            RewardView rewardView;

            switch (rewardPeriod)
            {
                case RewardPeriod.Weekly:
                    rewardView = _weeklyRewardView;
                    break;
                default:
                    rewardView = _dailyRewardView;
                    break;
            }

            GetRewardFromView(rewardView);

            RefreshRewardsState();
        }

        private void GetRewardFromView(RewardView rewardView)
        {
            var reward = rewardView.Rewards[rewardView.CurrentDailyActiveSlot];

            switch (reward.RewardType)
            {
                case RewardType.Gold:
                    CurrencyView.Instance.AddGold(reward.Ammount);
                    break;
                case RewardType.Crystal:
                    CurrencyView.Instance.AddCrystal(reward.Ammount);
                    break;
            }

            rewardView.LastRewardClaimTime = DateTime.UtcNow;
            rewardView.CurrentDailyActiveSlot = (rewardView.CurrentDailyActiveSlot + 1) % rewardView.Rewards.Count;
        }

        private void ResetTimer()
        {
            PlayerPrefs.DeleteAll();

            //Для обновления интерфейса
            CurrencyView.Instance.AddGold(0);
            CurrencyView.Instance.AddCrystal(0);
        }

        private void CloseWindow()
        {
            _playerProfile.CurrentState.Value = GameState.Menu;
        }


        protected override void OnDispose()
        {
            base.OnDispose();
            _currencyView.BackButton.onClick.RemoveAllListeners();
            _currencyView.ResetButton.onClick.RemoveAllListeners();
        }
    }
}
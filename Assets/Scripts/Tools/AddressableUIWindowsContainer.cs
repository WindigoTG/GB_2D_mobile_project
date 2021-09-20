using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

[Serializable]
public class AddressableUIWindowsContainer
{
    [SerializeField] private Transform _placeForUi;
    [SerializeField] private AssetReference _mainMenuWindowPrefab;
    [SerializeField] private AssetReference _startFightWindowPrefab;
    [SerializeField] private AssetReference _fightWindowPrefab;
    [SerializeField] private AssetReference _dailyRewardsWindowPrefab;
    [SerializeField] private AssetReference _weeklyRewardsWindowPrefab;
    [SerializeField] private AssetReference _currencyWindowPrefab;

    public Transform PlaceForUi => _placeForUi;
    public AssetReference MainMenuWindowPrefab => _mainMenuWindowPrefab;
    public AssetReference StartFightWindowPrefab => _startFightWindowPrefab;
    public AssetReference FightWindowPrefab => _fightWindowPrefab;
    public AssetReference DailyRewardsWindowPrefab => _dailyRewardsWindowPrefab; 
    public AssetReference WeeklyRewardsWindowPrefab => _weeklyRewardsWindowPrefab;
    public AssetReference CurrencyWindowPrefab => _currencyWindowPrefab;
}

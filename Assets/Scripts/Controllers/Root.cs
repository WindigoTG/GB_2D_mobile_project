using Profile;
using Profile.Analytics;
using Tools;
using UnityEngine;

public class Root : MonoBehaviour
{
    [SerializeField] private Transform _placeForUi;
    [SerializeField] private UnityAdsTools _ads;
    [SerializeField] private UpgradeItemConfigDataSource _upgradeItemDataSource;

    private MainController _mainController;

    private void Awake()
    {
        var playerProfile = new PlayerProfile(15f, new UnityAnalyticsTools(), _ads);
        playerProfile.CurrentState.Value = GameState.Menu;
        _mainController = new MainController(_placeForUi, playerProfile, _upgradeItemDataSource);
    }

    protected void OnDestroy()
    {
        _mainController?.Dispose();
    }
}

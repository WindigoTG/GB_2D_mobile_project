using Profile;
using Rewards;
using UnityEngine;

public class MainController : BaseController
{

    private MainMenuController _mainMenuController;
    private GameController _gameController;
    private GarageController _garageController;
    private RewardController _rewardController;
    private StartFightController _startFightController;
    private FightController _fightController;
    private readonly AddressableUIWindowsContainer _uiPrefabsContainer;
    private readonly PlayerProfile _playerProfile;
    private UpgradeItemConfigDataSource _upgradeItemDataSource;

    public MainController(AddressableUIWindowsContainer uiPrefabsContainer, PlayerProfile playerProfile, UpgradeItemConfigDataSource upgradeItemDataSource)
    {
        _playerProfile = playerProfile;
        _uiPrefabsContainer = uiPrefabsContainer;
        OnChangeGameState(_playerProfile.CurrentState.Value);
        playerProfile.CurrentState.SubscribeOnChange(OnChangeGameState);
        _upgradeItemDataSource = upgradeItemDataSource;
    }

     private void OnChangeGameState(GameState state)
    {
        switch (state)
        {
            case GameState.Menu:
                _mainMenuController = new MainMenuController(_uiPrefabsContainer, _playerProfile);
                _gameController?.Dispose();
                _rewardController?.Dispose();
                break;
            case GameState.Game:
                _gameController = new GameController(_playerProfile, _uiPrefabsContainer.PlaceForUi);
                _startFightController = new StartFightController(_uiPrefabsContainer, _playerProfile);
                _mainMenuController?.Dispose();
                _rewardController?.Dispose();
                _fightController?.Dispose();
                break;
            case GameState.Rewards:
                _rewardController = new RewardController(_uiPrefabsContainer, _playerProfile);
                _mainMenuController?.Dispose();
                _gameController?.Dispose();
                break;
            case GameState.Fight:
                _fightController = new FightController(_uiPrefabsContainer, _playerProfile);
                _startFightController?.Dispose();
                _mainMenuController?.Dispose();
                _gameController?.Dispose();
                break;
            default:
                _mainMenuController?.Dispose();
                _gameController?.Dispose();
                _garageController?.Dispose();
                _startFightController?.Dispose();
                _fightController?.Dispose();
                break;
        }
    }

    protected override void OnDispose()
    {
        _mainMenuController?.Dispose();
        _gameController?.Dispose();
        _garageController?.Dispose();
        _startFightController?.Dispose();
        _fightController?.Dispose();
        _playerProfile.CurrentState.UnSubscriptionOnChange(OnChangeGameState);
        base.OnDispose();
    }
}

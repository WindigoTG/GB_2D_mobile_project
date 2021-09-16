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
    private readonly Transform _placeForUi;
    private readonly PlayerProfile _playerProfile;
    private UpgradeItemConfigDataSource _upgradeItemDataSource;

    public MainController(Transform placeForUi, PlayerProfile playerProfile, UpgradeItemConfigDataSource upgradeItemDataSource)
    {
        _playerProfile = playerProfile;
        _placeForUi = placeForUi;
        OnChangeGameState(_playerProfile.CurrentState.Value);
        playerProfile.CurrentState.SubscribeOnChange(OnChangeGameState);
        _upgradeItemDataSource = upgradeItemDataSource;
    }

     private void OnChangeGameState(GameState state)
    {
        switch (state)
        {
            case GameState.Menu:
                _mainMenuController = new MainMenuController(_placeForUi, _playerProfile);
                _gameController?.Dispose();
                _rewardController?.Dispose();
                break;
            case GameState.Game:
                _gameController = new GameController(_playerProfile, _placeForUi);
                _startFightController = new StartFightController(_placeForUi, _playerProfile);
                _mainMenuController?.Dispose();
                _rewardController?.Dispose();
                _fightController?.Dispose();
                break;
            case GameState.Rewards:
                _rewardController = new RewardController(_placeForUi, _playerProfile);
                _mainMenuController?.Dispose();
                _gameController?.Dispose();
                break;
            case GameState.Fight:
                _fightController = new FightController(_placeForUi, _playerProfile);
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
        _playerProfile.CurrentState.UnSubscriptionOnChange(OnChangeGameState);
        base.OnDispose();
    }
}

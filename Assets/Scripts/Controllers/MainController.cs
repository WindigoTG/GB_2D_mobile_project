using Profile;
using UnityEngine;

public class MainController : BaseController
{

    private MainMenuController _mainMenuController;
    private GameController _gameController;
    private GarageController _garageController;
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
            case GameState.Start:
                _mainMenuController = new MainMenuController(_placeForUi, _playerProfile);
                _gameController?.Dispose();
                break;
            case GameState.Game:
                _gameController = new GameController(_playerProfile, _placeForUi);
                _mainMenuController?.Dispose();
                _garageController?.Dispose();
                break;
            default:
                _mainMenuController?.Dispose();
                _gameController?.Dispose();
                _garageController?.Dispose();
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

using Profile;
using UnityEngine;

public class MainController : BaseController
{

    private MainMenuController _mainMenuController;
    private GameController _gameController;
    private readonly Transform _placeForUi;
    private readonly PlayerProfile _playerProfile;

    public MainController(Transform placeForUi, PlayerProfile playerProfile)
    {
        _playerProfile = playerProfile;
        _placeForUi = placeForUi;
        OnChangeGameState(_playerProfile.CurrentState.Value);
        playerProfile.CurrentState.SubscribeOnChange(OnChangeGameState);
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
                _gameController = new GameController(_playerProfile);
                _mainMenuController?.Dispose();
                break;
            default:
                _mainMenuController?.Dispose();
                _gameController?.Dispose();
                break;
        }
    }

    protected override void OnDispose()
    {
        _mainMenuController?.Dispose();
        _gameController?.Dispose();
        _playerProfile.CurrentState.UnSubscriptionOnChange(OnChangeGameState);
        base.OnDispose();
    }
}

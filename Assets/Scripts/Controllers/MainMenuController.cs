using Profile;
using UnityEngine;

public class MainMenuController : BaseController
{
    private readonly PlayerProfile _playerProfile;
    private readonly MainMenuView _view;

    public MainMenuController(Transform placeForUi, PlayerProfile playerProfile)
    {
        _playerProfile = playerProfile;
        _view = LoadView(placeForUi);
        _view.Init(StartGame, playerProfile.SetInputMethod);
    }
    
    private MainMenuView LoadView(Transform placeForUi)
    {
        var objectView = Object.Instantiate(ResourceLoader.LoadPrefab(References.MAIN_MENU_PREFAB_PATH), placeForUi, false);
        AddGameObject(objectView);
        
        return objectView.GetComponent<MainMenuView>();
    }

    private void StartGame()
    {
        _playerProfile.CurrentState.Value = GameState.Game;
    }
}


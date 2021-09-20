using Profile;
using UnityEngine;

public class StartFightController : BaseController
{
    private StartFightView _startFightView;
    private PlayerProfile _profilePlayer;

    public StartFightController(AddressableUIWindowsContainer uiPrefabsContainer, PlayerProfile profilePlayer)
    {
        _profilePlayer = profilePlayer;

        CreateAddressablesPrefab<StartFightView>(uiPrefabsContainer.StartFightWindowPrefab, uiPrefabsContainer.PlaceForUi, InitializeView);
    }

    private void InitializeView(StartFightView view)
    {
        _startFightView = view;
        _startFightView.StartFightButton.onClick.AddListener(StartFight);
    }

    private void StartFight()
    {
        _profilePlayer.CurrentState.Value = GameState.Fight;
    }

    protected override void OnDispose()
    {
        _startFightView.StartFightButton.onClick.RemoveAllListeners();

        base.OnDispose();
    }
}

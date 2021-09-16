using Profile;
using UnityEngine;

public class StartFightController : BaseController
{
    private StartFightView _startFightViewInstance;
    private PlayerProfile _profilePlayer;

    public StartFightController(Transform placeForUi, PlayerProfile profilePlayer)
    {
        _profilePlayer = profilePlayer;

        _startFightViewInstance = ResourceLoader.LoadAndInstantiateObject<StartFightView>(References.START_FIGHT_VIEW_PREFAB_PATH, placeForUi, false);
        AddGameObject(_startFightViewInstance.gameObject);

        _startFightViewInstance.StartFightButton.onClick.AddListener(StartFight);
    }

    private void StartFight()
    {
        _profilePlayer.CurrentState.Value = GameState.Fight;
    }

    protected override void OnDispose()
    {
        _startFightViewInstance.StartFightButton.onClick.RemoveAllListeners();

        base.OnDispose();
    }
}

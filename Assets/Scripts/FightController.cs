using UnityEngine;
using AI;
using Profile;

public class FightController : BaseController
{
    private FightView _fightView;
    private PlayerProfile _playerProfile;

    public FightController (Transform placeForUi, PlayerProfile playerProfile)
    {
        _fightView = ResourceLoader.LoadAndInstantiateObject<FightView>(References.FIGHT_VIEW_PREFAB_PATH, placeForUi, false);
        AddGameObject(_fightView.gameObject);

        _playerProfile = playerProfile;
        _fightView.ButtonLeaveFight.onClick.AddListener(LeaveFight);
    }

    private void LeaveFight()
    {
        _playerProfile.CurrentState.Value = GameState.Game;
    }

    protected override void OnDispose()
    {
        base.OnDispose();
        _fightView.ButtonLeaveFight.onClick.RemoveAllListeners();
    }
}

using Profile;
using UnityEngine;

public class Root : MonoBehaviour
{
    [SerializeField] 
    private Transform _placeForUi;

    private MainController _mainController;

    private void Awake()
    {
        var playerProfile = new PlayerProfile(15f);
        playerProfile.CurrentState.Value = GameState.Start;
        _mainController = new MainController(_placeForUi, playerProfile);
    }

    protected void OnDestroy()
    {
        _mainController?.Dispose();
    }
}

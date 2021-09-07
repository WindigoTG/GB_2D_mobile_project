using Profile;
using UnityEngine;
using UnityEngine.Advertisements;
using System.Collections.Generic;
using System.Linq;

public class MainMenuController : BaseController
{
    private readonly PlayerProfile _playerProfile;
    private readonly MainMenuView _view;

    public MainMenuController(Transform placeForUi, PlayerProfile playerProfile)
    {
        _playerProfile = playerProfile;
        _view = ResourceLoader.LoadAndInstantiateObject<MainMenuView>(References.MAIN_MENU_PREFAB_PATH, placeForUi, false);
        AddGameObject(_view.gameObject);

        _view.Init(StartGame, playerProfile.SetInputMethod);
    }

    private BaseController ConfigureGarageController(
           Transform placeForUi,
           PlayerProfile playerProfile)
    {
        var upgradeItemsConfigCollection = ContentDataSourceLoader.LoadUpgradeItemConfigs(References.UPGRADE_ITEM_CONFIG_DATA_SOURCE_PATH);
        var upgradeItemsRepository = new UpgradeHandlersRepository(upgradeItemsConfigCollection);

        var itemsRepository = new ItemsRepository(upgradeItemsConfigCollection.Select(value => value.ItemConfig).ToList());
        var inventoryModel = new InventoryModel();

        var inventoryView = ResourceLoader.LoadAndInstantiateObject<InventoryView>(References.INVENTORY_PREFAB_PATH, placeForUi, false);
        AddGameObject(inventoryView.gameObject);

        var inventoryController = new InventoryController(inventoryModel, itemsRepository, inventoryView);
        AddController(inventoryController);

        var garageController = new GarageController(upgradeItemsRepository, inventoryController, playerProfile.CurrentCar);
        AddController(garageController);

        return garageController;
    }


    private void StartGame()
    {
        _playerProfile.CurrentState.Value = GameState.Game;

        Dictionary<string, object> eventData = new Dictionary<string, object>()
        {
            { "TimesinceStartup", Time.realtimeSinceStartup},
            { "ControlMethod", _view.SelectedControlMethod }
        };
        _playerProfile.AnalyticTools.SendMessage("start_game", eventData);

        _playerProfile.AdsDisplay.ShowInterstitial();
                Advertisement.AddListener(_playerProfile.AdsListener);
    }
}


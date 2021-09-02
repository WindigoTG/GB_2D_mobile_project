using Profile;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GarageController : BaseController, IGarageController
{
    private readonly Car _car;

    private readonly UpgradeHandlersRepository _upgradeHandlersRepository;
    private readonly ItemsRepository _upgradeItemsRepository;
    private readonly InventoryModel _inventoryModel;
    private readonly InventoryController _inventoryController;

    private PlayerProfile _playerProfile;

    public GarageController(List<UpgradeItemConfig> upgradeItemConfigs, PlayerProfile playerProfile, Transform placeForUi)
    {
        _playerProfile = playerProfile;
        _car = _playerProfile.CurrentCar;

        _upgradeHandlersRepository = new UpgradeHandlersRepository(upgradeItemConfigs);
        AddController(_upgradeHandlersRepository);

        _upgradeItemsRepository = new ItemsRepository(upgradeItemConfigs.Select(value => value.ItemConfig).ToList());
        AddController(_upgradeItemsRepository);

        _inventoryModel = new InventoryModel();

        _inventoryController = new InventoryController(_inventoryModel, _upgradeItemsRepository, placeForUi);
        AddController(_inventoryController);
    }

    public void Enter()
    {
        _inventoryController.ShowInventory(Exit);
        Debug.Log($"Enter: car has speed : {_car.Speed}");
    }

    public void Exit()
    {
        UpgradeCarWithEquippedItems(_car, _inventoryModel.GetEquippedItems(), _upgradeHandlersRepository.UpgradeItems);
        Debug.Log($"Exit: car has speed : {_car.Speed}");
        _playerProfile.CurrentState.Value = GameState.Game;
    }

    private void UpgradeCarWithEquippedItems(
       IUpgradableCar upgradableCar,
       IReadOnlyList<IItem> equippedItems,
       IReadOnlyDictionary<int, IUpgradeCarHandler> upgradeHandlers)
    {
        foreach (var equippedItem in equippedItems)
        {
            if (upgradeHandlers.TryGetValue(equippedItem.Id, out var handler))
            {
                handler.Upgrade(upgradableCar);
            }
        }
    }
}


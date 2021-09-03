using System.Collections.Generic;

public class GarageController : BaseController, IGarageController
{
    private readonly IUpgradableCar _upgradable;

    private readonly IRepository<int, IUpgradeCarHandler> _upgradeHandlersRepository;
    private readonly IInventoryController _inventoryController;

    public GarageController(IRepository<int, IUpgradeCarHandler> upgradeHandlersRepository,
           IInventoryController inventoryController,
           IUpgradableCar upgradable)
    {
        _upgradeHandlersRepository = upgradeHandlersRepository;

        _inventoryController  =  inventoryController;
          
        _upgradable = upgradable;
    }

    public void Enter()
    {
        _inventoryController.ShowInventory(Exit);
    }

    public void Exit()
    {
        UpgradeCarWithEquippedItems(
            _upgradable, _inventoryController.GetEquippedItems(), _upgradeHandlersRepository.Collection);
    }


    private void UpgradeCarWithEquippedItems(
           IUpgradableCar upgradable,
           IReadOnlyList<IItem> equippedItems,
           IReadOnlyDictionary<int, IUpgradeCarHandler> upgradeHandlers)
    {
        foreach (var equippedItem in equippedItems)
        {
            if (upgradeHandlers.TryGetValue(equippedItem.Id, out var handler))
            {
                handler.Upgrade(upgradable);
            }
        }
    }

}


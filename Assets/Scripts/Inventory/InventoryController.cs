using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class InventoryController : BaseController, IInventoryController
{
    private readonly IInventoryModel _inventoryModel;
    private readonly IItemsRepository _itemsRepository;
    private readonly IInventoryView _inventoryWindowView;

    Action _exit;

    public InventoryController(IInventoryModel inventoryModel, IItemsRepository itemsRepository, Transform placeForUi)
    {
        _inventoryModel = inventoryModel;
        _itemsRepository = itemsRepository;
        _inventoryWindowView = LoadView(placeForUi);
    }

    private InventoryView LoadView(Transform placeForUi)
    {
        var inventoryViewObject = Object.Instantiate(ResourceLoader.LoadPrefab(References.INVENTORY_PREFAB_PATH), placeForUi, false);
        AddGameObject(inventoryViewObject);

        return inventoryViewObject.GetComponent<InventoryView>();
    }

    public void HideInventory()
    {
        throw new NotImplementedException();
    }

    public void ShowInventory(Action callback)
    {
        List<IItem> items = new List<IItem>();
        foreach (var item in _itemsRepository.Items.Values)
            items.Add(item);

        _inventoryWindowView.Display(items);
        _inventoryWindowView.Selected += EquipSeletedItem;

        _exit = callback;
   }

    private void EquipSeletedItem(IItem item)
    {
        _inventoryModel.EquipItem(item);
        _exit.Invoke();
    }
}


using System;
using System.Collections.Generic;
using System.Linq;

public class InventoryController : BaseController, IInventoryController
{
    private readonly IInventoryModel _inventoryModel;
    private readonly IRepository<int, IItem> _itemsRepository;
    private readonly IInventoryView _inventoryView;
    private Action _exitAction;

    public InventoryController(IInventoryModel inventoryModel, IRepository<int, IItem> itemsRepository, IInventoryView inventoryView)
    {
        _inventoryModel = inventoryModel;
        _itemsRepository = itemsRepository;
        _inventoryView = inventoryView;
        SetupView(_inventoryView);
    }

    private void SetupView(IInventoryView inventoryView)
    {
        inventoryView.Selected += OnItemSelected;
        inventoryView.Deselected += OnItemDeselected;
    }

    private void CleanupView()
    {
        _inventoryView.Selected -= OnItemSelected;
        _inventoryView.Deselected -= OnItemDeselected;
    }

    private void OnItemSelected(object sender, IItem item)
    {
        _inventoryModel.EquipItem(item);
    }

    private void OnItemDeselected(object sender, IItem item)
    {
        _inventoryModel.UnequipItem(item);
    }

    public void ShowInventory(Action callback)
    {
        _inventoryView.Show();
        _inventoryView.Display(_itemsRepository.Collection.Values.ToList());

        _exitAction = callback;
   }

    public void HideInventory()
    {
        _inventoryView.Hide();
        _exitAction?.Invoke();
    }

    public IReadOnlyList<IItem> GetEquippedItems()
    {
        return _inventoryModel.GetEquippedItems();
    }

    protected override void OnDispose()
    {
        CleanupView();
        base.OnDispose();
    }

}


using System.Collections.Generic;

public class InventoryModel : IInventoryModel
{
    private static readonly List<IItem> _emptyCollection = new List<IItem>();
    private readonly List<IItem> _equippedItems = new List<IItem>();

    public IReadOnlyList<IItem> GetEquippedItems()
    {
        return _equippedItems ?? _emptyCollection;
    }

    public void EquipItem(IItem item)
    {
        if (_equippedItems.Contains(item))
            return;

        _equippedItems.Add(item);
    }

    public void UnequipItem(IItem item)
    {
        if (!_equippedItems.Contains(item))
            return;

        _equippedItems.Remove(item);
    }
}


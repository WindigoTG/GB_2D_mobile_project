using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryView : MonoBehaviour, IInventoryView
{
    public event Action<IItem> Selected;
    public event Action<IItem> Deselected;

    private IReadOnlyList<IItem> _itemInfoCollection;

    public void Display(IReadOnlyList<IItem> itemInfoCollection)
    {
        _itemInfoCollection = itemInfoCollection;
        CreateItemsToDisplay();
    }

    protected virtual void OnSelected(IItem item)
    {
        Selected?.Invoke(item);
    }

    protected virtual void OnDeselected(IItem item)
    {
        Deselected?.Invoke(item);
    }

    private void CreateItemsToDisplay()
    {
        int count = 0;

        foreach (var item in _itemInfoCollection)
        {
            var itemViewObject = new GameObject($"Item_{count++}", typeof(RectTransform));
            itemViewObject.transform.SetParent(transform);
            itemViewObject.transform.localScale = Vector3.one;
            var itemView = itemViewObject.AddComponent<ItemView>();
            itemView.Init(item);
            itemView.Selected += OnSelected;
        }
    }
}


using System;
using System.Collections.Generic;

public interface IInventoryView
{
    event Action<IItem> Selected;
    event Action<IItem> Deselected;
    void Display(IReadOnlyList<IItem> items);
}


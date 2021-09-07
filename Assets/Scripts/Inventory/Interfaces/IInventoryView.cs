using System;
using System.Collections.Generic;

public interface IInventoryView : IView
{
    event EventHandler<IItem> Selected;
    event EventHandler<IItem> Deselected;
    void Display(List<IItem> items);
}


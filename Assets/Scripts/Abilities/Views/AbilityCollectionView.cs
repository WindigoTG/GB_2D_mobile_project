using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityCollectionView : MonoBehaviour, IAbilityCollectionView
{
    private IReadOnlyList<IItem> _abilityItems;
    public event EventHandler<IItem> UseRequested;

    protected virtual void OnUseRequested(IItem e)
    {
        UseRequested?.Invoke(this, e);
    }

    public void Display(IReadOnlyList<IItem> abilityItems)
    {
        _abilityItems = abilityItems;
    }

    public void Show()
    {
        
    }

    public void Hide()
    {
       
    }
}

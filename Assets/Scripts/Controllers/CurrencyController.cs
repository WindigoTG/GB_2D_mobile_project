using UnityEngine;
using Rewards;

public class CurrencyController : BaseController
{
    private CurrencyView _currencyView;

    public CurrencyController(Transform placeForUi, CurrencyView currencyView)
    {
        _currencyView = ResourceLoader.LoadAndInstantiateObject<CurrencyView>(References.MAIN_MENU_PREFAB_PATH, placeForUi, false);
        AddGameObject(_currencyView.gameObject);
    }
}


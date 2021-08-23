using Tools;
using UnityEngine;

public class CarController : BaseController
{
    private readonly CarView _carView;

    public CarController(SubscriptionProperty<float> leftmove, SubscriptionProperty<float> rightMove)
    {
        _carView = LoadView();
        _carView.Init(leftmove, rightMove);
    }

    private CarView LoadView()
    {
        var objView = Object.Instantiate(ResourceLoader.LoadPrefab(References.CAR_PREFAB_PATH));
        AddGameObject(objView);
        
        return objView.GetComponent<CarView>();
    }

    public GameObject GetViewObject()
    {
        return _carView.gameObject;
    }
}


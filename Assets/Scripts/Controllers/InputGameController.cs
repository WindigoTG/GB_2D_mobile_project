using Tools;
using UnityEngine;

public class InputGameController : BaseController
{
    private BaseInputView _view;

    public InputGameController(SubscriptionProperty<float> leftMove, SubscriptionProperty<float> rightMove, Car car, InputMethod inputMethod)
    {
        _view = LoadView(inputMethod);
        _view.Init(leftMove, rightMove, car.Speed);
    }

    private BaseInputView LoadView(InputMethod inputMethod)
    {
        var objView = Object.Instantiate(ResourceLoader.LoadPrefab(GetPrefabPath(inputMethod)));
        AddGameObject(objView);
        
        return objView.GetComponent<BaseInputView>();
    }

    private string GetPrefabPath(InputMethod inputMethod)
    {
        switch (inputMethod)
        {
            case InputMethod.Acceleration:
            default:
                {
                    return References.ACCELERATION_CONTROL_PREFAB_PATH;
                }
            case InputMethod.Gyroscope:
                {
                    return References.GYROSCOPE_CONTROL_PREFAB_PATH;
                }
            case InputMethod.Joystick:
                {
                    return References.JOYSTICK_CONTROL_PREFAB_PATH;
                }
            case InputMethod.Swipe:
                {
                    return References.SWIPE_CONTROL_PREFAB_PATH;
                }
            case InputMethod.Tap:
                {
                    return References.TAP_CONTROL_PREFAB_PATH;
                }
        }
    }
}


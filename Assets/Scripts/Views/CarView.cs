using Tools;
using UnityEngine;

public class CarView : MonoBehaviour
{
    [SerializeField] Transform[] _wheels;
    [SerializeField] float _wheelSpinSpeed;

    private SubscriptionProperty<float> _leftMove;
    private SubscriptionProperty<float> _rightMove;

    public virtual void Init(SubscriptionProperty<float> leftMove, SubscriptionProperty<float> rightMove)
    {
        _leftMove = leftMove;
        _rightMove = rightMove;

        _leftMove.SubscribeOnChange(SpinWheels);
        _rightMove.SubscribeOnChange(SpinWheels);
    }

    private void OnDestroy()
    {
        _leftMove.UnSubscriptionOnChange(SpinWheels);
        _rightMove.UnSubscriptionOnChange(SpinWheels);
    }

    protected void OnLeftMove(float value)
    {
        _leftMove.Value = value;
    }

    protected void OnRightMove(float value)
    {
        _rightMove.Value = value;
    }

    private void SpinWheels(float value)
    {
        foreach (Transform wheel in _wheels)
            wheel.Rotate(0, 0, -value * _wheelSpinSpeed);
    }
} 


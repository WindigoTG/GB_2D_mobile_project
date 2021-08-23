using JoostenProductions;
using Tools;
using UnityEngine;

public class InputAccelerationView : BaseInputView
{
    public override void Init(SubscriptionProperty<float> leftMove, SubscriptionProperty<float> rightMove, float speed)
    {
        base.Init(leftMove, rightMove, speed);
        UpdateManager.SubscribeToUpdate(Move);
    }

    private void OnDestroy()
    {
        UpdateManager.UnsubscribeFromUpdate(Move);
    }

    private void Move()
    {
        var direction = Vector3.zero;
        direction.x = Input.acceleration.x;
        
        if (direction.sqrMagnitude > 1)
            direction.Normalize();
        if (direction.x > 0)
            OnRightMove(direction.sqrMagnitude / 20 * _speed);
        else if (direction.x < 0)
            OnLeftMove(-direction.sqrMagnitude / 20 * _speed);
    }
}


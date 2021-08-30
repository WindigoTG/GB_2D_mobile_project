using Profile;
using Tools;
using UnityEngine;

public class DistanceTracker
{
    private readonly PlayerProfile _playerProfile;

    private SubscriptionProperty<float> _leftMove;
    private SubscriptionProperty<float> _rightMove;

    private float _distanceTraveled;

    public DistanceTracker(PlayerProfile playerProfile)
    {
        _playerProfile = playerProfile;
    }

    public virtual void Init(SubscriptionProperty<float> leftMove, SubscriptionProperty<float> rightMove)
    {
        _leftMove = leftMove;
        _rightMove = rightMove;

        _leftMove.SubscribeOnChange(CountDistance);
        _rightMove.SubscribeOnChange(CountDistance);

        _distanceTraveled = 0;
    }

    private void CountDistance(float value) 
    {
        _distanceTraveled += value;

        Debug.Log(_distanceTraveled);

        if (_distanceTraveled > 50)
        {
            _playerProfile.CurrentState.Value = GameState.Garage;
        }
    }
}

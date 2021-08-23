using JoostenProductions;
using Tools;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputTapView : BaseInputView, IPointerClickHandler
{
    [SerializeField] private float _acceleration;
    [SerializeField] private float _deceleration;
    [SerializeField] private float _maxSpeed;
    private float _currentSpeed;

    public override void Init(SubscriptionProperty<float> leftMove, SubscriptionProperty<float> rightMove, float speed)
    {
        base.Init(leftMove, rightMove, speed);
        UpdateManager.SubscribeToUpdate(Move);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        int modifier = eventData.position.x > Screen.width / 2 ? 1 : -1;
        _currentSpeed = Mathf.Clamp(_currentSpeed + _acceleration * modifier, -_maxSpeed, _maxSpeed);
    }

    private void Move()
    {
        OnRightMove(_currentSpeed * Time.deltaTime * _speed);

        Decelerate();
    }

    private void Decelerate()
    {
        if (_currentSpeed > 0)
            _currentSpeed = Mathf.Clamp(_currentSpeed - _deceleration, 0f, _currentSpeed);
        else if (_currentSpeed < 0)
            _currentSpeed = Mathf.Clamp(_currentSpeed + _deceleration, _currentSpeed, 0f);
    }

    private void OnDestroy()
    {
        UpdateManager.UnsubscribeFromUpdate(Move);
    }
}

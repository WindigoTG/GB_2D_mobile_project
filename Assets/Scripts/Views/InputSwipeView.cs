using JoostenProductions;
using System;
using Tools;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputSwipeView : BaseInputView, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] [Range (0f, 1f)] private float _swipeRate = 0.5f;
    [SerializeField] private float _deceleration;
    private float _currentSpeed;
    private bool _isDragging;

    public override void Init(SubscriptionProperty<float> leftMove, SubscriptionProperty<float> rightMove, float speed)
    {
        base.Init(leftMove, rightMove, speed);
        UpdateManager.SubscribeToUpdate(Move);
    }

    public void OnDrag(PointerEventData eventData)
    {
        _currentSpeed = eventData.delta.x;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _isDragging = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _isDragging = false;
    }

    private void Move()
    {
        OnRightMove(_currentSpeed * Time.deltaTime * _swipeRate * _speed);
        
        if (!_isDragging)
        {
            Decelerate();
        }
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

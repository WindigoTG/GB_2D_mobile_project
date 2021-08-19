using UnityEngine;

public class BackgroundView : MonoBehaviour
{
    [SerializeField] private float _relativeSpeedRate;

    private float _size;

    public void CalculateSize()
    {
        BoxCollider2D collider = gameObject.AddComponent<BoxCollider2D>();
        _size = collider.size.x;
        Destroy(collider);
    }

    public void Move(float value)
    {
        transform.position += Vector3.right * value * _relativeSpeedRate;
        
        var position = transform.position;

        if (position.x <= -(_size * 2))
            transform.position = transform.position.Change(x: transform.position.x + _size * 3);
        else if (transform.position.x >= _size * 2)
            transform.position = transform.position.Change(x: transform.position.x - _size * 3);
    }
}


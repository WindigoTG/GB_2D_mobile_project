using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using System;

[RequireComponent(typeof(Renderer))]
public class TweenCube : MonoBehaviour
{
    [SerializeField]
    private float _duration;

    [SerializeField]
    private Vector3 _endValue;

    [SerializeField]
    private Color _color;

    [SerializeField]
    private PathType _pathType = PathType.Linear;

    [SerializeField]
    private Transform[] _points;

    [SerializeField]
    private int _countLoops;

    [SerializeField]
    private float _positionX;

    [SerializeField]
    private float _endScale;

    [SerializeField]
    private Ease _loopEase;

    private List<Vector3> _pointPosition = new List<Vector3>();

    private Material _material;

    private void Awake()
    {
        _material = GetComponent<Renderer>().material;
    }

    private void Start()
    {
        //transform.DOMove(_endValue, _duration).From();
        _material.DOColor(_color, _duration);

        //DoPunch();

        //DoPath();

        DoComplexTween();
    }

    private void DoComplexTween()
    {
        Sequence sequence = DOTween.Sequence();

        sequence.Append(transform.DOMoveX(_positionX, _duration).SetLoops(_countLoops).SetEase(_loopEase));
        sequence.Insert(0, transform.DOScale(_endScale, _duration));
        sequence.Insert(1, transform.DOJump(Vector3.forward, 5, 5, _duration));

    }

    private void DoPunch()
    {
        transform.DOPunchPosition(_endValue, _duration);
        transform.DOPunchRotation(_endValue, _duration);
        transform.DOPunchScale(_endValue, _duration);
    }

    private void DoPath()
    {
        foreach (var point in _points)
            _pointPosition.Add(point.position);

        transform.DOPath(_pointPosition.ToArray(), _duration, _pathType);
    }
}



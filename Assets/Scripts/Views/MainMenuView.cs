using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using UnityEngine.EventSystems;

public class MainMenuView : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] private Button _buttonStart;
    [SerializeField] private Dropdown _controlMethodDropdown;
    [SerializeField] private Transform _trail;
        
    public void Init(UnityAction startGame, UnityAction<int> receiveDropdownValue)
    {
        _buttonStart.onClick.AddListener(startGame);

        FillDropdownOptions();

        _controlMethodDropdown.onValueChanged.AddListener(receiveDropdownValue);
    }

    private void FillDropdownOptions()
    {
        List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();

        for (int i = 0; i < Enum.GetNames(typeof(InputMethod)).Length; i++)
        {
            options.Add(new Dropdown.OptionData(Enum.GetName(typeof(InputMethod), i)));
        }

        _controlMethodDropdown.ClearOptions();
        _controlMethodDropdown.AddOptions(options);
    }

    public void OnDrag(PointerEventData eventData)
    {
        var newPos = Camera.main.ScreenToWorldPoint(eventData.position);
        _trail.position = _trail.position.Change(x: newPos.x, y: newPos.y);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _trail.gameObject.SetActive(true);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _trail.gameObject.SetActive(false);
    }

    protected void OnDestroy()
    {
        _buttonStart.onClick.RemoveAllListeners();
    }

    public string SelectedControlMethod => ((InputMethod)_controlMethodDropdown.value).ToString();
}


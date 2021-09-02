using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemView : MonoBehaviour, IPointerClickHandler
{
    Image _image;

    IItem _item;

    public event UnityAction<IItem> Selected;

    public void Init(IItem item)
    {
        _item = item;
        _image = gameObject.AddComponent<Image>();
        _image.sprite = _item.Info.ItemSprite;
        _image.color = _item.Info.ItemColor;
        gameObject.AddComponent<GraphicRaycaster>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Selected?.Invoke(_item);
    }
}

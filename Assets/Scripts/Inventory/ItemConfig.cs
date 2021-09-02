using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Inventory/Item", order = 0)]
public class ItemConfig : ScriptableObject
{
    public int Id;
    public string Title;
    public Sprite ItemSprite;
    public Color ItemColor = Color.white;
}


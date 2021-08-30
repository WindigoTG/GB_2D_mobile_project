using UnityEngine;

[CreateAssetMenu(fileName = "Upgrade item", menuName = "Inventory/Upgrade item", order = 0)]
public class UpgradeItemConfig : ScriptableObject
{
    public ItemConfig ItemConfig;
    public UpgradeType Type;
    public float Value;

    public int Id => ItemConfig.Id;
    public Sprite ItemSprite => ItemConfig.ItemSprite;
    public Color ItemColor => ItemConfig.ItemColor;
}


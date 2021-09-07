using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeItemConfigDataSource", menuName = "Inventory/UpgradeItemConfigDataSource", order = 0)]
public class UpgradeItemConfigDataSource : ScriptableObject
{
    public List<UpgradeItemConfig> ItemConfigs;
}


using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AbilityItemConfigDataSource", menuName = "Abilities/AbilityItemConfigDataSource", order = 1)]
public class AbilityItemConfigDataSource : ScriptableObject
{
    public List<AbilityItemConfig> AbilityConfigs;
}

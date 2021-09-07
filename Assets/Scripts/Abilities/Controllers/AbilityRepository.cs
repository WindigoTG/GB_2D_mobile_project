using System.Collections.Generic;

public class AbilityRepository : IRepository<int, IAbility>
{
    private readonly Dictionary<int, IAbility> _abilityMapById = new Dictionary<int, IAbility>();

    public AbilityRepository(List<AbilityItemConfig> itemConfigs)
    {
        PopulateItems(ref _abilityMapById, itemConfigs);
    }

    private void PopulateItems(
        ref Dictionary<int, IAbility> upgradeHandlersMapByType,
        List<AbilityItemConfig> configs)
    {
        foreach (var config in configs)
        {
            if (upgradeHandlersMapByType.ContainsKey(config.Id)) continue;
            upgradeHandlersMapByType.Add(config.Id, CreateAbilityByType(config));
        }
    }

    private IAbility CreateAbilityByType(AbilityItemConfig config)
    {
        switch (config.type)
        {
            case AbilityType.Gun:
                return new GunAbility(config);
            default:
                return StubAbility.Default;
        }
    }

    public IReadOnlyDictionary<int, IAbility> Collection => _abilityMapById;
}


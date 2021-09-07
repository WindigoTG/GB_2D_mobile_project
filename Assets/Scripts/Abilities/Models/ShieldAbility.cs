using UnityEngine;

public class ShieldAbility : IAbility
{
    private readonly AbilityItemConfig _config;

    public ShieldAbility(AbilityItemConfig config)
    {
        _config = config;
    }

    public void Use(IAbilityActivator activator)
    {
        Object.Instantiate(_config.view);
    }
}

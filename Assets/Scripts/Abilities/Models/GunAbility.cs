using UnityEngine;

public class GunAbility : IAbility
{
    private readonly AbilityItemConfig _config;

    public GunAbility(AbilityItemConfig config)
    {
        _config = config;
    }

    public void Use(IAbilityActivator activator)
    {
        var projectile = Object.Instantiate(_config.view).GetComponent<Rigidbody2D>();
        projectile.AddForce(activator.GetViewObject().transform.right * _config.value, ForceMode2D.Force);
    }
}


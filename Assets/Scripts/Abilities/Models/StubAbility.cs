using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StubAbility : IAbility
{
    public StubAbility()
    {

    }

    public void Use(IAbilityActivator activator)
    {
        
    }

    public static StubAbility Default => new StubAbility();
}

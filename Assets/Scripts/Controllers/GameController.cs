using Tools;
using Profile;
using UnityEngine;

public class GameController : BaseController
{
    public GameController(PlayerProfile playerProfile, Transform placeForUi)
    {
        var leftMoveDiff = new SubscriptionProperty<float>();
        var rightMoveDiff = new SubscriptionProperty<float>();
        
        var tapeBackgroundController = new TapeBackgroundController(leftMoveDiff, rightMoveDiff);
        AddController(tapeBackgroundController);
        
        var inputGameController = new InputGameController(leftMoveDiff, rightMoveDiff, playerProfile.CurrentCar, playerProfile.InputMethod);
        AddController(inputGameController);
            
        var carController = new CarController(leftMoveDiff, rightMoveDiff);
        AddController(carController);

        var abilityController = ConfigureAbilityController(placeForUi, carController);
        AddController(abilityController as BaseController);
    }

    private IAbilitiesController ConfigureAbilityController(Transform placeForUi, IAbilityActivator abilityActivator)
    {
        var abilityItemsConfigCollection = ContentDataSourceLoader.LoadAbilityItemConfigs(References.ABILITY_ITEM_CONFIG_DATA_SOURCE_PATH);
        var abilityRepository = new AbilityRepository(abilityItemsConfigCollection);
        var abilityCollectionView = ResourceLoader.LoadAndInstantiateObject<AbilityCollectionView>(References.ABILITY_COLLECTION_PREFAB_PATH, placeForUi, false);
        AddGameObject(abilityCollectionView.gameObject);

        var inventoryModel = new InventoryModel();
        var abilitiesController = new AbilitiesController(abilityRepository, inventoryModel, abilityCollectionView, abilityActivator);
        AddController(abilitiesController);

        return abilitiesController;
    }

}


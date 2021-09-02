public class AbilitiesController : BaseController
{
    private readonly IInventoryModel _inventoryModel;
    private readonly IAbilityRepository _abilityRepository;
    private readonly IAbilityCollectionView _abilityCollectionView;
    private readonly IAbilityActivator _abilityActivator;

    public AbilitiesController(
        IAbilityActivator abilityActivator,
        IInventoryModel inventoryModel,
        IAbilityRepository abilityRepository,
        IAbilityCollectionView abilityCollectionView)
    {
        _abilityActivator = abilityActivator;
        _inventoryModel = inventoryModel;
        _abilityRepository = abilityRepository;
        _abilityCollectionView = abilityCollectionView;
        _abilityCollectionView.Display(_inventoryModel.GetEquippedItems());
    }

    private void OnAbilityUseRequested(object sender, IItem e)
    {
        if (_abilityRepository.AbilityMapByItemId.TryGetValue(e.Id, out var ability))
            ability.Apply(_abilityActivator);
    }

}


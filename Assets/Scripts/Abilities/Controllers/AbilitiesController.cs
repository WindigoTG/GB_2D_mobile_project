public class AbilitiesController : BaseController, IAbilitiesController
{
    private readonly IRepository<int, IAbility> _abilityRepository;
    private readonly IInventoryModel _inventoryModel;
    private readonly IAbilityCollectionView _abilityCollectionView;
    private readonly IAbilityActivator _carController;

    public AbilitiesController(
           IRepository<int, IAbility> abilityRepository,
           IInventoryModel inventoryModel,
           IAbilityCollectionView abilityCollectionView,
           IAbilityActivator abilityActivator)
    {
        _abilityRepository = abilityRepository;
        _inventoryModel = inventoryModel;
        _abilityCollectionView = abilityCollectionView;
        _carController = abilityActivator;

        SetupView(_abilityCollectionView);
    }

    private void SetupView(IAbilityCollectionView view)
    {
        view.UseRequested += OnAbilityUseRequested;
    }

    private void CleanupView(IAbilityCollectionView view)
    {
        view.UseRequested -= OnAbilityUseRequested;
    }


    private void OnAbilityUseRequested(object sender, IItem e)
    {
        if (_abilityRepository.Collection.TryGetValue(e.Id, out var ability))
        {
            ability.Use(_carController);
        }
    }

    public void ShowAbilities()
    {
        _abilityCollectionView.Show();
        _abilityCollectionView.Display(_inventoryModel.GetEquippedItems());
    }

    protected override void OnDispose()
    {
        CleanupView(_abilityCollectionView);
        base.OnDispose();
    }

}


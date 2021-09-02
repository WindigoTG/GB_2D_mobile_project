using System.Collections.Generic;

public class UpgradeHandlersRepository : BaseController
{
    private Dictionary<int, IUpgradeCarHandler> _upgradeItemsMapById = new Dictionary<int, IUpgradeCarHandler>();

    public UpgradeHandlersRepository(List<UpgradeItemConfig> upgradeItemConfigs)
    {
        PopulateItems(upgradeItemConfigs);
    }

    public IReadOnlyDictionary<int, IUpgradeCarHandler> UpgradeItems => _upgradeItemsMapById;

    private void PopulateItems(List<UpgradeItemConfig> configs)
    {
        foreach (var config in configs)
        {
            if (_upgradeItemsMapById.ContainsKey(config.Id)) continue;
            _upgradeItemsMapById.Add(config.Id, CreateHandlerByType(config));
        }
    }

    private IUpgradeCarHandler CreateHandlerByType(UpgradeItemConfig config)
    {
        switch (config.Type)
        {
            case UpgradeType.Speed:
                return new SpeedUpgradeCarHandler(config.Value);
            default:
                return StubUpgradeCarHandler.Default;
        }
    }
    protected override void OnDispose()
    {
        _upgradeItemsMapById.Clear();
        _upgradeItemsMapById = null;
    }
}


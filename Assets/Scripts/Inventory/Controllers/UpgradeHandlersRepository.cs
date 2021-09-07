using System.Collections.Generic;

public class UpgradeHandlersRepository : IRepository<int, IUpgradeCarHandler>
{
    private readonly Dictionary<int, IUpgradeCarHandler> _upgradeItemsMapById = new Dictionary<int, IUpgradeCarHandler>();

    public UpgradeHandlersRepository(
        List<UpgradeItemConfig> upgradeItemConfigs)
    {
        PopulateItems(ref _upgradeItemsMapById, upgradeItemConfigs);
    }

    private void PopulateItems(
        ref Dictionary<int, IUpgradeCarHandler> upgradeHandlersMapByType,
        List<UpgradeItemConfig> configs)
    {
        foreach (var config in configs)
        {
            if (upgradeHandlersMapByType.ContainsKey(config.Id)) continue;
            upgradeHandlersMapByType.Add(config.Id, CreateHandlerByType(config));
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

    public IReadOnlyDictionary<int, IUpgradeCarHandler> Collection => _upgradeItemsMapById;
}


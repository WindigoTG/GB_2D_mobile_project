using System.Collections.Generic;

public static class ContentDataSourceLoader
{
    public static List<UpgradeItemConfig> LoadUpgradeItemConfigs(string path)
    {
        var config = ResourceLoader.LoadObject<UpgradeItemConfigDataSource>(path);
        return config == null ? new List<UpgradeItemConfig>() : config.ItemConfigs;
    }

    public static List<AbilityItemConfig> LoadAbilityItemConfigs(string path)
    {
        var config = ResourceLoader.LoadObject<AbilityItemConfigDataSource>(path);
        return config == null ? new List<AbilityItemConfig>() : config.AbilityConfigs;
    }
}


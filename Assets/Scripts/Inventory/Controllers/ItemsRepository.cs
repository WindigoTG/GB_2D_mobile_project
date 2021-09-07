using System.Collections.Generic;

public class ItemsRepository : BaseController, IRepository<int, IItem>
{
    private Dictionary<int, IItem> _itemsMapById = new Dictionary<int, IItem>();

    public ItemsRepository(List<ItemConfig> itemConfigs)
    {
        PopulateItems(itemConfigs);
    }

    public IReadOnlyDictionary<int, IItem> Items => _itemsMapById;

    public IReadOnlyDictionary<int, IItem> Collection => throw new System.NotImplementedException();

    private void PopulateItems(List<ItemConfig> configs)
    {
        foreach (var config in configs)
        {
            if (_itemsMapById.ContainsKey(config.Id))
                continue;

            _itemsMapById.Add(config.Id, CreateItem(config));
        }
    }

    private IItem CreateItem(ItemConfig config)
    {
        return new Item
        {
            Id = config.Id,
            Info = new ItemInfo 
            { 
                Title = config.Title ,
                ItemSprite = config.ItemSprite,
                ItemColor = config.ItemColor
            },
        };
    }

    protected override void OnDispose()
    {
        _itemsMapById.Clear();
        _itemsMapById = null;
    }
}


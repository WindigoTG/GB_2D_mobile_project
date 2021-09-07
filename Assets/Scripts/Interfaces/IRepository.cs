using System.Collections.Generic;

public interface IRepository<TKey, TValue>
{
    IReadOnlyDictionary<TKey, TValue> Collection { get; }
}


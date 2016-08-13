using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public enum PlayerId { }

public class PlayerItemManager : NetworkBehaviour
{
    public static PlayerItemManager inst;
    private readonly Dictionary<PlayerId, ItemType> _items = new Dictionary<PlayerId, ItemType>();

    void Awake()
    {
        if (inst != null)
            Debug.LogError("inst not null");
        inst = this;
    }

    void OnDestroy()
    {
        if (this == inst) inst = null;
        else Debug.LogError("inst null");
    }

    public void Set(PlayerId playerId, ItemType itemType)
    {
        if (itemType == ItemType.None)
        {
            Debug.LogError("trying to set none");
            return;
        }

        _items[playerId] = itemType;
    }

    public void UnSet(PlayerId playerId)
    {
        _items.Remove(playerId);
    }

    public ItemType? Find(PlayerId playerId)
    {
        ItemType itemFound;
        if (_items.TryGetValue(playerId, out itemFound))
            return itemFound;
        return null;
    }

    public ItemType? FindAndUnSet(PlayerId playerId)
    {
        var ret = Find(playerId);
        UnSet(playerId);
        return ret;
    }

    public IEnumerable<KeyValuePair<PlayerId, ItemType>> Each()
    {
        foreach (var kv in _items)
            yield return kv;
    }
}
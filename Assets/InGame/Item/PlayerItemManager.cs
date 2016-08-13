using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerItemManager : NetworkBehaviour
{
    public static PlayerItemManager inst;
    private readonly Dictionary<int, ItemType> _items = new Dictionary<int, ItemType>();

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

    [ClientRpc]
    public void RpcSet(int playerId, ItemType itemType)
    {
        if (itemType == ItemType.None)
        {
            Debug.LogError("trying to set none");
            return;
        }

        _items[playerId] = itemType;
    }

    [ClientRpc]
    private void RpcUnSet(int playerId)
    {
        _items.Remove(playerId);
    }

    public ItemType? Find(int playerId)
    {
        ItemType itemFound;
        if (_items.TryGetValue(playerId, out itemFound))
            return itemFound;
        return null;
    }

    public ItemType? FindAndUnSet(int playerId)
    {
        var ret = Find(playerId);
        RpcUnSet(playerId);
        return ret;
    }

    public IEnumerable<KeyValuePair<int, ItemType>> Each()
    {
        foreach (var kv in _items)
            yield return kv;
    }
}
using System.Collections;
using UnityEngine;

public class HUDPlayerItemPanel : MonoBehaviour
{
    public HUDPlayerItem[] itemFrames;

    void Start()
    {
        for (var i = 0; i != itemFrames.Length; ++i)
            itemFrames[i].SetColorWithPlayerOrder(i + 1);
    }

    void OnEnable()
    {
        StartCoroutine(CoroutineRefresh());
    }

    private IEnumerator CoroutineRefresh()
    {
        while (true)
        {
            Refresh();
            yield return new WaitForSeconds(0.3f);
        }
    }

    private void Refresh()
    {
        if (ClientGameManager.inst == null) return;
        if (PlayerItemManager.inst == null) return;

        var setFrames = new bool[4];
        foreach (var kv in PlayerItemManager.inst.Each())
        {
            var netId = kv.Key;
            var playerOrder = 0;

            if (!ClientGameManager.inst.connIdToPlayerOrder.TryGetValue(netId, out playerOrder))
            {
                Debug.LogWarning("player order not found: " + netId);
                continue;
            }

            if (playerOrder == 0 || playerOrder > 4 || playerOrder > itemFrames.Length)
            {
                Debug.LogWarning("index out of range: " + playerOrder);
                continue;
            }

            var itemType = kv.Value;
            itemFrames[playerOrder - 1].SetIcon(itemType);
            setFrames[playerOrder - 1] = true;
        }

        for (var i = 0; i != setFrames.Length; ++i)
        {
            if (setFrames[i]) continue;
            itemFrames[i].ResetIcon();
        }
    }
}

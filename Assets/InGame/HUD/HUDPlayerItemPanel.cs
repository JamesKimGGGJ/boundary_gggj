using System.Collections;
using UnityEngine;
using System.Linq;

public class HUDPlayerItemPanel : MonoBehaviour
{
    public HUDPlayerItem[] itemFrames;

    void Start()
    {
        for (var i = 0; i != itemFrames.Length; ++i)
        {
            itemFrames[i].SetColorWithPlayerOrder(i);
        }
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
            itemFrames[kv.Key].SetIcon(kv.Value);
            setFrames[kv.Key] = true;
        }

        for (var i = 0; i != setFrames.Length; ++i)
        {
            if (setFrames[i]) continue;
            itemFrames[i].ResetIcon();
        }
    }
}

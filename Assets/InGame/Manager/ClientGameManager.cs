using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ClientGameManager : NetworkBehaviour
{
    public static ClientGameManager inst;
    public StormEffect stormEffect;

    void Awake()
    {
        if (inst == null)
            inst = this;
        else
            Debug.LogWarning("inst not null");
    }

    void Start()
    {
        GameGlobalVar.Reset();
        stormEffect.stormSize = GameGlobalVar.stormRadius;
    }

    void OnDestroy()
    {
        if (inst != this)
            inst = this;
        else
            Debug.LogWarning("inst not match");
    }

    [ClientRpc]
    public void RpcSetStormRadius(float value)
    {
        GameGlobalVar.stormRadius = value;
        stormEffect.stormSize = value;
    }

    [ClientRpc]
    public void RpcWin(int winnerId)
    {
        // TODO
        Debug.Log("winner is " + winnerId);
        bool win = false;
        try
        {
            win = (Lobby.playerId == winnerId);
        }
        catch { }
        GameOver.instance.SetGameOver(win);
    }
}
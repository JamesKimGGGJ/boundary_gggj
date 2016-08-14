using UnityEngine;
using UnityEngine.Networking;

public class GameMessagePasser : NetworkBehaviour
{
    public static GameMessagePasser inst;

    private HostGameManager hostGameManagerCache;
    private HostGameManager hostGameManager
    {
        get
        {
            if (hostGameManagerCache == null)
                hostGameManagerCache = FindObjectOfType<HostGameManager>();
            return hostGameManagerCache;
        }
    }

    void Awake()
    {
        inst = this;
        Player.OnSpawn += CmdPlayerSpawn;
        Player.OnDie += CmdPlayerDie;
    }

    void OnDestroy()
    {
        Player.OnSpawn -= CmdPlayerSpawn;
        Player.OnDie -= CmdPlayerDie;
        inst = null;
    }

    [Command]
    public void CmdPlayerSpawn(int playerId)
    {
        if (!isServer) return;
        hostGameManager.OnPlayerSpawn(playerId);
    }

    [Command]
    public void CmdPlayerDie(int playerId)
    {
        if (!isServer) return;
        hostGameManager.OnPlayerDie(playerId);
    }

    [ClientRpc]
    public void RpcWin(int winnerId)
    {
        // TODO
        Debug.Log("winner is " + winnerId);
    }
}
